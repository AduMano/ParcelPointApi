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
        Task<IEnumerable<UserInformationDTO>> GetUsersListAsync(Guid loggedInUserId);
        Task<MemberInfoDTO> CreateMemberAsync(AddMemberDto addMemberRequest);
        Task<string> CreateUserGroupAsync(UserGroup user);
        Task<MemberResult> UpdateMemberAsync(UpdateMemberDto updateRequest);
        Task<MemberResult> DeleteMemberAsync(Guid deleteRequest, Guid groupOwner);
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

        public async Task<IEnumerable<UserInformationDTO>> GetUsersListAsync(Guid loggedInUserId)
        {
            var users = await _userGroupRepository.GetUsersListAsync(loggedInUserId);
            if (users == null) return Enumerable.Empty<UserInformationDTO>();

            return users;
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

        public async Task<MemberInfoDTO> CreateMemberAsync(AddMemberDto addMemberRequest)
        {
            return await _userGroupRepository.CreateMemberAsync(addMemberRequest);
        }

        public async Task<MemberResult> UpdateMemberAsync(UpdateMemberDto updateRequest)
        {
            try
            {
                var result = await _userGroupRepository.UpdateMemberAsync(updateRequest);
                return new MemberResult { Success = result, ErrorMessage = result ? null : "Failed to update member." };
            }
            catch (Exception ex)
            {
                return new MemberResult { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<MemberResult> DeleteMemberAsync(Guid deleteRequest, Guid groupOwner)
        {
            try
            {
                var result = await _userGroupRepository.DeleteMemberAsync(deleteRequest, groupOwner);
                return new MemberResult { Success = result, ErrorMessage = result ? null : "Failed to delete member." };
            }
            catch (Exception ex)
            {
                return new MemberResult { Success = false, ErrorMessage = ex.Message };
            }
        }
    }
}