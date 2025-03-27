using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ParcelPointApi.Data.Interface.ParcelLogs;
using ParcelPointApi.Data.Repositories;
using ParcelPointApi.Hubs;
using ParcelPointDB.Data.Repositories;
using System.Linq;
using System.Net.Sockets;

namespace ParcelPointApi.Services
{
    public interface IParcelLogsService
    {
        Task<IEnumerable<ParcelLog>> GetParcelLogsByIdAsync(Guid id);

        Task<IEnumerable<ParcelLogSummaryDto>> GetParcelLogsSummaryAsync();
        Task<ParcelLogsCountsDto> GetParcelLogsCountsAsync();

        Task<string> CreateParcelLogsAsync(string user_number, int size);

        Task<List<String>> GetActiveParcelsAsync(int bioID, int mode);
    }

    public class ParcelLogsService : IParcelLogsService
    {
        private readonly IUserRepository _usersRepository;
        private readonly IParcelLogsRepository _parcelLogsRepository;
        private readonly IHubContext<HomeHub> _hubContext;
        private readonly UserConnectionManager _connectionManager;
            private readonly ParcelPointDbContext _context;
        public ParcelLogsService(ParcelPointDbContext dbCon, IParcelLogsRepository parcelRepository, IUserRepository userRepository, IHubContext<HomeHub> hubContext, UserConnectionManager connectionManager)
        {
            _parcelLogsRepository = parcelRepository;
            _usersRepository = userRepository;
            _hubContext = hubContext;
            _connectionManager = connectionManager;
                _context = dbCon;
        }

        public async Task<IEnumerable<ParcelLog>> GetParcelLogsByIdAsync(Guid id)
        {
            return await _parcelLogsRepository.GetParcelLogsByIdAsync(id);
        }

        public async Task<IEnumerable<ParcelLogSummaryDto>> GetParcelLogsSummaryAsync()
        {
            return await _parcelLogsRepository.GetParcelLogsSummaryAsync();
        }

        public async Task<ParcelLogsCountsDto> GetParcelLogsCountsAsync()
        {
            return await _parcelLogsRepository.GetParcelLogsCountsAsync();
        }
        public async Task<List<String>> GetActiveParcelsAsync(int useBioId, int mode)
        {
            var tenMinutesLater = DateTime.Now.AddMinutes(-1440);

            // Get user ID of the scanned user
            var userBioIds = await _context.UserbioTemps
                .Where(b => b.BioId == useBioId)
                .Select(b => b.OwnerId)
                .FirstOrDefaultAsync();

            // Get Name of the scanned user
            var retriever = await _context.UserInformations
                    .Where(u => u.UserId == userBioIds)
                    .Select(i => i.FirstName + " " + i.LastName)
                    .SingleOrDefaultAsync();

            Console.WriteLine(userBioIds);

            var lockerNumbers = new List<string>();
            var parcelLogs = new List<ParcelLog>();
    
            if (mode == 1)
            {
                // Step 2: Get all parcels linked to this user where:
                // - `RetrievedBy` is NULL (not yet picked up)
                // - `CreatedAt` is within the last 10 minutes
                lockerNumbers = await _context.ParcelLogs
                    .Where(p => p.UserId == userBioIds && p.RetrievedBy == null && p.CreatedAt >= tenMinutesLater)
                    .Select(p => p.LockerNumber)
                    .ToListAsync();

                // Update
                parcelLogs = await _context.ParcelLogs
                    .Where(p => p.UserId == userBioIds && p.RetrievedBy == null && p.CreatedAt >= tenMinutesLater)
                    .ToListAsync();
            }

            else if (mode == 2)
            {
                var ownerIds = new List<Guid>();
                var association = await _context.UserGroupMembers
                    .Where(user => user.MemberId == userBioIds)
                    .ToListAsync();

                foreach(var associated in association)
                {
                    if ((bool)associated.IsAuthorized)
                    {
                        ownerIds.Add(await _context.UserGroups.Where(grp => grp.Id == associated.GroupId).Select(i => i.OwnerId).FirstOrDefaultAsync() ?? new Guid());
                        await _context.Database.ExecuteSqlRawAsync("UPDATE USER_GROUP_MEMBERS SET is_authorized = {0} WHERE id = {1}", false, associated.Id);
                    }
                }

                foreach(var owner in ownerIds)
                {
                    lockerNumbers = await _context.ParcelLogs
                    .Where(p => ownerIds.Contains(owner)
                                && p.RetrievedBy == null
                                && p.CreatedAt >= tenMinutesLater)
                    .Select(p => p.LockerNumber)
                    .ToListAsync();

                    // Then fetch the actual ParcelLog objects in a similar way:
                    parcelLogs = await _context.ParcelLogs
                        .Where(p => ownerIds.Contains(owner)
                                    && p.RetrievedBy == null
                                    && p.CreatedAt >= tenMinutesLater)
                        .ToListAsync();
                }
            }
            else
            {
                throw new InsufficientMemoryException();
            }

            foreach (var parcel in parcelLogs)
            {
                parcel.RetrievedBy = retriever;  // Assign the userBioIds (or current user ID) as needed
                parcel.RetrievedAt = DateTime.Now;  // Use DateTime.Now without parentheses
                parcel.Status = "Picked Up";

                // New Notification
                var newNotif = new NotificationLog
                {
                    Id = Guid.NewGuid(),
                    IsRead = false,
                    Title = "Parcel Retrieved",
                    Context = "Your parcel has been retrieved by " + (parcel.UserId == userBioIds ? "You" : retriever),
                    CreatedAt = DateTime.Now,
                    LockerNumber = int.Parse(parcel.LockerNumber),
                    RetrievedBy = (parcel.UserId == userBioIds ? "You" : retriever),
                    UserId = parcel.UserId ?? Guid.Empty
                };

                // Add the notification to the context
                await _context.NotificationLogs.AddAsync(newNotif);

                // Update Locker make it available
                await _context.Database.ExecuteSqlRawAsync("UPDATE Table_Status SET owner_id = {0}, is_open = {2} WHERE locker_number = {1}", null, parcel.LockerNumber, true);
            }

            // Save all changes to the database
            await _context.SaveChangesAsync();

            // Get All Notif and Parcel lOgs
            var notifs = await _context.NotificationLogs
                .Where(notif => notif.UserId == userBioIds)
                .ToListAsync();
            var parcels = await _context.ParcelLogs
                .Where(parcel => parcel.UserId == userBioIds)
                .ToListAsync();

            var combinedData = new CombinedListTableDTO
            {
                Parcel = parcels,
                Notification = notifs
            };

            var connectionIds = _connectionManager.GetConnections(userBioIds);

            foreach(var connectionId in connectionIds)
            {
                var id = connectionId;
                await _hubContext.Clients.Client(id).SendAsync("ParcelAndNotifUpdate", combinedData);
            };

            return lockerNumbers.Where(x => x != null).ToList();
        }

    public async Task<string> CreateParcelLogsAsync(string user_number, int size)
        {
            // Verify User by Contact Number
            var user = await _usersRepository.GetUserByNumberAsync(user_number);

            if (user == null) return "No User Found.";

            // Check Size if available
            var checkLockerAvailability = await _usersRepository.CheckLockerAvailability(user.Id, size);
            Console.WriteLine("NOT AVAILABLE: " + checkLockerAvailability);
            if (!checkLockerAvailability) return "Locker not available.";

            // Create Parcel Log
            var homeUpdate = await _parcelLogsRepository.CreateParcelLogsAsync(user.Id, size);

            if (homeUpdate == null) return "Failed Creating Logs";
            // Get the Connection IDs for the User
            var connectionIds = _connectionManager.GetConnections(user.Id);

            Console.WriteLine(homeUpdate.ToString());

            // Send the notification to each connected client of the user
            foreach (var connectionId in connectionIds)
            {
                var id = connectionId;
                await _hubContext.Clients.Client(id).SendAsync("HomeListUpdate", homeUpdate);
            }

            return "Success";
        }
    }
}
 