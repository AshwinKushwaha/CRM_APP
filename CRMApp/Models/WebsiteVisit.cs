namespace CRMApp.Models
{
    public class WebsiteVisit
    {
        public int Id { get; set; }
        public DateTime VisitDate { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? PageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class DailyVisitStats
    {
        public string Date { get; set; } = string.Empty;
        public int VisitCount { get; set; }
    }
}