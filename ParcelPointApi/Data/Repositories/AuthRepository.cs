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
        Task<bool> VerifyEmailAsync(string email, string except, string type);
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
                ActionTitle = "Admin Logged In",
                ActionContext = $"Admin {user.Username} Just Logged In",
                CreatedAt = DateTime.Now,
                CreatedBy = user.Id,
                Module = "Utilities",
                SubModule = "User Logs"
            };

            await _context.ActivityLogs.AddAsync(log);
            await _context.SaveChangesAsync();

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
            var user = await _context.Users
            .Where(u => u.Id == userID)
            .Select(u => new
            {
                Username = u.Username,
                RoleName = u.Role != null ? u.Role.Name : "No Role"
            })
            .FirstOrDefaultAsync();

            var log = new ActivityLog
            {
                ActionTitle = $"{user.RoleName} Logged Out",
                ActionContext = $"{user.RoleName} {user.Username} Just Logged Out",
                CreatedAt = DateTime.Now,
                CreatedBy = userID,
                Module = "Utilities",
                SubModule = "User Logs"
            };

            await _context.ActivityLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> VerifyEmailAsync(string email, string except, string type)
        {
            if (type == "user")
            {
                var user = await _context.UserInformations
                .Where(u =>
                    u.Email == email &&
                    u.User.Role.Name == "Users" &&
                    u.Email != except
                )
                .FirstOrDefaultAsync();

                return (user != null) ? true : false;
            }
            else if (type == "admin")
            {
                var user = await _context.UserInformations
                .Where(u =>
                    u.Email == email &&
                    u.User.Role.Name == "Admin" &&
                    u.Email != except
                )
                .FirstOrDefaultAsync();

                return (user != null) ? true : false;
            }
            else
            {
                return false;
            }

        }

        public async Task<string> GenerateVerificationCodeByEmail(string email)
        {
            var existingCode = await _context.EmailVerifications
                .Where(e => e.Email == email && e.IsUsed == false && e.ExpiresAt > DateTime.Now)
                .OrderByDescending(e => e.CreatedAt) // Get the latest record first
                .FirstOrDefaultAsync();

            if (existingCode != null)
            {
                // If there's an active code, just update the expiration time
                existingCode.ExpiresAt = DateTime.Now.AddMinutes(10);
                await _context.SaveChangesAsync();
                return existingCode.VerificationCode;
            }

            // No active code found, generate a new unique verification code
            string code;
            EmailVerification existingUsedCode;

            do
            {
                code = Generate6Digits.GenerateVerificationCode();

                // Check if the generated code exists and is already used
                existingUsedCode = await _context.EmailVerifications
                    .Where(e => e.VerificationCode == code && e.IsUsed == true)
                    .FirstOrDefaultAsync();

            } while (existingUsedCode == null && await _context.EmailVerifications.AnyAsync(e => e.VerificationCode == code && e.IsUsed == false));

            if (existingUsedCode != null)
            {
                // If the generated code exists but was used, reset it
                existingUsedCode.IsUsed = false;
                existingUsedCode.ExpiresAt = DateTime.Now.AddMinutes(10);
                await _context.SaveChangesAsync();
                return existingUsedCode.VerificationCode;
            }

            // If a completely new code was generated, create a new record
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
            // Fetch the verification code for the given email
            var verificationCode = await _context.EmailVerifications
                .Where(e => e.Email == email && e.VerificationCode == code)
                .FirstOrDefaultAsync();

            // If the code doesn't exist, return false
            if (verificationCode == null) return false;

            // Ensure `IsUsed` is treated as `false` when `null`
            bool isUsed = verificationCode.IsUsed ?? false;

            // If the code is expired or already used, return false
            if (verificationCode.ExpiresAt <= DateTime.Now || isUsed)
            {
                return false;
            }

            // Mark the code as used and save changes
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
                ActionContext = $"{userAccount.Username} Updated their password",
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