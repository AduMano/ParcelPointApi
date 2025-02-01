using Microsoft.EntityFrameworkCore;
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
        Task<UserGroup> CreateUserGroupAsync(UserGroup user);
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

        public async Task<IEnumerable<MemberInfoDTO>> GetMemberListByIdAsync(Guid id)
        {
            var users = await _context.UserGroupMembers
                .Where(ugm => ugm.GroupId == id) // Exclude members that belong to the given GroupId
                .Include(ugm => ugm.Member) // Include the Member (User) related to this group
                .ThenInclude(user => user.UserInformations) // Include UserInformation for each User
                .Select(ugm => new MemberInfoDTO
                {
                    Id = ugm.Member.Id, // Map Id from the User entity
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
    }
}
