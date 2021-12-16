using Application.Interfaces.Context;
using Domain.Visitors;
using MongoDB.Driver;
using System;
using System.Linq;

namespace Application.Visitors.GetTodayReport
{
    public partial class GetTodayReportService : IGetTodayReportService
    {
        private readonly IMongoDbContext<Visitor> _mongoDbContext;
        private readonly IMongoCollection<Visitor> _visitorMongoCollection;

        public GetTodayReportService(IMongoDbContext<Visitor> mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
            _visitorMongoCollection = _mongoDbContext.GetCollection();
        }
        public ResultTodayReportDto Execute()
        {
            DateTime startDate = DateTime.Now.Date;
            DateTime endDate = DateTime.Now.AddDays(1);

            var todayPageViewCount = _visitorMongoCollection.AsQueryable()
                .Where(v => v.Time <= startDate && v.Time < endDate).LongCount();
            var totalVisitorCount = _visitorMongoCollection.AsQueryable()
               .Where(v => v.Time <= startDate && v.Time < endDate).GroupBy(v => v.VisitorId).LongCount();

            var allPageViewCount = _visitorMongoCollection.AsQueryable().LongCount();
            var allVisitorCount = _visitorMongoCollection.AsQueryable().GroupBy(v => v.VisitorId).LongCount();


            var todayPageViewList = _visitorMongoCollection.AsQueryable()
                .Where(v => v.Time >= startDate && v.Time < endDate)
                .Select(v => new { v.Time }).ToList();

            VisitorCountDto visitorCountDto = new VisitorCountDto()
            {
                Display = new string[24],
                Value = new int[24]
            };

            for (int i = 0; i <= 23; i++)
            {
                visitorCountDto.Display[i] = $"{i}";
                visitorCountDto.Value[i] = todayPageViewList.Where(v => v.Time.Hour == i).Count();
            }


            DateTime MountStart = DateTime.Now.AddDays(-30);
            DateTime MountEnds = DateTime.Now.AddDays(1);

            var Mount_PageViewList = _visitorMongoCollection.AsQueryable()
                .Where(v => v.Time >= MountStart && v.Time < MountEnds)
                .Select(v => new { v.Time }).ToList();


            VisitorCountDto visitorPerDay = new VisitorCountDto()
            {
                Display = new string[31],
                Value = new int[31]
            };

            for (int i = 0; i <= 30; i++)
            {
                var currentDay = DateTime.Now.AddDays(i * (-1));
                visitorPerDay.Display[i] = i.ToString();
                visitorPerDay.Value[i] = Mount_PageViewList.Where(v => v.Time.Date == currentDay.Date).Count();

            }

            var visitors = _visitorMongoCollection.AsQueryable()
                .OrderByDescending(v => v.Time)
                .Select(v => new VisitorsDto
                {
                    Id = v.Id,
                    Browser = v.Browser.Family,
                    CurrentLink = v.CurrentLink,
                    Ip = v.IP,
                    OperationSystem = v.OperationSystem.Family,
                    IsSpider = v.Device.IsSpider,
                    ReferrerLink = v.ReferrerLink,
                    Time = v.Time,
                    VisitorId = v.VisitorId
                }).ToList();

            return new ResultTodayReportDto()
            {
                GeneralStats = new GeneralStatesDto()
                {
                    TotalVisitors = allVisitorCount,
                    TotalPageView = allPageViewCount,
                    PageViewsPerVisit = GetAvg(allPageViewCount, allVisitorCount),
                    VisitPerDay = visitorPerDay
                },
                Today = new TodayDto()
                {
                    PageViews = todayPageViewCount,
                    Visitors = totalVisitorCount,
                    ViewsPerVisitor = GetAvg(todayPageViewCount, totalVisitorCount),
                    VisitPerhour = visitorCountDto
                },
                visitors = visitors
            };
        }

        public float GetAvg(long visitPage, long visitor)
        {
            if (visitor == 0)
            {
                return 0;
            }
            else
            {
                return visitPage / visitor;
            }
        }

        public class VisitorsDto
        {
            public string Id { get; set; }
            public string Ip { get; set; }
            public string CurrentLink { get; set; }
            public string ReferrerLink { get; set; }
            public string Browser { get; set; }
            public string OperationSystem { get; set; }
            public bool IsSpider { get; set; }
            public DateTime Time { get; set; }
            public string VisitorId { get; set; }

        }
    }
}
