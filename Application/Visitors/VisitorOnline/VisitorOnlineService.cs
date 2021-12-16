using Application.Interfaces.Context;
using Domain.Visitors;
using MongoDB.Driver;
using System;
using System.Linq;

namespace Application.Visitors.VisitorOnline
{
    public class VisitorOnlineService : IVisitorOnlineService
    {
        private readonly IMongoDbContext<OnlineVisitor> _mongoDbContext;
        private readonly IMongoCollection<OnlineVisitor> _mongoCollection;

        public VisitorOnlineService(IMongoDbContext<OnlineVisitor> mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
            _mongoCollection = _mongoDbContext.GetCollection();
        }

        public void ConnectUser(string clientId)
        {
            var exists = _mongoCollection.AsQueryable().FirstOrDefault(u => u.ClientId == clientId);
            if (exists == null)
            {
                _mongoCollection.InsertOne(new OnlineVisitor()
                {
                    ClientId = clientId,
                    Time = DateTime.Now
                });
            }
        }

        public void DisConnectUser(string clientId)
        {
            _mongoCollection.FindOneAndDelete(u => u.ClientId == clientId);
        }

        public int GetCount()
        {
            return _mongoCollection.AsQueryable().Count();
        }
    }
}
