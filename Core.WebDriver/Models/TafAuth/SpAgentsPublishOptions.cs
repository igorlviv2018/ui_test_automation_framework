namespace Taf.UI.Core.Models.TafAuth
{
    public class TafPublishOptions
    {
        public bool IsChannelEnabled { get; set; } = false;

        public bool IsArticlesLocationSelected { get; set; } = false;

        public bool PostToNewsFeed { get; set; } = false;

        public string ReleaseNote { get; set; } = string.Empty;

        public bool IncludeArticleInSearch { get; set; } = false;
    }
}
