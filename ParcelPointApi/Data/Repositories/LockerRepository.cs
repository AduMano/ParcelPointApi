using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ParcelPointApi.Data.Interface.Users;

public interface ILockerRepository
{
    Task<IEnumerable<TableStatus>> GetLockerStatus(int[] ids);
    Task<bool> UpdateLockerStatus(int id);
}

public class LockerRepository : ILockerRepository
{
    private readonly ParcelPointDbContext _context;
    private readonly PasswordHelper _passwordHelper;

    public LockerRepository(ParcelPointDbContext context, PasswordHelper passwordHelper)
    {
        _context = context;
    }

    public async Task<IEnumerable<TableStatus>> GetLockerStatus(int[] ids)
    {
        var lockerStatuses = await _context.TableStatuses
            .Where(l => ids.Contains(l.LockerNumber))
            .ToListAsync();

        return lockerStatuses;
    }

    public async Task<bool> UpdateLockerStatus(int id)
    {
        try
        {
            int rowsAffected = await _context.Database.ExecuteSqlRawAsync(
                "UPDATE Table_Status SET is_open = {0} WHERE locker_number = {1}", false, id);

            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            // Optionally log the exception
            Console.WriteLine("Error updating locker status: " + ex.Message);
            return false;
        }
    }

};