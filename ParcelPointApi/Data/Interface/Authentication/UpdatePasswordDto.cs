namespace ParcelPointApi.Data.Interface.Authentication
{
    public interface IUpdatePasswordDto
    {
        string email { get; set; }
        string password { get; set; }
    }
    public class UpdatePasswordDto : IUpdatePasswordDto
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}
