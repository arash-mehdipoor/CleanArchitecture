using Application.Interfaces.Context;
using Domain.Visitors;
using MongoDB.Driver;
using System;
using System.Linq;

namespace Application.Visitors.GetTodayReport
{
    public class GetTodayReportService : IGetTodayReportService
    {
        private readonly IMongoDbContext<Visitor> _mongoDbContext;
        private readonly IMongoCollection<Visitor> _visitorMongoCollection;

        public GetTodayReportService(IMongoDbContext<Visitor> mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
            _visitorMongoCollection = _mongoDbContext.GetCollection();
        }
        public ResultTodayReportDto Exequte()
        {
            DateTime startDate = DateTime.Now.Date;
            DateTime endDate = DateTime.Now.AddDays(1);

            var todayPageViewCount = _visitorMongoCollection.AsQueryable()
                .Where(v => v.Time <= startDate && v.Time < endDate).LongCount();
            var totalVisitorCount = _visitorMongoCollection.AsQueryable()
               .Where(v => v.Time <= startDate && v.Time < endDate).GroupBy(v => v.VisitorId).LongCount();

            var allPageViewCount = _visitorMongoCollection.AsQueryable().LongCount();
            var allVisitorCount = _visitorMongoCollection.AsQueryable().GroupBy(v => v.VisitorId).LongCount();

            return new ResultTodayReportDto()
            {
                GeneralStatesDto = new GeneralStatesDto()
                {
                    TotalVisitors = allVisitorCount,
                    TotalPageView = allPageViewCount,
                    PageViewsPerVisit = (allPageViewCount / allVisitorCount) == 0 ? 0 : (allPageViewCount / allVisitorCount)
                },
                TodayDto = new TodayDto()
                {
                    PageViews = todayPageViewCount,
                    Visitors = totalVisitorCount,
                    ViewsPerVisitor = (todayPageViewCount / totalVisitorCount) == 0 ? 0 : (todayPageViewCount / totalVisitorCount)
                }
            };
        }
    }
}
