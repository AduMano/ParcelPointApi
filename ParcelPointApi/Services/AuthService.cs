using ParcelPointApi.Data.Interface.Users;
using ParcelPointDB.Data.Repositories;

namespace ParcelPointDB.Services
{
    public interface IAuthService
    {
        Task<IUserDto?> LoginAdmin(string username, string password);
        Task<IUserDto?> LoginUser(string username, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        public async Task<IUserDto?> LoginAdmin(string username, string password)
        {
            return await _authRepository.LoginAdmin(username, password);
        }
        public async Task<IUserDto?> LoginUser(string username, string password)
        {
            return await _authRepository.LoginUser(username, password);
        }
    }
}