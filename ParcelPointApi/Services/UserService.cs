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
        Task<string> CreateUserAsync(User user);
        Task<string> UpdateUserAsync(User user);
        Task<string> DeleteUserAsync(Guid id);

        // Get User Info
        Task<UserInformationDTO> GetUserInfoByIdAsync(Guid id);

        // Update User Info
        Task<string> UpdateUserInfoAsync(UserUpdateInformationDTO info);
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

        public async Task<string> UpdateUserAsync(User user)
        {
            try
            {
                var isUpdated = await _userRepository.UpdateUserAsync(user);
                if (isUpdated)
                {
                    return "User updated successfully.";
                }
                else
                {
                    return "Failed to update user.";
                }
            }
            catch (Exception ex)
            {
                return $"Error updating user: {ex.Message}";
            }
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
    }
}