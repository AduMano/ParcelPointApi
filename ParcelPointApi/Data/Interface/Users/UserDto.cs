using Microsoft.Identity.Client;
using ParcelPointApi.Models;

namespace ParcelPointApi.Data.Interface.Users
{
    public interface IUserDto
    {
        Guid Id { get; set; } // Changed from int to Guid
        string Username { get; set; }
        Guid? CreatedBy { get; set; }
        DateTime? CreatedAt { get; set; }
        Guid? RoleId { get; set; }
        string RoleName { get; set; } // Nullable, as it may not always have a value
        string Password { get; set; }
        bool isActive { get; set; }
    }

    public class UserDto : IUserDto
    {
        public Guid Id { get; set; } // Changed from int to Guid
        public string Username { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid? RoleId { get; set; }
        public string RoleName { get; set; }
        public string Password { get; set; }
        public bool isActive { get; set; }
    }

    public interface IRegisterUserDto
    {
        Guid? UserId { get; set; }
        string FirstName { get; set; }
        string MiddleName { get; set; }
        string LastName { get; set; }
        string? Suffix { get; set; }
        DateTime BirthDate { get; set; }
        Guid Gender { get; set; }
        string Address { get; set; }
        string Email { get; set; }
        string ContactNumber { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        Guid Role { get; set; }
        Guid OperatorID { get; set; }
        string UserType { get; set; }
        string PhotoUrl { get; set; }
    }
    public class RegisterUserDto : IRegisterUserDto
    {
        public Guid? UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Suffix { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public Guid Gender { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Guid Role { get; set; }
        public Guid OperatorID { get; set; }
        public string UserType { get; set; }
        public string PhotoUrl { get; set; }
    }
}
