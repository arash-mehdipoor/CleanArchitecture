using static Application.Visitors.GetTodayReport.GetTodayReportService;

namespace Application.Visitors.GetTodayReport
{
    public class GeneralStatesDto
    {
        public long TotalPageView { get; set; }
        public long TotalVisitors { get; set; }
        public float PageViewsPerVisit { get; set; }

        public VisitorCountDto VisitPerDay { get; set; }
    }
}
