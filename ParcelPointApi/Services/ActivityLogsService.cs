using ParcelPointApi.Data.Interface.Users;
using ParcelPointDB.Data.Repositories;
using System.Runtime.CompilerServices;

namespace ParcelPointDB.Services
{
    public interface IActivityLogsService
    {
        Task<IEnumerable<ActivityLog>> GetActivityLogsAsync();
    }

    public class ActivityLogsService : IActivityLogsService
    {
        private readonly IActivityLogsRepository _activityLogsRepository;

        public ActivityLogsService(IActivityLogsRepository activityLogsRepository)
        {
            _activityLogsRepository = activityLogsRepository;
        }

        public async Task<IEnumerable<ActivityLog>> GetActivityLogsAsync()
        {
            return await _activityLogsRepository.GetActivityLogsAsync();
        }
    }
}