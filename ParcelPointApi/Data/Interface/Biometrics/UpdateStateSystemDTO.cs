using Microsoft.AspNetCore.Mvc;

namespace ParcelPointApi.Data.Interface.Biometrics
{
    public interface IUpdateSystemStateDto
    {
        string mode { get; set; }
        int? bioID { get; set; }
    }
    public class UpdateSystemStateDto: IUpdateSystemStateDto
    {
        public string mode { get; set; }
        public int? bioID { get; set; }
    }
}
