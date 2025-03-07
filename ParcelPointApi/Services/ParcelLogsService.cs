using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ParcelPointApi.Data.Interface.ParcelLogs;
using ParcelPointApi.Data.Repositories;
using ParcelPointApi.Hubs;
using ParcelPointDB.Data.Repositories;

namespace ParcelPointApi.Services
{
    public interface IParcelLogsService
    {
        Task<IEnumerable<ParcelLog>> GetParcelLogsByIdAsync(Guid id);

        Task<IEnumerable<ParcelLogSummaryDto>> GetParcelLogsSummaryAsync();
        Task<ParcelLogsCountsDto> GetParcelLogsCountsAsync();

        Task<string> CreateParcelLogsAsync(string user_number, int size);

        Task<List<String>> GetActiveParcelsAsync(int bioID);
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
            public async Task<List<String>> GetActiveParcelsAsync(int useBioId)
            {
                var tenMinutesLater = DateTime.UtcNow.AddMinutes(10);

                // Step 1: Find all `BioId` entries that belong to the user
                var userBioIds = await _context.UserbioTemps
                    .Where(b => b.BioId == useBioId)
                    .Select(b => b.OwnerId)
                    .FirstOrDefaultAsync();

            Console.WriteLine(userBioIds);

                // Step 2: Get all parcels linked to this user where:
                // - `RetrievedBy` is NULL (not yet picked up)
                // - `CreatedAt` is within the last 10 minutes
                var lockerNumbers = await _context.ParcelLogs
                    .Where(p => p.UserId == userBioIds && p.RetrievedBy == null && p.CreatedAt <= tenMinutesLater)
                    .Select(p => p.LockerNumber)
                    .ToListAsync();


            // Update
            var parcelLogs = await _context.ParcelLogs
            .Where(p => p.UserId == userBioIds && p.RetrievedBy == null && p.CreatedAt <= tenMinutesLater)
            .ToListAsync();

            foreach (var parcel in parcelLogs)
            {
                parcel.RetrievedBy = await _context.UserInformations.Where(u => u.UserId == userBioIds).Select(i => i.FirstName + " " + i.LastName).SingleOrDefaultAsync();  // Assign the userBioIds (or current user ID) as needed
                parcel.RetrievedAt = DateTime.Now;  // Use DateTime.Now without parentheses
                parcel.Status = "Picked Up";
            }

            // Save all changes to the database
            await _context.SaveChangesAsync();

            return lockerNumbers;
            }

            public async Task<string> CreateParcelLogsAsync(string user_number, int size)
        {
            // Verify User by Contact Number
            var user = await _usersRepository.GetUserByNumberAsync(user_number);

            if (user == null) return "No User Found.";

            // Check Size if available
            // ...

            // Create Parcel Log
            var homeUpdate = await _parcelLogsRepository.CreateParcelLogsAsync(user.Id, 1);

            if (homeUpdate == null) return "Failed Creating Logs";
            // Get the Connection IDs for the User
            var connectionIds = _connectionManager.GetConnections(user.Id);

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
 