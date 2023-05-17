using Taf.UI.Core.Constants;
using Taf.UI.Core.Element;
using Taf.UI.Core.Interfaces;
using System.Collections.Generic;

namespace Taf.UI.PageObjects.Taf
{
    public class ArticlesTable : BasePage, ITafItemsTable
    {
        private const string tableRow = "//div[@class='list-item-content']";

        private readonly string articleByTitle = tableRow + "/div//h4[contains(text(),'{0}')]";

        private readonly string articleTitle = "//div[contains(@class,'list-item-main')]/h4";

        private readonly string articleLink = "//div[contains(@class,'list-card-item')]/a";

        private readonly string articleByLink = "//div[contains(@class,'list-card-item')]/a[contains('href','{0}')]";

        private readonly string closeButton = "//div[@class='card-header']//a[@title='Close']";

        //redesign
        private const string articleInGrid = "//div[@class='relative']//div[contains(@class,'card-body')]";

        //private readonly string articleInGridTitle = articleInGrid + "/h3";

        private readonly string articleInGridDescription = articleInGrid + "/p";

        public int GetRowCount() => new Element(tableRow).Count;

        public void ClickItemTitle(string title) => new Element(string.Format(articleByTitle, title)).Click();

        public void ClickItemLink(string link) => new Element(string.Format(articleByLink, link)).Click();

        //to optimize
        public bool IsItemPresent(string title) => new Element(string.Format(articleByTitle, title)).Exists(WaitConstants.ImplicitWaitInSec);

        public List<string> GetTitleColumn() => GetTextOfElementsViaJs(articleTitle);

        //public List<string> GetTitlesInGrid() => GetTextOfElementsViaJs(articleInGridTitle);

        public List<string> GetLinkColumn() => GetAttributeOfElementsViaJs("href", articleLink);

        public void ClickCloseButton() => new Element(closeButton).ClickIfExists();
    }
}
