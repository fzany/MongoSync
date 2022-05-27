using System;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoSync.Models;

namespace MongoSync.Services
{
    public class MongoService
    {
        public readonly IMongoCollection<User> _users;
        public readonly IMongoCollection<BsonDocument> _usersBson;

        public MongoService(IOptions<ConnectionStrings> settings)
        {
            string MongoKey = settings.Value.MongoDb;
            var client = new MongoClient(MongoKey);
            var database = client.GetDatabase(settings.Value.DatabaseName);

            #region LoadCollections
            _users = database.GetCollection<User>(nameof(User).ToLower());
            _usersBson = database.GetCollection<BsonDocument>(nameof(User).ToLower());

            #endregion
        }
    }

}
