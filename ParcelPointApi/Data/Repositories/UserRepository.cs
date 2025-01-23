using Microsoft.EntityFrameworkCore;
using ParcelPointApi.Data.Interface.Users;

namespace ParcelPointDB.Data.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IUserDto>> GetAllUsersAsync();
        Task<IUserDto?> GetUserByIdAsync(Guid id);
        Task<User> CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(Guid id);
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

        public async Task<User> CreateUserAsync(User user)
        {
            // Encrypt the password before saving
            user.Password = _passwordHelper.HashPassword(user.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }


        public async Task<bool> UpdateUserAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
            {
                return false;
            }

            // Update only the properties you want to modify
            existingUser.Username = user.Username;

            // Add other fields here as needed, excluding Password
            //existingUser.ModifiedBy = "1234-1231-2412-2344";
            existingUser.ModifiedAt = new DateTime();

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
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
    }
}