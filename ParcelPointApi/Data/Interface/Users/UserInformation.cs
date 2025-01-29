
namespace ParcelPointApi.Data.Interface.Users
{

    public interface IUserInformation
    {
        Guid Id { get; set; }
        string FirstName { get; set; }
        string MiddleName { get; set; }
        string LastName { get; set; }
        string? Suffix { get; set; }
        DateOnly? BirthDate { get; set; }
        string Address { get; set; }
        string ContactNumber { get; set; }
        string PhotoUrl { get; set; }
        string Email { get; set; }
        string Username { get; set; }
    }

    public class UserInformationDTO : IUserInformation
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string? Suffix { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string PhotoUrl { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }

}
