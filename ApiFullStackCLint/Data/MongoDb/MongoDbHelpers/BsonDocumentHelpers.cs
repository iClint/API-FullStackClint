
using ApiFullStackCLint.Data.GraphQL.DTO;
using MongoDB.Bson;

namespace ApiFullStackCLint.Data.MongoDb.MongoDbHelpers
{
    public static class BsonDocumentHelpers
    {
        public static string GetStringValue(BsonDocument document, string fieldName)
        {
            return document.TryGetValue(fieldName, out var value) ? value.AsString : string.Empty;
        }

        public static List<string> GetStringArrayValue(BsonDocument document, string fieldName)
        {
            return document.TryGetValue(fieldName, out var value)
                ? value.AsBsonArray.Select(x => x.AsString).ToList()
                : new List<string>();
        }

        public static List<ExpansionPanelDto> GetExpansionPanels(BsonDocument document, string fieldName)
        {
            return document.TryGetValue(fieldName, out var value) && value.IsBsonArray
                ? value.AsBsonArray.Select(panel => CreateExpansionPanelDto(panel.AsBsonDocument)).ToList()
                : new List<ExpansionPanelDto>();
        }
        
        private static ExpansionPanelDto CreateExpansionPanelDto(BsonDocument panel)
        {
            return new ExpansionPanelDto
            {
                Expanded = GetBooleanValue(panel, "expanded"),
                Title = GetStringValue(panel, "title"),
                ImgUrls = GetImageUrls(panel,"imgUrl"),
                Paragraphs = GetStringArrayValue(panel, "paragraphs")
            };
        }

        private static List<ImageUrlsDto> GetImageUrls(BsonDocument document, string fieldName)
        {
            return document.TryGetValue(fieldName, out var value) && value.IsBsonArray
                ? value.AsBsonArray.Select(image => CreateImageUrlsDto(image.AsBsonDocument)).ToList()
                : new List<ImageUrlsDto>();
        }
                
        private static ImageUrlsDto CreateImageUrlsDto(BsonDocument image)
        {
            return new ImageUrlsDto
            {
                Url = GetStringValue(image, "url"),
                Label = GetStringValue(image, "label")
            };
        }

        private static bool GetBooleanValue(BsonDocument document, string fieldName)
        {
            return document.TryGetValue(fieldName, out var value) && value.AsBoolean;
        }
    }
}
