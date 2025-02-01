using ParcelPointApi.Data.Repositories;

namespace ParcelPointApi.Services
{
    public interface IUserRelationshipService
    {
        Task<IEnumerable<UserRelationship>> GetAllUserRelationshipAsync();
        Task<UserRelationship?> GetUserRelationshipByIdAsync(Guid id);
        Task<string> CreateUserRelationshipAsync(UserRelationship relationshipData);
    }
    public class UserRelationshipService : IUserRelationshipService
    {
        private readonly IUserRelationshipRepository _userRelationshipRepository;

        public UserRelationshipService(IUserRelationshipRepository userRelationshipRepository)
        {
            _userRelationshipRepository = userRelationshipRepository;
        }

        public async Task<IEnumerable<UserRelationship>> GetAllUserRelationshipAsync()
        {
            return await _userRelationshipRepository.GetAllUserRelationshipAsync();
        }

        public async Task<UserRelationship?> GetUserRelationshipByIdAsync(Guid id)
        {
            return await _userRelationshipRepository.GetUserRelationshipByIdAsync(id);
        }

        public async Task<string> CreateUserRelationshipAsync(UserRelationship relationshipData)
        {
            var data = await _userRelationshipRepository.CreateUserRelationshipAsync(relationshipData);
            if (!data) return "creation failed";

            return "success";
        }
    }
}
