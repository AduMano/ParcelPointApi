using ParcelPointApi.Data.Interface.Users;
using ParcelPointApi.Models;
using ParcelPointDB.Data.Repositories;

namespace ParcelPointDB.Services
{
    public interface IUserService
    {
        Task<IEnumerable<IUserDto>> GetAllUsersAsync();
        Task<IUserDto?> GetUserByIdAsync(Guid id);
        Task<string> CreateUserAsync(User user);
        Task<string> UpdateUserAsync(User user);
        Task<string> DeleteUserAsync(Guid id);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
    }
}