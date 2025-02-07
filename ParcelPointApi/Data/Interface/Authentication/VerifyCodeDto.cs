namespace ParcelPointApi.Data.Interface.Authentication
{
    public interface IVerifyCodeDto
    {
        string email { get; set; }
        string code { get; set; }
    }
    public class VerifyCodeDto : IVerifyCodeDto
    {
        public string email { get; set; }
        public string code { get; set; }
    }
}
