using Microsoft.Identity.Client;
using ParcelPointApi.Data.Interface.UserGroup;
using ParcelPointApi.Data.Interface.Users;
using ParcelPointApi.Data.Repositories;
using ParcelPointDB.Data.Repositories;
using System.Threading.Tasks.Dataflow;

namespace ParcelPointDB.Services
{
    public interface IUserGroupService
    {
        Task<IEnumerable<UserGroup>> GetAllUserGroupsAsync();
        Task<UserGroup?> GetUserGroupByIdAsync(Guid id);
        Task<IEnumerable<MemberInfoDTO>> GetMemberListByIdAsync(Guid id);
        Task<string> CreateUserGroupAsync(UserGroup user);
        Task<bool> UpdateMemberAsync(UpdateMemberDto updateRequest);
    }

    public class UserGroupService : IUserGroupService
    {
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IUserRepository _userRepository;

        public UserGroupService(IUserGroupRepository userGroupRepository, IUserRepository userRepository)
        {
            _userGroupRepository = userGroupRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserGroup>> GetAllUserGroupsAsync()
        {
            return await _userGroupRepository.GetAllUserGroupsAsync();
        }

        public async Task<UserGroup?> GetUserGroupByIdAsync(Guid id)
        {
            return await _userGroupRepository.GetUserGroupByIdAsync(id);
        }

        public async Task<IEnumerable<MemberInfoDTO>> GetMemberListByIdAsync(Guid id)
        {
            var group = await _userGroupRepository.GetUserGroupByUserIdAsync(id);
            if (group == null) return Enumerable.Empty<MemberInfoDTO>();

            return await _userGroupRepository.GetMemberListByIdAsync(group.Id);
        }

        public async Task<string> CreateUserGroupAsync(UserGroup user)
        {
            try
            {
                await _userGroupRepository.CreateUserGroupAsync(user);
                return "Group created successfully.";
            }
            catch (Exception ex)
            {
                return $"Error creating group for user: {ex.Message}";
            }
        }

        public async Task<bool> UpdateMemberAsync(UpdateMemberDto updateRequest)
        {
            try
            {
                await _userGroupRepository.UpdateMemberAsync(updateRequest);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}