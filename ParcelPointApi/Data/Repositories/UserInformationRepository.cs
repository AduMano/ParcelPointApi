using Microsoft.EntityFrameworkCore;
using ParcelPointApi.Data.Interface.Users;

namespace ParcelPointDB.Data.Repositories
{
    public interface IUserInfoRepository
    {
        // Get UserInformation
        Task<UserInformationDTO?> GetUserInformationByIdAsync(Guid id);

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