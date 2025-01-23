using Microsoft.EntityFrameworkCore;
using ParcelPointApi.Data.Interface.Users;

namespace ParcelPointDB.Data.Repositories
{
    public interface IAuthRepository
    {
        Task<IUserDto?> LoginAdmin(string username, string password);
        Task<IUserDto?> LoginUser(string username, string password);
    }

    public class AuthRepository : IAuthRepository
    {
        private readonly ParcelPointDbContext _context;
        private readonly PasswordHelper _passwordHelper;

        public AuthRepository(ParcelPointDbContext context, PasswordHelper passwordHelper)
        {
            _context = context;
            _passwordHelper = passwordHelper;
        }

        public async Task<IUserDto?> LoginAdmin(string username, string password)
        {
            var user = await _context.Users
                .Where(u => u.Username == username && u.Role.Name == "Admin")
                .FirstOrDefaultAsync();

            if (user == null || !_passwordHelper.ValidatePassword(password, user.Password))
                return null;

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                CreatedBy = user.CreatedBy,
                CreatedAt = user.CreatedAt,
                RoleId = user.RoleId,
                RoleName = user.Role?.Name
            };
        }

        public async Task<IUserDto?> LoginUser(string username, string password)
        {
            var user = await _context.Users
                .Where(u => u.Username == username && u.Role.Name == "Users")
                .FirstOrDefaultAsync();

            if (user == null || !_passwordHelper.ValidatePassword(password, user.Password))
                return null;

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                CreatedBy = user.CreatedBy,
                CreatedAt = user.CreatedAt,
                RoleId = user.RoleId,
                RoleName = user.Role?.Name
            };
        }
    }
}