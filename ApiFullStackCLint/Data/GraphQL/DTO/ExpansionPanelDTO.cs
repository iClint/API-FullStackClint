namespace ApiFullStackCLint.Data.GraphQL.DTO
{
    public class AboutDto
    {
        public string? TabLabel { get; set; }
        public List<string>? Content { get; set; }
        public List<ImageUrlsDto>? ImgUrls { get; set; }
        public List<ExpansionPanelDto>? ExpansionPanels { get; set; }
    }

    public class ExpansionPanelDto
    {
        public bool? Expanded { get; set; }
        public string? Title { get; set; }
        public List<ImageUrlsDto>? ImgUrls { get; set; }
        public List<string>? Paragraphs { get; set; }
    }
    
    public class ImageUrlsDto
    {
        public string? Url { get; set; }
        public string? Label { get; set; }
        public string? Alt { get; set; }
    }
}