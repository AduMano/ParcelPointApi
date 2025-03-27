using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ParcelPointApi.Data.Interface.Users;
using ParcelPointApi.Data.Repositories;
using ParcelPointApi.Hubs;
using ParcelPointApi.Models;
using ParcelPointApi.Services;
using Sprache;

public interface ILockerRepository
{
    Task<IEnumerable<TableStatus>> GetLockerStatus(int[] ids);
    Task<IEnumerable<TableStatusAdmin>> GetLockerStatusAdmin();
    Task<bool> OpenEmptyLocker(string lockerNumber, Guid operatorID);
    Task<bool> OpenLockerTaken(string lockerNumber, string username, Guid operatorID);
    Task<bool> UpdateLockerStatus(int id);
}

public class LockerRepository : ILockerRepository
{
    private readonly ParcelPointDbContext _context;
    private readonly PasswordHelper _passwordHelper;
    private readonly IHubContext<HomeHub> _hubContext;
    private readonly UserConnectionManager _connectionManager;

    public LockerRepository(ParcelPointDbContext context, PasswordHelper passwordHelper, IHubContext<HomeHub> hubContext, UserConnectionManager connectionManager)
    {
        _context = context;
        _hubContext = hubContext;
        _connectionManager = connectionManager;
    }

    public async Task<bool> OpenEmptyLocker(string lockerNumber, Guid operatorID)
    {
        try
        {
            await _context.Database.ExecuteSqlRawAsync("UPDATE Table_Status SET is_open = 1 WHERE locker_number = " + lockerNumber);

            // Create Logs
            var log = new ActivityLog
            {
                ActionTitle = "Admin Opened a Locker",
                ActionContext = $"Admin opened locker #{lockerNumber}.",
                CreatedAt = DateTime.Now,
                CreatedBy = operatorID,
                Module = "Utilities",
                SubModule = "User Logs"
            };

            await _context.ActivityLogs.AddAsync(log);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> OpenLockerTaken(string lockerNumber, string username, Guid operatorID)
    {
        try
        {
            // Get Parcel logs and Update
            var user = await _context.Users.Where(u => u.Username == username).FirstOrDefaultAsync();
            var getParcelLog = await _context.ParcelLogs.Where(pl => pl.LockerNumber == lockerNumber && pl.UserId == user.Id).OrderByDescending(o => o.ArrivedAt).FirstAsync();

            var findParcel = await _context.ParcelLogs.FindAsync(getParcelLog.Id);
            var operatorUser = await _context.Users.FindAsync(operatorID);

            findParcel.Status = "Picked Up";
            findParcel.RetrievedAt = DateTime.Now;
            findParcel.RetrievedBy = operatorUser.Username;

            // Create Logs
            var log = new ActivityLog
            {
                ActionTitle = "Admin Opened a Locker",
                ActionContext = $"Admin {operatorUser.Username} opened {user.Username}'s locker and retrieved the parcel.",
                CreatedAt = DateTime.Now,
                CreatedBy = operatorID,
                Module = "Utilities",
                SubModule = "User Logs"
            };

            // Mobile Notif:
            var notification = new NotificationLog
            {
                Id = Guid.NewGuid(),
                Title = "Retrieval Parcel",
                Context = "Your parcel has been retrieved by Admin " + operatorUser.Username,
                CreatedAt = DateTime.Now,
                LockerNumber = Int32.Parse(lockerNumber),
                IsRead = false,
                RetrievedBy = null,
                UserId = user.Id,
            };

            // Send Notification
            var HubNotif = new CombinedTableDTO
            {
                Parcel = findParcel,
                Notification = notification
            };

            await _context.Database.ExecuteSqlRawAsync("UPDATE Table_Status SET is_open = 1, owner_id = NULL WHERE locker_number = " + lockerNumber);

            // Get the Connection IDs for the User
            var connectionIds = _connectionManager.GetConnections(user.Id);

            // Send the notification to each connected client of the user
            foreach (var connectionId in connectionIds)
            {
                var id = connectionId;
                await _hubContext.Clients.Client(id).SendAsync("HomeListUpdate", HubNotif);
            }


            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<IEnumerable<TableStatusAdmin>> GetLockerStatusAdmin()
    {
        var lockerStatus = await (from locker in _context.TableStatuses
                                  join user in _context.Users on locker.OwnerId equals user.Id into userGroup
                                  from owner in userGroup.DefaultIfEmpty() // Handle null owner
                                  select new TableStatusAdmin
                                  {
                                      LockerNumber = locker.LockerNumber,
                                      LockerSize = locker.LockerSize,
                                      OwnerName = owner != null ? owner.Username : "Unassigned", // Handle null
                                      IsOpen = locker.IsOpen
                                  })
                              .ToListAsync();

        return lockerStatus;
    }

    public async Task<IEnumerable<TableStatus>> GetLockerStatus(int[] ids)
    {
        var lockerStatuses = await _context.TableStatuses
            .Where(l => ids.Contains(l.LockerNumber))
            .ToListAsync();

        return lockerStatuses;
    }

    public async Task<bool> UpdateLockerStatus(int id)
    {
        try
        {
            int rowsAffected = await _context.Database.ExecuteSqlRawAsync(
                "UPDATE Table_Status SET is_open = {0} WHERE locker_number = {1}", false, id);

            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            // Optionally log the exception
            Console.WriteLine("Error updating locker status: " + ex.Message);
            return false;
        }
    }

};