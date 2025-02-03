using Microsoft.EntityFrameworkCore;
using ParcelPointApi.Data.Interface.UserGroup;
using ParcelPointApi.Data.Interface.Users;
using System.Numerics;

namespace ParcelPointApi.Data.Repositories
{
    public interface IUserGroupRepository
    {
        Task<IEnumerable<UserGroup>> GetAllUserGroupsAsync();
        Task<UserGroup?> GetUserGroupByIdAsync(Guid id);
        Task<UserGroup?> GetUserGroupByUserIdAsync(Guid id);
        Task<IEnumerable<MemberInfoDTO>> GetMemberListByIdAsync(Guid id);
        Task<IEnumerable<UserInformationDTO>> GetUsersListAsync(Guid loggedInUserId);
        Task<MemberInfoDTO> CreateMemberAsync(AddMemberDto addMemberRequest);
        Task<UserGroup> CreateUserGroupAsync(UserGroup user);
        Task<bool> UpdateMemberAsync(UpdateMemberDto updateRequest);
        Task<bool> DeleteMemberAsync(Guid deleteRequest);
    }

    public class UserGroupRepository : IUserGroupRepository
    {
        private readonly ParcelPointDbContext _context;

        public UserGroupRepository(ParcelPointDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserGroup>> GetAllUserGroupsAsync()
        {
            return await _context.UserGroups.ToListAsync();
        }

        public async Task<UserGroup?> GetUserGroupByIdAsync(Guid id)
        {
            return await _context.UserGroups.FindAsync(id);
        }

        public async Task<UserGroup?> GetUserGroupByUserIdAsync(Guid id)
        {
            return await _context.UserGroups.Where(group => group.OwnerId == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserInformationDTO>> GetUsersListAsync(Guid loggedInUserId)
        {
            // Get the GroupId for the currently logged-in user (excluding the current user)
            var currentUserGroupId = await _context.UserGroupMembers
                .Where(ugm => ugm.MemberId == loggedInUserId)
                .Select(ugm => ugm.GroupId)
                .FirstOrDefaultAsync();

            if (currentUserGroupId == Guid.Empty)
            {
                return Enumerable.Empty<UserInformationDTO>(); // No group found for the logged-in user
            }

            // Get users who are not part of the same group as the logged-in user
            var usersNotInGroup = await _context.UserInformations
                .Where(u => u.UserId != loggedInUserId) // Exclude the logged-in user
                .Where(u => u.User.Role.Name == "users") // Ensure the user has the "users" role
                .Where(u => !_context.UserGroupMembers
                    .Where(ugm => ugm.GroupId == currentUserGroupId)
                    .Select(ugm => ugm.MemberId)
                    .Contains(u.UserId)) // Exclude users who are already in the group
                .Select(u => new UserInformationDTO
                {
                    Id = (Guid)u.UserId,
                    FirstName = u.FirstName,
                    MiddleName = u.MiddleName,
                    LastName = u.LastName,
                    Suffix = u.Suffix,
                    BirthDate = u.Birthdate,
                    Address = u.Address,
                    ContactNumber = u.ContactNumber,
                    PhotoUrl = "Test",
                    Email = u.Email,
                    Username = u.User.Username
                })
                .ToListAsync();


            return usersNotInGroup;
        }

        public async Task<IEnumerable<MemberInfoDTO>> GetMemberListByIdAsync(Guid id)
        {
            var users = await _context.UserGroupMembers
                .Where(ugm => ugm.GroupId == id) // Exclude members that belong to the given GroupId
                .Include(ugm => ugm.Member) // Include the Member (User) related to this group
                .ThenInclude(user => user.UserInformations) // Include UserInformation for each User
                .Select(ugm => new MemberInfoDTO
                {
                    Id = (Guid)ugm.Member.UserInformations.FirstOrDefault().UserId,
                    FirstName = ugm.Member.UserInformations.FirstOrDefault().FirstName, // Map FirstName
                    MiddleName = ugm.Member.UserInformations.FirstOrDefault().MiddleName, // Map MiddleName
                    LastName = ugm.Member.UserInformations.FirstOrDefault().LastName, // Map LastName
                    Suffix = ugm.Member.UserInformations.FirstOrDefault().Suffix, // Map Suffix
                    BirthDate = ugm.Member.UserInformations.FirstOrDefault().Birthdate, // Map BirthDate
                    Address = ugm.Member.UserInformations.FirstOrDefault().Address, // Map Address
                    ContactNumber = ugm.Member.UserInformations.FirstOrDefault().ContactNumber, // Map ContactNumber
                    PhotoUrl = "Test", // Map PhotoUrl (you can modify this according to your actual logic)
                    Email = ugm.Member.UserInformations.FirstOrDefault().Email, // Map Email
                    Username = ugm.Member.Username, // Map Username
                    Relationship = ugm.Relationship, // Assuming you have the relationship object available
                    IsAuthorized = ugm.IsAuthorized, // Map IsAuthorized
                    GroupMemberId = ugm.MemberId
                })
                .ToListAsync();

            return users;
        }

        public async Task<UserGroup> CreateUserGroupAsync(UserGroup addRequest)
        {
            _context.UserGroups.Add(addRequest);
            await _context.SaveChangesAsync();
            return addRequest;
        }

        public async Task<MemberInfoDTO> CreateMemberAsync(AddMemberDto addMemberRequest)
        {
            var currentUserGroupId = await _context.UserGroups
                .Where(ugm => ugm.Owner.Id == addMemberRequest.CreatedBy)
                .Select(ugm => ugm.Id)
                .FirstOrDefaultAsync();

            if (currentUserGroupId == Guid.Empty)
            {
                throw new InvalidOperationException("No group found for the given user.");
            }

            var newMember = new UserGroupMember
            {
                Id = Guid.NewGuid(),
                MemberId = addMemberRequest.MemberId,
                GroupId = currentUserGroupId,
                RelationshipId = addMemberRequest.RelationshipId,
                IsAuthorized = addMemberRequest.IsAuthorized,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = addMemberRequest.CreatedBy
            };

            Console.WriteLine(newMember);

            await _context.UserGroupMembers.AddAsync(newMember);
            await _context.SaveChangesAsync();

            var memberInfo = await _context.UserGroupMembers
                .Where(ugm => ugm.Id == newMember.Id)
                .Include(ugm => ugm.Member) // Include the User details for the member
                .ThenInclude(user => user.UserInformations) // Include UserInformation for each User
                .Select(ugm => new MemberInfoDTO
                {
                    Id = (Guid)ugm.Member.UserInformations.FirstOrDefault().UserId,
                    FirstName = ugm.Member.UserInformations.FirstOrDefault().FirstName,
                    MiddleName = ugm.Member.UserInformations.FirstOrDefault().MiddleName,
                    LastName = ugm.Member.UserInformations.FirstOrDefault().LastName,
                    Suffix = ugm.Member.UserInformations.FirstOrDefault().Suffix,
                    BirthDate = ugm.Member.UserInformations.FirstOrDefault().Birthdate,
                    Address = ugm.Member.UserInformations.FirstOrDefault().Address,
                    ContactNumber = ugm.Member.UserInformations.FirstOrDefault().ContactNumber,
                    PhotoUrl = "Test",
                    Email = ugm.Member.UserInformations.FirstOrDefault().Email,
                    Username = ugm.Member.Username,
                    Relationship = ugm.Relationship, // Assuming you have the relationship object available
                    IsAuthorized = ugm.IsAuthorized, // Map IsAuthorized
                    GroupMemberId = ugm.MemberId
                })
                .FirstOrDefaultAsync();

            return memberInfo;
        }

        public async Task<bool> UpdateMemberAsync(UpdateMemberDto updateRequest)
        {
            var test = updateRequest;
            var member = await _context.UserGroupMembers
                .Where(x => x.MemberId == updateRequest.GroupMemberId)
                .SingleOrDefaultAsync();


            if (member == null) return false;

            member.RelationshipId = updateRequest.RelationshipId;
            member.IsAuthorized = updateRequest.IsAuthorized;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return false;
            }
        }

        public async Task<bool> DeleteMemberAsync(Guid deleteRequest)
        {
            var member = await _context.UserGroupMembers
                .Where(member => member.MemberId == deleteRequest)
                .SingleOrDefaultAsync();

            Console.WriteLine(member);

            if (member == null) return false;

            try
            {
                _context.UserGroupMembers.Remove(member);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
