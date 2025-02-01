using Microsoft.EntityFrameworkCore;

namespace ParcelPointApi.Data.Repositories
{
    public interface IUserRelationshipRepository
    {
        Task<IEnumerable<UserRelationship>> GetAllUserRelationshipAsync();
        Task<UserRelationship?> GetUserRelationshipByIdAsync(Guid id);
        Task<bool> CreateUserRelationshipAsync(UserRelationship relationshipData);
    }
    public class UserRelationshipRepository : IUserRelationshipRepository
    {
        private readonly ParcelPointDbContext _context;

        public UserRelationshipRepository(ParcelPointDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserRelationship>> GetAllUserRelationshipAsync()
        {
            return await _context.UserRelationships
                .OrderBy(r => r.Name)
                .ToListAsync();
        }

        public async Task<UserRelationship?> GetUserRelationshipByIdAsync(Guid id)
        {
            return await _context.UserRelationships
                .Where(relationship => relationship.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CreateUserRelationshipAsync(UserRelationship relationshipData)
        {
            try
            {
                _context.UserRelationships.Add(relationshipData);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
