using CRMApp.Areas.Identity.Data;
using CRMApp.Models;

namespace CRMApp.Services
{
    public interface IWebsiteVisitService
    {
        Task LogVisitAsync(string ipAddress, string userAgent, string pageUrl);
        List<DailyVisitStats> GetDailyVisitStats(int days = 30);
        int GetTotalVisitsToday();
        int GetTotalVisitsThisWeek();
        int GetTotalVisitsThisMonth();
    }

    public class WebsiteVisitService : IWebsiteVisitService
    {
        private readonly ApplicationUserIdentityContext _context;
        private readonly ILogger<WebsiteVisitService> _logger;

        public WebsiteVisitService(ApplicationUserIdentityContext context, ILogger<WebsiteVisitService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task LogVisitAsync(string ipAddress, string userAgent, string pageUrl)
        {
            try
            {
                var visit = new WebsiteVisit
                {
                    VisitDate = DateTime.Now.Date,
                    IpAddress = ipAddress,
                    UserAgent = userAgent,
                    PageUrl = pageUrl,
                    CreatedAt = DateTime.Now
                };

                _context.WebsiteVisits.Add(visit);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging website visit");
            }
        }

        public List<DailyVisitStats> GetDailyVisitStats(int days = 30)
        {
            try
            {
                var startDate = DateTime.Now.Date.AddDays(-days);
                
                var stats = _context.WebsiteVisits
                    .Where(v => v.VisitDate >= startDate)
                    .GroupBy(v => v.VisitDate)
                    .Select(g => new DailyVisitStats
                    {
                        Date = g.Key.ToString("MMM dd"),
                        VisitCount = g.Count()
                    })
                    .OrderBy(s => s.Date)
                    .ToList();

                // Fill in missing dates with zero visits
                var allDates = Enumerable.Range(0, days + 1)
                    .Select(i => DateTime.Now.Date.AddDays(-days + i))
                    .ToList();

                var result = allDates.Select(date => 
                {
                    var stat = stats.FirstOrDefault(s => s.Date == date.ToString("MMM dd"));
                    return new DailyVisitStats
                    {
                        Date = date.ToString("MMM dd"),
                        VisitCount = stat?.VisitCount ?? 0
                    };
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting daily visit stats");
                return new List<DailyVisitStats>();
            }
        }

        public int GetTotalVisitsToday()
        {
            try
            {
                var today = DateTime.Now.Date;
                return _context.WebsiteVisits.Count(v => v.VisitDate == today);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting today's visit count");
                return 0;
            }
        }

        public int GetTotalVisitsThisWeek()
        {
            try
            {
                var startOfWeek = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek);
                return _context.WebsiteVisits.Count(v => v.VisitDate >= startOfWeek);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting this week's visit count");
                return 0;
            }
        }

        public int GetTotalVisitsThisMonth()
        {
            try
            {
                var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                return _context.WebsiteVisits.Count(v => v.VisitDate >= startOfMonth);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting this month's visit count");
                return 0;
            }
        }
    }
}