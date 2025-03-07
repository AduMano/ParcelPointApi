namespace ParcelPointApi.Data.Interface.ParcelLogs
{
    public interface IGenerateParcelLog
    {
        string user_number { get; set; } 
        int size { get; set; }
    }
    public class GenerateParcelLogDto : IGenerateParcelLog
    {
        public string user_number { get; set; }
        public int size { get; set; }
    }
}
