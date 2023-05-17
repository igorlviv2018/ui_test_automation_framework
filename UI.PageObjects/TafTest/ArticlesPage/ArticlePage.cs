using Taf.UI.Core.Element;

namespace Taf.UI.PageObjects.Taf
{
    public class ArticlePage : BasePage
    {
        private const string articlePage = "//main[@class='article-page']";

        private readonly string articleTitle = articlePage + "//div[contains(@class,'card-header')]//h2";

        private readonly string closeButton = "//div[@class='card-header']//a[@title='Close']";

        public string GetTitle() => new Element(articleTitle).Text;

        public void ClickCloseButton() => new Element(closeButton).ClickIfExists();
    }
}
