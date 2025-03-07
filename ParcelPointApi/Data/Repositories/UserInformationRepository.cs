using Microsoft.EntityFrameworkCore;
using ParcelPointApi.Data.Interface.Users;

namespace ParcelPointDB.Data.Repositories
{
    public interface IUserInfoRepository
    {
        // Get UserInformation
        Task<UserInformationDTO?> GetUserInformationByIdAsync(Guid id);
        Task<IEnumerable<UserDetailsDto>> GetAllUsersWithDetailsAsync(String type);

        // Update UserInformation
        Task<bool> UpdateUserInfoAsync(Guid id, UserUpdateInformationDTO userInformation);
    }

    public class UserInformationRepository : IUserInfoRepository
    {
        private readonly ParcelPointDbContext _context;

        public UserInformationRepository(ParcelPointDbContext context)
        {
            _context = context;
        }

        public async Task<UserInformationDTO?> GetUserInformationByIdAsync(Guid id)
        {
            try
            {
                return await _context.UserInformations
                .Where(user => user.UserId == id)
                .Select(user => new UserInformationDTO
                {
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    Suffix = user.Suffix,
                    BirthDate = user.Birthdate,
                    Address = user.Address,
                    ContactNumber = user.ContactNumber,
                    Email = user.Email,
                    PhotoUrl = "Test"
                })
                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<UserDetailsDto>> GetAllUsersWithDetailsAsync(String type)
        {
            // 
            // We'll:
            // 1) Include Role
            // 2) Include UserInformations -> ThenInclude Gender
            // 3) Flatten the data into UserDetailsDto
            //    (If a user can have multiple UserInformations, we'll pick the first. 
            //     Adjust as needed if you want all of them.)
            //
            var users = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.UserInformations)
                    .ThenInclude(ui => ui.Gender)
                .Where(u => u.Role.Name == type)
                .Select(u => new UserDetailsDto
                {
                    UserId = u.Id,
                    Username = u.Username,
                    IsActive = u.IsActive,
                    RoleName = u.Role.Name,
                    RoleID = u.Role.Id,

                    // For user info, assume there's either exactly one or we take the first
                    FirstName = u.UserInformations.Select(ui => ui.FirstName).FirstOrDefault(),
                    MiddleName = u.UserInformations.Select(ui => ui.MiddleName).FirstOrDefault(),
                    LastName = u.UserInformations.Select(ui => ui.LastName).FirstOrDefault(),
                    Suffix = u.UserInformations.Select(ui => ui.Suffix).FirstOrDefault(),
                    Birthdate = u.UserInformations.Select(ui => ui.Birthdate.HasValue
                        ? ui.Birthdate.Value.ToDateTime(new System.TimeOnly(0, 0))
                        : (System.DateTime?)null).FirstOrDefault(),

                    // Gender name from nested Gender
                    Gender = u.UserInformations
                        .Select(ui => ui.Gender != null ? ui.Gender.Name : null)
                        .FirstOrDefault(),

                    GenderID = u.UserInformations.Select(ui => ui.Gender.Id).FirstOrDefault(),

                    Address = u.UserInformations.Select(ui => ui.Address).FirstOrDefault(),
                    ContactNumber = u.UserInformations.Select(ui => ui.ContactNumber).FirstOrDefault(),
                    Email = u.UserInformations.Select(ui => ui.Email).FirstOrDefault(),
                    PhotoUrl = "" // or read from userInformation if you store photo there
                })
                .ToListAsync();

            return users;
        }

        public async Task<bool> UpdateUserInfoAsync(Guid id, UserUpdateInformationDTO userInformation)
        {
            try
            {
                var user = await _context.UserInformations
                    .Where(user => user.UserId == id)
                    .FirstAsync();

                if (user == null) { return false; }

                user.FirstName = userInformation.FirstName;
                user.LastName = userInformation.LastName;
                user.Birthdate = userInformation.BirthDate;
                user.Address = userInformation.Address;

                // Insert Logs
                var luser = await _context.Users.Where(u => u.Id == userInformation.Id).Select(u => new User { Username = u.Username, Id = u.Id }).FirstOrDefaultAsync();
                var log = new ActivityLog
                {
                    ActionTitle = "Update Information",
                    ActionContext = $"User {luser.Username} Updated their own information",
                    CreatedBy = luser.Id,
                    Module = "Utilities",
                    SubModule = "User Logs",
                };


                await _context.ActivityLogs.AddAsync(log);

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