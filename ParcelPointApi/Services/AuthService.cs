using ParcelPointApi.Data.Interface.Users;
using ParcelPointDB.Data.Repositories;
using System.Runtime.CompilerServices;

namespace ParcelPointDB.Services
{
    public interface IAuthService
    {
        Task<IUserDto?> LoginAdmin(string username, string password);
        Task<IUserDto?> LoginUser(string username, string password);
        Task LogoutUser(Guid userID);
        Task<bool> VerifyEmailAsync(string email);
        Task<string> SendVerificationCodeAsync(string email);
        Task<bool> VerifyCodeAsync(string email, string code);
        Task<bool> UpdatePasswordAsync(string email, string password);  

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

        public async Task LogoutUser(Guid userId)
        {
            await _authRepository.LogoutUser(userId);
        }

        public async Task<bool> VerifyEmailAsync(string email)
        {
            return await _authRepository.VerifyEmailAsync(email);
        }

        public async Task<string> SendVerificationCodeAsync(string email)
        {
            // Returns 6 Digits Code
            var code = await _authRepository.GenerateVerificationCodeByEmail(email);

            // Send Email
            await _authRepository.SendVerificationCodeEmail(email, code);

            return code;
        }

        public async Task<bool> VerifyCodeAsync(string email, string code)
        {
            // Verify Code First
            var isVerified = await _authRepository.VerifyCodeAsync(email, code);
            return isVerified;
        }

        public async Task<bool> UpdatePasswordAsync(string email, string password)
        {
            var isChanged = await _authRepository.UpdatePasswordAsync(email, password);
            return isChanged;
        } 
    }
}