using ParcelPointApi.Data.Interface.Users;
using ParcelPointApi.Models;
using ParcelPointDB.Data.Repositories;
using System.Threading.Tasks;

namespace ParcelPointDB.Services
{
    public interface IUserService
    {
        Task<IEnumerable<IUserDto>> GetAllUsersAsync();
        Task<IUserDto?> GetUserByIdAsync(Guid id);
        Task<IEnumerable<UserDetailsDto>> GetAllUsersWithDetailsAsync(String type);
        Task<Guid> AddNewUserAsync(RegisterUserDto request);
        Task<bool> UsernameCheckAsync(String username, String except);
        Task<string> CreateUserAsync(User user);
        Task UpdateUserAsync(RegisterUserDto request);
        Task<string> DeleteUserAsync(Guid id);

        // Get User Info
        Task<UserInformationDTO> GetUserInfoByIdAsync(Guid id);

        // Read Notification
        Task<bool> ReadNotificationByIdAsync(Guid[] id);

        // Update User Info
        Task<string> UpdateUserInfoAsync(UserUpdateInformationDTO info);

        // Get User Notifications
        Task<IEnumerable<NotificationLog>> GetUserNotificationsByIdAsync(Guid id);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserInfoRepository _userInfoRepository;

        public UserService(IUserRepository userRepository, IUserInfoRepository userInfoRepository)
        {
            _userRepository = userRepository;
            _userInfoRepository = userInfoRepository;
        }

        public async Task<IEnumerable<IUserDto>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<IUserDto?> GetUserByIdAsync(Guid id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<bool> UsernameCheckAsync(String username, String except)
        {
            return await _userRepository.UsernameCheckAsync(username, except);
        }

        public async Task<IEnumerable<UserDetailsDto>> GetAllUsersWithDetailsAsync(String type)
        {
            return await _userInfoRepository.GetAllUsersWithDetailsAsync(type);
        }

        public async Task<Guid> AddNewUserAsync(RegisterUserDto request)
        {
            // Potentially do validations, password hashing, etc.
            // Then delegate to repository:
            return await _userRepository.AddNewUserAsync(request);
        }

        public async Task<string> CreateUserAsync(User user)
        {
            try
            {
                await _userRepository.CreateUserAsync(user);
                return "User created successfully.";
            }
            catch (Exception ex)
            {
                return $"Error creating user: {ex.Message}";
            }
        }

        public async Task UpdateUserAsync(RegisterUserDto request)
        {

            Console.WriteLine("Went to Service");
            await _userRepository.UpdateUserAsync(request);
        }

        public async Task<string> DeleteUserAsync(Guid id)
        {
            try
            {
                var isDeleted = await _userRepository.DeleteUserAsync(id);
                if (isDeleted)
                {
                    return "User deleted successfully.";
                }
                else
                {
                    return "Failed to delete user.";
                }
            }
            catch (Exception ex)
            {
                return $"Error deleting user: {ex.Message}";
            }
        }

        public async Task<UserInformationDTO> GetUserInfoByIdAsync(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            var information = await _userInfoRepository.GetUserInformationByIdAsync(id);

            if (information == null)
            {
                return information;
            }

            information.Username = user.Username;

            return information;
        }

        public async Task<bool> ReadNotificationByIdAsync(Guid[] id)
        {
            for (var i = 0; i < id.Length; i++)
            {
                await _userRepository.ReadNotificationByIdAsync(id[i]);
            }

            return true;
        }

        public async Task<string> UpdateUserInfoAsync(UserUpdateInformationDTO info)
        {
            var checkUsername = await _userRepository.CheckUsernameAsync(info, "inclusive");
            if (!checkUsername) return "existing username";

            var updateUsername = await _userRepository.UpdateUsernameAsync(info.Id, info.Username);
            if (!updateUsername) return "update username failed";

            var updateInfo = await _userInfoRepository.UpdateUserInfoAsync(info.Id, info);
            if (!updateInfo) return "update info failed";

            return "success";
        }

        public async Task<IEnumerable<NotificationLog>> GetUserNotificationsByIdAsync(Guid id)
        {
            return await _userRepository.GetUserNotificationsByIdAsync(id);
        }
    }
}