using ApiFullStackCLint.Data.GraphQL.DTO;
using ApiFullStackCLint.Data.MongoDb.MongoDbHelpers;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApiFullStackCLint.Data.MongoDB
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;
        private readonly ILogger<MongoDbService> _logger;
        private readonly MongoCollectionHelper _collectionHelper;

        public MongoDbService(IOptions<MongoDbSettings> settings, ILogger<MongoDbService> logger, ILogger<MongoCollectionHelper> collectionHelperLogger)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
            _logger = logger;
            // Create the appropriate logger for MongoCollectionHelper
            _collectionHelper = new MongoCollectionHelper(_database, collectionHelperLogger);
        }

        public async Task<List<AboutDto>> GetAllAboutDocumentsAsync(string collectionName)
        {
            if (!await _collectionHelper.CollectionExistsAsync(collectionName)) 
                return new List<AboutDto>();
            
            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                var documents = await collection.Find(new BsonDocument()).ToListAsync();
                return MapToAboutDto(documents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching documents");
                return new List<AboutDto>();
            }
        }

        public async Task<AboutDto?> GetAboutDocumentByTabLabelAsync(string collectionName, string tabLabel)
        {
            if (!await _collectionHelper.CollectionExistsAsync(collectionName)) 
                return new AboutDto();

            try
            {
                var collection = _database.GetCollection<BsonDocument>(collectionName);
                var filter = Builders<BsonDocument>.Filter.Eq("tabLabel", tabLabel);
                var document = await collection.Find(filter).FirstOrDefaultAsync();
                return document == null ? null : MapToAboutDto(new List<BsonDocument> { document }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the document by tab label");
                return null;
            }
        }

        private static List<AboutDto> MapToAboutDto(IEnumerable<BsonDocument> documents)
        {
            return documents.Select(document =>
            {
                var tabLabel = BsonDocumentHelpers.GetStringValue(document, "tabLabel");
                var content = BsonDocumentHelpers.GetStringArrayValue(document, "content");
                var imgUrls = BsonDocumentHelpers.GetStringArrayValue(document, "imgUrls");
                var expansionPanels = BsonDocumentHelpers.GetExpansionPanels(document, "expansionPanels");

                return new AboutDto
                {
                    TabLabel = tabLabel,
                    Content = content,
                    ImgUrls = imgUrls,
                    ExpansionPanels = expansionPanels
                };
            }).ToList();
        }
    }
}
