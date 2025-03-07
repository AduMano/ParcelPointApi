using Microsoft.EntityFrameworkCore;
using ParcelPointApi.Data.Interface.ParcelLogs;

namespace ParcelPointApi.Data.Repositories
{
    public class CombinedTableDTO
    {
        public ParcelLog Parcel { get; set; }
        public NotificationLog Notification { get; set; }
    }

    public interface IParcelLogsRepository
    {
        Task<IEnumerable<ParcelLog>> GetParcelLogsByIdAsync(Guid id);
        Task<IEnumerable<ParcelLogSummaryDto>> GetParcelLogsSummaryAsync();
        Task<ParcelLogsCountsDto> GetParcelLogsCountsAsync();
        Task<CombinedTableDTO> CreateParcelLogsAsync(Guid id, int locker);
    }
    public class ParcelLogsRepository : IParcelLogsRepository
    {
        private readonly ParcelPointDbContext _context;

        public ParcelLogsRepository(ParcelPointDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ParcelLog>> GetParcelLogsByIdAsync(Guid id)
        {
            var logs = await _context.ParcelLogs
                .Where(p => p.UserId == id)
                .OrderBy(p => p.ArrivedAt)
                .ToListAsync();

            return logs;
        }

        public async Task<IEnumerable<ParcelLogSummaryDto>> GetParcelLogsSummaryAsync()
        {
            // Query for logs with arrived_at (i.e. "Arrived" events) where status is "Not Picked Up"
            var arrivedLogs = (from pl in _context.ParcelLogs
                join u in _context.Users on pl.UserId equals u.Id
                where pl.ArrivedAt != null && pl.Status == "Not Picked Up"
                select new ParcelLogSummaryDto
                {
                    ParcelId = (Guid)pl.ParcelId,
                    ParcelName = pl.ParcelName,
                    LockerNumber = pl.LockerNumber,
                    Status = pl.Status,
                    Action = pl.Action,
                    LogDate = pl.ArrivedAt,
                    RetrievedAt = pl.RetrievedAt,
                    LogType = "Arrived",
                    UserId = (Guid)pl.UserId,
                    UserName = u.Username
                });

            // Query for logs with retrieved_at (i.e. "Retrieved" events) where status is "Picked Up"
            var retrievedLogs = (from pl in _context.ParcelLogs
                                 join u in _context.Users on pl.UserId equals u.Id
                                 where pl.ArrivedAt != null && pl.Status == "Picked Up"
                                 select new ParcelLogSummaryDto
                                 {
                    ParcelId = (Guid)pl.ParcelId,
                    ParcelName = pl.ParcelName,
                    LockerNumber = pl.LockerNumber,
                    Status = pl.Status,
                    Action = pl.Action,
                    LogDate = pl.RetrievedAt,
                    RetrievedAt = pl.RetrievedAt,
                    LogType = "Retrieved",
                    UserId = (Guid)pl.UserId,
                    UserName = u.Username
            });

            // Combine both queries using Concat (which is equivalent to UNION ALL) and sort the combined list by log_date descending
            var combinedLogs = await arrivedLogs
                .Concat(retrievedLogs)
                .OrderByDescending(log => log.LogDate)
                .ToListAsync();

            return combinedLogs;
        }

        public async Task<ParcelLogsCountsDto> GetParcelLogsCountsAsync()
        {
            // Adjust these date offsets as needed:
            DateTime now = DateTime.Now;
            DateTime dailyCutoff = now.AddDays(-1);
            DateTime weeklyCutoff = now.AddDays(-7);
            DateTime monthlyCutoff = now.AddDays(-30);
            DateTime annualCutoff = now.AddDays(-365);

            // Daily Count (last 24 hours)
            int dailyCount = await _context.ParcelLogs
                .Where(pl => (pl.ArrivedAt >= dailyCutoff) || (pl.RetrievedAt >= dailyCutoff))
                .CountAsync();

            // Weekly Count (last 7 days)
            int weeklyCount = await _context.ParcelLogs
                .Where(pl => (pl.ArrivedAt >= weeklyCutoff) || (pl.RetrievedAt >= weeklyCutoff))
                .CountAsync();

            // Monthly Count (last 30 days)
            int monthlyCount = await _context.ParcelLogs
                .Where(pl => (pl.ArrivedAt >= monthlyCutoff) || (pl.RetrievedAt >= monthlyCutoff))
                .CountAsync();

            // Annual Count (last 365 days)
            int annualCount = await _context.ParcelLogs
                .Where(pl => (pl.ArrivedAt >= annualCutoff) || (pl.RetrievedAt >= annualCutoff))
                .CountAsync();

            // Return the result as a DTO
            return new ParcelLogsCountsDto
            {
                Daily = dailyCount,
                Weekly = weeklyCount,
                Monthly = monthlyCount,
                Annually = annualCount
            };
        }

        public async Task<CombinedTableDTO> CreateParcelLogsAsync(Guid id, int locker)
        {
            var newID = Guid.NewGuid();
            var log = new ParcelLog
            {
                Id = newID,
                ParcelId = newID,
                ParcelName = "Parcel",
                Action = null,
                ArrivedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                LockerNumber = locker + "",
                Status = "Not Picked Up",
                UserId = id,
                RetrievedAt = null,
                RetrievedBy = null
            };

            await _context.ParcelLogs.AddAsync(log);

            var notification = new NotificationLog
            {
                Id = Guid.NewGuid(),
                Title = "Delivered Parcel",
                Context = "Your parcel has been delivered and stored in Locker #" + locker,
                CreatedAt = DateTime.UtcNow,
                LockerNumber = locker,
                IsRead = false,
                RetrievedBy = null,
                UserId = id,
            };

            await _context.NotificationLogs.AddAsync(notification);

            await _context.SaveChangesAsync();

            return new CombinedTableDTO
            {
                Parcel = log,
                Notification = notification
            };
        }
    }
}
