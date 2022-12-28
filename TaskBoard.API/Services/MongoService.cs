using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace TaskBoard.API.Services
{
    public abstract class MongoService
    {
        protected const string DB_NAME = "TaskBoardDB";

        protected readonly IMongoClient _mongo;

        public MongoService(IMongoClient mongo)
        {
            _mongo = mongo;
        }

        protected IMongoDatabase GetDatabase() => _mongo.GetDatabase(DB_NAME);

        protected IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            if (string.IsNullOrWhiteSpace(collectionName))
                throw new ArgumentException("invalid collection name");

            return GetDatabase().GetCollection<T>(collectionName);
        }
    }
}