using static Application.Visitors.GetTodayReport.GetTodayReportService;

namespace Application.Visitors.GetTodayReport
{
    public class TodayDto
    {
        public long PageViews { get; set; }
        public long Visitors { get; set; }
        public float ViewsPerVisitor { get; set; }
        public VisitorCountDto VisitPerhour { get; set; }
    }
}
