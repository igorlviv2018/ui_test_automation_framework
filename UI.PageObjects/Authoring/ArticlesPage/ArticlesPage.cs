using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;

namespace Taf.UI.PageObjects.Authoring
{
    public class ArticlesPage
    {
        private readonly string createArticleButton = "//div[contains(@class,'content-table-wrap')]/button";

        private readonly string newArticleTypeButton = "//div[contains(@class,'modal-body')]//div[contains(@class,'card') and contains(@class,'available')]";

        public void ClickCreateArticleButton() => new Element(createArticleButton).ClickIfExists();

        public void ClickArticleType(ArticleType articleType) => new Element($"({newArticleTypeButton})[{(int)articleType}]").ClickIfExists();
    }
}
