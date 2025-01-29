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
}
