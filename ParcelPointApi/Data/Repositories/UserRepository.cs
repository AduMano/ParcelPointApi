using Microsoft.EntityFrameworkCore;
using ParcelPointApi.Data.Interface.Users;
using Sprache;

namespace ParcelPointDB.Data.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IUserDto>> GetAllUsersAsync();
        Task<IUserDto?> GetUserByIdAsync(Guid id);
        Task<Guid> AddNewUserAsync(RegisterUserDto request);
        Task<bool> UsernameCheckAsync(String username, String except);
        Task<User> CreateUserAsync(User user);
        Task UpdateUserAsync(RegisterUserDto request);
        Task<bool> DeleteUserAsync(Guid id);

        // Check Existing Username 
        Task<bool> CheckUsernameAsync(UserUpdateInformationDTO info, string type);

        // Read Notification
        Task<bool> ReadNotificationByIdAsync(Guid id);

        // Get User By Contact Number
        Task<User> GetUserByNumberAsync(string number);

        // Update Username
        Task<bool> UpdateUsernameAsync(Guid id, string username);

        // Get User Notifications
        Task<IEnumerable<NotificationLog>> GetUserNotificationsByIdAsync(Guid id);
    }

    public class UserRepository : IUserRepository
    {
        private readonly ParcelPointDbContext _context;
        private readonly PasswordHelper _passwordHelper;

        public UserRepository(ParcelPointDbContext context, PasswordHelper passwordHelper)
        {
            _context = context;
            _passwordHelper = passwordHelper;
        }

        public async Task<IEnumerable<IUserDto>> GetAllUsersAsync()
        {
            return await _context.Users
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    CreatedBy = user.CreatedBy,
                    CreatedAt = user.CreatedAt,
                    RoleId = user.RoleId,
                    RoleName = (user.Role.Name != null) ? user.Role.Name : null
                })
                .ToListAsync();
        }

        public async Task<bool> UsernameCheckAsync(String username, String except)
        {
            var IsExisting = await _context.Users
                .Where(u => u.Username == username && u.Username != except)
                .FirstOrDefaultAsync();

            if (IsExisting != null) return true;
            return false;
        }

        public async Task<IUserDto?> GetUserByIdAsync(Guid id)
        {
            return await _context.Users
                .Where(user => user.Id == id)
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    CreatedBy = user.CreatedBy,
                    CreatedAt = user.CreatedAt,
                    RoleId = user.RoleId,
                    RoleName = (user.Role.Name != null) ? user.Role.Name : null
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Guid> AddNewUserAsync(RegisterUserDto request)
        {
            // 1) Create the User entity
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Password = _passwordHelper.HashPassword(request.Password),
                RoleId = request.Role,        // from your RegisterUserDto
                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.OperatorID,
                IsActive = true  // or false if you want to require activation
                                 // You can set CreatedBy, etc. if needed
            };

            // 2) Add to the DB context
            _context.Users.Add(newUser);

            // 3) Create the UserInformation entity
            var userInfo = new UserInformation
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                Suffix = request.Suffix,
                Address = request.Address,
                ContactNumber = request.ContactNumber,
                Email = request.Email,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.OperatorID,
                // Convert your BirthDate (DateTime) to DateOnly if the field is DateOnly
                Birthdate = DateOnly.FromDateTime(request.BirthDate),
                GenderId = request.Gender,
                UserId = newUser.Id,
                PhotoUrl = request.PhotoUrl
            };

            // 4) Add the UserInformation to the context
            _context.UserInformations.Add(userInfo);

            var userOperator = await _context.Users
                .Where(u => u.Id == request.OperatorID)
                .FirstOrDefaultAsync();

            // Insert User Logs
            var log = new ActivityLog
            {
                ActionTitle = "Admin added a user",
                ActionContext = $"Admin {userOperator.Username} Just Added {request.Username} as {request.UserType}",
                CreatedAt = DateTime.Now,
                CreatedBy = request.OperatorID,
                Module = "Utilities",
                SubModule = "User Logs"
            };

            await _context.ActivityLogs.AddAsync(log);

            // 5) Save changes once after both user & userInfo are added
            await _context.SaveChangesAsync();

            return newUser.Id;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            // Encrypt the password before saving
            user.Password = _passwordHelper.HashPassword(user.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }


        public async Task UpdateUserAsync(RegisterUserDto request)
        {

            Console.WriteLine("Went to Repository");
            // Find the existing user
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);
            if (user == null)
            {
                Console.WriteLine("Not Found");
                throw new Exception("User not found.");
            }

            // Update the User entity
            user.Username = request.Username;
            Console.WriteLine("Got the username");

            user.RoleId = request.Role; // Assuming Role is provided as a string representation of a Guid.
            user.ModifiedAt = DateTime.UtcNow;
            user.ModifiedBy = request.OperatorID;
            // Optionally update CreatedBy/ModifiedBy if needed.

            Console.WriteLine("Updating User");

            // Find the associated UserInformation (assuming one-to-one or using the first record)
            var userInfo = await _context.UserInformations.FirstOrDefaultAsync(ui => ui.UserId == request.UserId);
            if (userInfo == null)
            {

                Console.WriteLine("No User Information, Creating one.");
                // If no UserInformation exists, create a new record.
                userInfo = new UserInformation
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    CreatedAt = DateTime.UtcNow
                };
                _context.UserInformations.Add(userInfo);
            }

            // Update the UserInformation fields

            Console.WriteLine("Updating Information...");
            userInfo.FirstName = request.FirstName;
            userInfo.MiddleName = request.MiddleName;
            userInfo.LastName = request.LastName;
            userInfo.Suffix = request.Suffix;
            // Convert the DateTime to DateOnly if your model uses DateOnly (requires .NET 6+)
            userInfo.Birthdate = DateOnly.FromDateTime(request.BirthDate);
            userInfo.GenderId = request.Gender;  // Assuming Gender is of type Guid in your model.
            userInfo.Address = request.Address;
            userInfo.Email = request.Email;
            userInfo.ContactNumber = request.ContactNumber;
            userInfo.ModifiedAt = DateTime.UtcNow;
            userInfo.ModifiedBy = request.OperatorID;

            var userOperator = await _context.Users
                .Where(u => u.Id == request.OperatorID)
                .FirstOrDefaultAsync();


            Console.WriteLine("Adding Logs");
            // Insert User Logs
            var log = new ActivityLog
            {
                ActionTitle = "Admin updated a user",
                ActionContext = $"Admin {userOperator.Username} Just Updated {request.Username} Information",
                CreatedAt = DateTime.Now,
                CreatedBy = request.OperatorID,
                Module = "Utilities",
                SubModule = "User Logs"
            };

            await _context.ActivityLogs.AddAsync(log);

            // Save all changes

            Console.WriteLine("Saving...");
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckUsernameAsync(UserUpdateInformationDTO info, string type)
        {
            try
            {
                var user = await _context.Users
                .Where(u => u.Username == info.Username)
                .Select(u => new UserInformationDTO { Username = u.Username, Id = u.Id })
                .FirstOrDefaultAsync();

                if (user == null || (type == "inclusive" && user.Id == info.Id))
                    return true; // Username is available or inclusive check passes (same user)

                return false; // Username is taken (exclusive check fails)
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> ReadNotificationByIdAsync(Guid id)
        {
            var notification = await _context.NotificationLogs.FindAsync(id);

            if (notification == null) return false;

            notification.IsRead = true;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> GetUserByNumberAsync(string number)
        {
            var user = await _context.Users
                .Where(u => u.UserInformations.FirstOrDefault().ContactNumber == number)
                .FirstOrDefaultAsync();

            if (user == null) return null;

            return user;
        }

        public async Task<bool> UpdateUsernameAsync(Guid id, string username)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null) return false;

                user.Username = username;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<NotificationLog>> GetUserNotificationsByIdAsync(Guid id)
        {
            var notifications = await _context.NotificationLogs
                .Where(n => n.UserId == id)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return notifications;
        }
    }
}