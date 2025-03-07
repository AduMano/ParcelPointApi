namespace ParcelPointApi.Data.Interface.ParcelLogs
{
    public interface IParcelLogSummaryDto
    {
        Guid ParcelId { get; set; }
        string ParcelName { get; set; }
        string LockerNumber { get; set; }
        string Status { get; set; }
        string Action { get; set; }
        DateTime? LogDate { get; set; }
        DateTime? RetrievedAt { get; set; }
        string LogType { get; set; }
        Guid UserId { get; set; }
        string UserName { get; set; }
    }

    public interface IParcelLogsCountsDto
    {
        int Daily { get; set; }
        int Weekly { get; set; }
        int Monthly { get; set; }
        int Annually { get; set; }
    }

    public class ParcelLogSummaryDto : IParcelLogSummaryDto
    {
        public Guid ParcelId { get; set; }
        public string ParcelName { get; set; }
        public string LockerNumber { get; set; }
        public string Status { get; set; }
        public string Action { get; set; }
        public DateTime? LogDate { get; set; }
        public DateTime? RetrievedAt { get; set; }
        public string LogType { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
    }

    public class ParcelLogsCountsDto : IParcelLogsCountsDto
    {
        public int Daily { get; set; }
        public int Weekly { get; set; }
        public int Monthly { get; set; }
        public int Annually { get; set; }
    }


}
