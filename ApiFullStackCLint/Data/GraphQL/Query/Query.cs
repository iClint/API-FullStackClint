using ApiFullStackCLint.Data.GraphQL.DTO;
using ApiFullStackCLint.Data.MongoDB;

namespace ApiFullStackCLint.Data.GraphQL.Query
{
    public class Query
    {
        private readonly MongoDbService _mongoDbService;

        public Query(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public Task<List<AboutDto>> GetAllAboutDocuments(string collectionName)
        {
            return _mongoDbService.GetAllAboutDocumentsAsync(collectionName);
        }
        
        public Task<AboutDto?> GetAboutDocumentByTabLabel(string collectionName, string tabLabel)
        {
            return _mongoDbService.GetAboutDocumentByTabLabelAsync(collectionName, tabLabel);
        }

    }
}