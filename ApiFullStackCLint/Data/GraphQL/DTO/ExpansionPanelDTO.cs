namespace ApiFullStackClint.Data.GraphQL.DTO
{
    public class AboutDto
    {
        public string? TabLabel { get; set; }
        public List<string>? Content { get; set; }
        public List<ImageUrlsDto>? ImgUrls { get; set; }
        public ImageViewerStylesDto? ImageViewerStyles { get; set; }
        public List<ExpansionPanelDto>? ExpansionPanels { get; set; }
    }

    public class ExpansionPanelDto
    {
        public bool? Expanded { get; set; }
        public string? Title { get; set; }
        public List<ImageUrlsDto>? ImgUrls { get; set; }

        public ImageViewerStylesDto? ImageViewerStyles { get; set; }
        public List<string>? Paragraphs { get; set; }
    }

    public class ImageUrlsDto
    {
        public string? Url { get; set; }
        public string? Label { get; set; }
        public string? Alt { get; set; }
    }

    public class ImageViewerStylesDto
    {
        public string? ViewerStyle { get; set; }
        public string? ViewSize { get; set; }
        public bool? IsPreview { get; set; }
        public int? SoloIndex { get; set; }
        public CarouselConfigDto? CarouselConfig { get; set; }
    }

    public class CarouselConfigDto
    {
        public int? Interval { get; set; }
        public bool? ShowNavigationArrows { get; set; }
        public bool? ShowNavigationIndicators { get; set; }
        public bool? PauseOnFocus { get; set; }
        public bool? PauseOnHover { get; set; }
    }
}
