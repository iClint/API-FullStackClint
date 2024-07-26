using ApiFullStackClint.Data.GraphQL.DTO;
using MongoDB.Bson;

namespace ApiFullStackClint.Data.MongoDb.MongoDbHelpers;

public static class BsonDocumentHelpers
{
    public static string GetStringValue(BsonDocument document, string fieldName) =>
        document.TryGetValue(fieldName, out var value) ? value.AsString : string.Empty;

    public static List<string> GetStringArrayValue(BsonDocument document, string fieldName) =>
        document.TryGetValue(fieldName, out var value) && value.IsBsonArray
            ? value.AsBsonArray.Select(x => x.AsString).ToList()
            : new List<string>();

    public static List<ExpansionPanelDto> GetExpansionPanels(BsonDocument document, string fieldName) =>
        document.TryGetValue(fieldName, out var value) && value.IsBsonArray
            ? value.AsBsonArray.Select(panel => CreateDto<ExpansionPanelDto>(panel.AsBsonDocument, CreateExpansionPanelDto)).ToList()
            : new List<ExpansionPanelDto>();

    public static ImageViewerStylesDto GetImageViewerStyles(BsonDocument document, string fieldName) =>
        document.TryGetValue(fieldName, out var value) && value.IsBsonDocument
            ? CreateDto<ImageViewerStylesDto>(value.AsBsonDocument, CreateImageViewerStylesDto)
            : new ImageViewerStylesDto();

    public static List<ImageUrlsDto> GetImageUrls(BsonDocument document, string fieldName) =>
        document.TryGetValue(fieldName, out var value) && value.IsBsonArray
            ? value.AsBsonArray.Select(image => CreateDto<ImageUrlsDto>(image.AsBsonDocument, CreateImageUrlsDto)).ToList()
            : new List<ImageUrlsDto>();

    private static T CreateDto<T>(BsonDocument document, Func<BsonDocument, T> createDtoFunc) =>
        createDtoFunc(document);

    private static ImageUrlsDto CreateImageUrlsDto(BsonDocument image) => new()
    {
        Url = GetStringValue(image, "url"),
        Label = GetStringValue(image, "label"),
        Alt = GetStringValue(image, "alt")
    };
    
    private static ImageViewerStylesDto CreateImageViewerStylesDto(BsonDocument document) => new()
    {
        ViewerStyle = GetStringValue(document, "viewerStyle"),
        ViewerSize = GetStringValue(document, "viewerSize"),
        IsPreview = GetBooleanValue(document, "isPreview"),
        SoloIndex = GetIntValue(document, "soloIndex"),
        CarouselConfig = GetCarouselConfig(document, "carouselConfig")
    };

    private static CarouselConfigDto GetCarouselConfig(BsonDocument document, string fieldName) =>
        document.TryGetValue(fieldName, out var value) && value.IsBsonDocument
            ? CreateDto<CarouselConfigDto>(value.AsBsonDocument, CreateCarouselConfigDto)
            : new CarouselConfigDto();

    private static CarouselConfigDto CreateCarouselConfigDto(BsonDocument document) => new()
    {
        Interval = GetIntValue(document, "interval"),
        ShowNavigationArrows = GetBooleanValue(document, "showNavigationArrows"),
        ShowNavigationIndicators = GetBooleanValue(document, "showNavigationIndicators"),
        PauseOnFocus = GetBooleanValue(document, "pauseOnFocus"),
        PauseOnHover = GetBooleanValue(document, "pauseOnHover")
    };

    private static ExpansionPanelDto CreateExpansionPanelDto(BsonDocument panel) => new()
    {
        Expanded = GetBooleanValue(panel, "expanded"),
        Title = GetStringValue(panel, "title"),
        ImgUrls = GetImageUrls(panel, "imgUrls"),
        Paragraphs = GetStringArrayValue(panel, "paragraphs"),
        ImageViewerStyles = GetImageViewerStyles(panel, "imageViewerStyles")
    };

    private static bool GetBooleanValue(BsonDocument document, string fieldName) =>
        document.TryGetValue(fieldName, out var value) && value.IsBoolean && value.AsBoolean;

    private static int GetIntValue(BsonDocument document, string fieldName) =>
        document.TryGetValue(fieldName, out var value) && value.IsInt32 ? value.AsInt32 : 0;
}