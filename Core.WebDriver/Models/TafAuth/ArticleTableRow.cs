using Taf.UI.Core.Enums;

namespace Taf.UI.Core.Models.TafAuth
{
    public class ArticleTableRow
    {
        public bool IsArticleSelected { get; set; } = false;

        public ArticleType ArticleType { get; set; }

        public string ArticleTitle { get; set; } = string.Empty;

        public string ArticleDescription { get; set; } = string.Empty;

        public string ArticleId { get; set; } = string.Empty;

        public ArticleStatus ArticleStatus { get; set; } = ArticleStatus.None;

        public string ArticleVersion { get; set; } = string.Empty;

        public string ArticleOwner { get; set; } = string.Empty;

        public string ArticleReviewByDate { get; set; } = string.Empty;

        public string ArticleCreatedDate { get; set; } = string.Empty;

        public string ArticleModifiedDate { get; set; } = string.Empty;
    }
}
