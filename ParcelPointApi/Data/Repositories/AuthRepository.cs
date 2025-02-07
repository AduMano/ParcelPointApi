using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParcelPointApi.Data.Interface;
using ParcelPointApi.Data.Interface.Users;
using ParcelPointApi.Helpers;
using ParcelPointApi.Models;
using System.Reflection;

namespace ParcelPointDB.Data.Repositories
{
    public interface IAuthRepository
    {
        Task<IUserDto?> LoginAdmin(string username, string password);
        Task<IUserDto?> LoginUser(string username, string password);
        Task LogoutUser(Guid userID);
        Task<bool> VerifyEmailAsync(string email);
        Task<string> GenerateVerificationCodeByEmail(string email);
        Task SendVerificationCodeEmail(string email, string code);
        Task<bool> VerifyCodeAsync(string email, string code);
        Task<bool> UpdatePasswordAsync(string email, string password);
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

            // Insert User Logs
            var log = new ActivityLog
            {
                ActionTitle = "User Logged In",
                ActionContext = $"Admin {user.Username} Just Logged In",
                CreatedAt = DateTime.Now,
                CreatedBy = user.Id,
                Module = "",
                SubModule = ""
            };

            await _context.ActivityLogs.AddAsync(log);

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                CreatedBy = user.CreatedBy,
                CreatedAt = user.CreatedAt,
                RoleId = user.RoleId,
                RoleName = user.Role?.Name,
                isActive = user.IsActive
            };
        }

        public async Task<IUserDto?> LoginUser(string username, string password)
        {
            var user = await _context.Users
                .Where(u => u.Username == username && u.Role.Name == "Users")
                .FirstOrDefaultAsync();

            if (user == null || !_passwordHelper.ValidatePassword(password, user.Password))
                return null;

            // Insert User Logs
            var log = new ActivityLog
            {
                ActionTitle = "User Logged In",
                ActionContext = $"User {user.Username} Just Logged In",
                CreatedAt = DateTime.Now,
                CreatedBy = user.Id,
                Module = "Utilities",
                SubModule = "User Logs"
            };

            await _context.ActivityLogs.AddAsync(log);
            await _context.SaveChangesAsync();

            // Return Data
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                CreatedBy = user.CreatedBy,
                CreatedAt = user.CreatedAt,
                RoleId = user.RoleId,
                RoleName = user.Role?.Name,
                isActive = user.IsActive
            };
        }

        public async Task LogoutUser(Guid userID)
        {
            // Insert User Logs
            var user = await _context.Users.Where(u => u.Id == userID).Select(u => new User { Username = u.Username }).FirstOrDefaultAsync();
            var log = new ActivityLog
            {
                ActionTitle = "User Logged Out",
                ActionContext = $"User {user.Username} Just Logged Out",
                CreatedAt = DateTime.Now,
                CreatedBy = userID,
                Module = "Utilities",
                SubModule = "User Logs"
            };

            await _context.ActivityLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> VerifyEmailAsync(string email)
        {
            var user = await _context.UserInformations.Where(u => u.Email == email).FirstOrDefaultAsync();

            return (user != null) ? true : false;
        }

        public async Task<string> GenerateVerificationCodeByEmail(string email)
        {
            // Loop generate until number that is not yet used is available
            var code = "";
            while (true)
            {
                code = Generate6Digits.GenerateVerificationCode();
                var locate = await _context.EmailVerifications
                    .Where(e => e.VerificationCode == code && e.IsUsed == false)
                    .FirstOrDefaultAsync(); 
                
                if (locate != null)
                {
                    locate.Email = email;
                    locate.CreatedAt = DateTime.Now;
                    locate.ExpiresAt = DateTime.Now.AddMinutes(10);

                    await _context.SaveChangesAsync();

                    break;
                }
                else
                {
                    var newCode = new EmailVerification
                    {
                        Email = email,
                        CreatedAt = DateTime.Now,
                        ExpiresAt = DateTime.Now.AddMinutes(10),
                        IsUsed = false,
                        VerificationCode = code
                    };

                    await _context.EmailVerifications.AddAsync(newCode);
                    await _context.SaveChangesAsync();
                    break;
                }
            }

            return code;
        }

        public async Task SendVerificationCodeEmail(string email, string code)
        {
            var user = await _context.UserInformations
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();

            var name = $"{user.FirstName} {user.LastName}";
            var emailSender = new EmailSenderDto();
            emailSender.EmailSender(email, "VERIFICATION CODE | ParcelPoint", code, name);
        }

        public async Task<bool> VerifyCodeAsync(string email, string code)
        {
            // Select the verification code and update to used
            var verificationCode = await _context.EmailVerifications
                .Where(e => e.Email == email && e.VerificationCode == code)
                .FirstOrDefaultAsync();

            if (verificationCode == null) return false;

            verificationCode.IsUsed = true;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdatePasswordAsync(string email, string password)
        {
            var userID = await _context.UserInformations
                .Where(u => u.Email == email)
                .Select(u => u.UserId)
                .FirstOrDefaultAsync();

            var userAccount = await _context.Users
                .Where(u => u.Id == userID)
                .FirstOrDefaultAsync();

            // If user not found
            if (userAccount == null) return false;

            // Update Password
            userAccount.Password = _passwordHelper.HashPassword(password);

            // Logs
            var log = new ActivityLog
            {
                ActionTitle = "User Password Update",
                ActionContext = $"User {userAccount.Username} Updated their password",
                CreatedAt = DateTime.Now,
                CreatedBy = userID,
                Module = "Utilities",
                SubModule = "User Logs"
            };

            await _context.ActivityLogs.AddAsync(log);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}