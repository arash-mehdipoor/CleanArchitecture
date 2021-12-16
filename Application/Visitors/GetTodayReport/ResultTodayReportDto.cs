using System.Collections.Generic;
using static Application.Visitors.GetTodayReport.GetTodayReportService;

namespace Application.Visitors.GetTodayReport
{
    public class ResultTodayReportDto
    {
        public GeneralStatesDto GeneralStats { get; set; }
        public TodayDto Today { get; set; }
        public List<VisitorsDto> visitors { get; set; }
}
}
