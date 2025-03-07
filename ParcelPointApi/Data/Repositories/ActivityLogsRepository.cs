using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParcelPointApi.Data.Interface;
using ParcelPointApi.Data.Interface.Users;
using ParcelPointApi.Helpers;
using ParcelPointApi.Models;
using System.Reflection;

namespace ParcelPointDB.Data.Repositories
{
    public interface IActivityLogsRepository
    {
        Task<IEnumerable<ActivityLog>> GetActivityLogsAsync();
    }

    public class ActivityLogsRepository : IActivityLogsRepository
    {
        private readonly ParcelPointDbContext _context;

        public ActivityLogsRepository(ParcelPointDbContext context)
        {
            _context = context;
        }

        public async Task <IEnumerable<ActivityLog>> GetActivityLogsAsync()
        {
            var logs = await _context.ActivityLogs
                .OrderByDescending(al => al.CreatedAt)
                .ToListAsync();

            return logs;
        }
    }
}