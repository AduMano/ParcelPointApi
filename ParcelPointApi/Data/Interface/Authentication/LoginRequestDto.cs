using System.ComponentModel.DataAnnotations;

namespace ParcelPointApi.Data.Interface.Authentication
{
    public interface ILoginRequestDto
    {
        string username { get; set; }
        string password { get; set; }
        string type { get; set; }
    }
    public class LoginRequestDto : ILoginRequestDto
    {
        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public string type { get; set; }
    }
}
