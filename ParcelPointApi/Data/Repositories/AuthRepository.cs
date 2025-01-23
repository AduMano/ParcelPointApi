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

        public AuthRepository(ParcelPointDbContext context)
        {
            _context = context;
        }

        public async Task<IUserDto?> LoginAdmin(string username, string password)
        {
            return await _context.Users
                .Where(user =>
                    user.Username == username &&
                    user.Password == password &&
                    user.Role.Name == "Admin"
                )
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

        public async Task<IUserDto?> LoginUser(string username, string password)
        {
            return await _context.Users
                .Where(user =>
                    user.Username == username &&
                    user.Password == password &&
                    user.Role.Name == "Users"
                )
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
    }
}