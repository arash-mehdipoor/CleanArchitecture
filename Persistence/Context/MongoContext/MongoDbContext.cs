using Application.Interfaces.Context;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Context.MongoContext
{
    public class MongoDbContext<T> : IMongoDbContext<T>
    {
        private readonly IMongoDatabase db;
        private readonly IMongoCollection<T> _mongoCollection;

        public MongoDbContext()
        {
            var client = new MongoClient();
            db = client.GetDatabase("visitorDb");
            _mongoCollection = db.GetCollection<T>(typeof(T).Name);
        }

        public IMongoCollection<T> GetCollection()
        {
            return _mongoCollection;
        }
    }
}
