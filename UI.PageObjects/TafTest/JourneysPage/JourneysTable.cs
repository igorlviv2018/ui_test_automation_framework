using Taf.UI.Core.Constants;
using Taf.UI.Core.Element;
using Taf.UI.Core.Interfaces;
using System.Collections.Generic;

namespace Taf.UI.PageObjects.Taf
{
    public class JourneysTable : BasePage, ITafItemsTable
    {
        private const string tableRow = "//div[contains(@class,'list-card-item')]";

        private readonly string journeyByTitle = tableRow + "//div[contains(@class,'list-header')]/div[@class='title' and contains(text(),'{0}')]";

        private readonly string journeyTitle = "//div[contains(@class,'list-header')]/div[@class='title']";

        private readonly string closeButton = "//div[@class='card-header']//a[@title='Close']";

        public int GetRowCount() => new Element(tableRow).Count;

        public void ClickItemTitle(string title) => new Element(string.Format(journeyByTitle, title)).Click();

        //to optimize
        public bool IsItemPresent(string title) => new Element(string.Format(journeyByTitle, title)).Exists(WaitConstants.ImplicitWaitInSec);

        public List<string> GetTitleColumn() => GetTextOfElementsViaJs(journeyTitle);

        public void ClickCloseButton() => new Element(closeButton).ClickIfExists();
    }
}
