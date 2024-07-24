using MongoDB.Driver;
namespace Api
{
    public class MongoCollectionHelper
    {
        private readonly IMongoDatabase _database;
        private readonly ILogger<MongoCollectionHelper> _logger;

        public MongoCollectionHelper(IMongoDatabase database, ILogger<MongoCollectionHelper> logger)
        {
            _database = database;
            _logger = logger;
        }
        
        public async Task<bool> CollectionExistsAsync(string collectionName)
        {
            if (await GetCollectionListAsync(collectionName)) 
                return true;

            _logger.LogWarning($"Collection '{collectionName}' does not exist.");
            return false;
        }

        private async Task<bool> GetCollectionListAsync(string collectionName)
        {
            var collectionNames = await (await _database.ListCollectionNamesAsync()).ToListAsync();
            return collectionNames.Contains(collectionName);
        }
    }
}