using Api;
using MongoDB.Bson;

namespace ApiFullStackClint.Data.MongoDb.MongoDbHelpers
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
                ImgUrls = GetImageUrls(panel,"imgUrls"),
                Paragraphs = GetStringArrayValue(panel, "paragraphs")
            };
        }

        public static List<ImageUrlsDto> GetImageUrls(BsonDocument document, string fieldName)
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
                Label = GetStringValue(image, "label"),
                Alt = GetStringValue(image, "alt")
            };
        }

        private static bool GetBooleanValue(BsonDocument document, string fieldName)
        {
            return document.TryGetValue(fieldName, out var value) && value.AsBoolean;
        }
    }
}
