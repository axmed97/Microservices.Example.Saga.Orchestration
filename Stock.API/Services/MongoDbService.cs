using MongoDB.Driver;

namespace Stock.API.Services
{
    public class MongoDbService
    {
        private readonly IMongoDatabase mongoDatabase;
        public MongoDbService(IConfiguration configuration)
        {
            MongoClient mongoClient = new(configuration.GetConnectionString("Default"));
            mongoDatabase = mongoClient.GetDatabase("Default");
        }

        public IMongoCollection<T> GetCollection<T>() => mongoDatabase.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
    }
}
