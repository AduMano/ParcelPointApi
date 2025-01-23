using ParcelPointDB.Data.Repositories;
using ParcelPointApi.Models;

namespace ParcelPointDB.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role?> GetRoleByIdAsync(Guid id);
        Task<string> CreateRoleAsync(Role role);
        Task<string> UpdateRoleAsync(Role role);
        Task<string> DeleteRoleAsync(Guid id);
    }

    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _roleRepository.GetAllRolesAsync();
        }

        public async Task<Role?> GetRoleByIdAsync(Guid id)
        {
            return await _roleRepository.GetRoleByIdAsync(id);
        }

        public async Task<string> CreateRoleAsync(Role role)
        {
            try
            {
                await _roleRepository.CreateRoleAsync(role);
                return "Role created successfully.";
            }
            catch (Exception ex)
            {
                return $"Error creating role: {ex.Message}";
            }
        }

        public async Task<string> UpdateRoleAsync(Role role)
        {
            try
            {
                var isUpdated = await _roleRepository.UpdateRoleAsync(role);
                if (isUpdated)
                {
                    return "Role updated successfully.";
                }
                else
                {
                    return "Failed to update role.";
                }
            }
            catch (Exception ex)
            {
                return $"Error updating role: {ex.Message}";
            }
        }

        public async Task<string> DeleteRoleAsync(Guid id)
        {
            try
            {
                var isDeleted = await _roleRepository.DeleteRoleAsync(id);
                if (isDeleted)
                {
                    return "Role deleted successfully.";
                }
                else
                {
                    return "Failed to delete role.";
                }
            }
            catch (Exception ex)
            {
                return $"Error deleting role: {ex.Message}";
            }
        }
    }
}