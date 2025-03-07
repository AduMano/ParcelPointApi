
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
    public interface IMemberInfo
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
        UserRelationship Relationship { get; set; }
        bool? IsAuthorized { get; set; }
        Guid? GroupMemberId { get; set; }
    }

    public interface IUserDetailsDto
    {
        Guid UserId { get; set; }

        string Username { get; set; }
        string FirstName { get; set; }
        string MiddleName { get; set; }
        string LastName { get; set; }
        string Suffix { get; set; }

        DateTime? Birthdate { get; set; }
        string Gender { get; set; }
        Guid GenderID { get; set; }

        string Address { get; set; }
        string ContactNumber { get; set; }
        string Email { get; set; }

        string PhotoUrl { get; set; } // If you store photo path/URL somewhere
        bool IsActive { get; set; }

        string RoleName { get; set; }
        Guid RoleID { get; set; }
        // Add any more fields you need to display...
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

    public class MemberInfoDTO : IMemberInfo
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
        public UserRelationship Relationship { get; set; }
        public bool? IsAuthorized { get; set; }
        public Guid? GroupMemberId { get; set; }
    }

    public class UserDetailsDto : IUserDetailsDto
    {
        public Guid UserId { get; set; }

        public string Username { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }

        public DateTime? Birthdate { get; set; }
        public string Gender { get; set; }
        public Guid GenderID { get; set; }

        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }

        public string PhotoUrl { get; set; } // If you store photo path/URL somewhere
        public bool IsActive { get; set; }

        public string RoleName { get; set; }
        public Guid RoleID { get; set; }
        // Add any more fields you need to display...
    }

}
