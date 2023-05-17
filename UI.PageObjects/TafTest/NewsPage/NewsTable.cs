using Taf.UI.Core.Element;
using System.Collections.Generic;

namespace Taf.UI.PageObjects.Taf.NewsPage
{
    public class NewsTable : BasePage
    {
        private const string tableRow = "//div[@class='list-item-content']";

        private readonly string tableTabByName = "//ul[contains(@class,'header-tabs')]//a[contains(@class,'nav-link') and contains(text(),'{0}')]";

        private const string newsContents = "//div[contains(@class,'list-item-main')]/div[contains(@class,'news-item-inline')]";

        private readonly string newsCaption = "//h4";

        private readonly string linkToItem = newsContents + "//a";

        private readonly string releaseNotesText = newsContents + "/p[contains(@class,'rn-text')]";

        private readonly string dateTime = "//div[@class='list-item-footer']//span[not(contains(@class,'username'))]";

        public void ClickTab(string tabName) => new Element(string.Format(tableTabByName, tabName)).ClickIfExists();

        public bool IsTabActive(string tabName) => new Element(string.Format(tableTabByName, tabName)).GetAttribute("class").Contains("active");

        public bool IsTabPresent(string tabName) => new Element(string.Format(tableTabByName, tabName)).IsDisplayedSafe();

        public int GetRowCount() => new Element(newsContents).Count;

        private string RowAtPositionXpath(int position) => IndexedXpath(tableRow, position);

        //public string GetCaption(int rowPosition) => new Element(RowAtPositionXpath(rowPosition) + newsCaption).Text;

        //public string GetLinkText(int rowPosition) => new Element(RowAtPositionXpath(rowPosition) + linkToItem).Text;

        public string GetReleaseNotes(int rowPosition)
        {
            Element releaseNotes = new Element(RowAtPositionXpath(rowPosition) + releaseNotesText);

            return releaseNotes.Exists() ? releaseNotes.Text : string.Empty;
        }

        public string GetDate(int rowPosition) => new Element(RowAtPositionXpath(rowPosition) + dateTime).Text;

        public List<string> GetNewsCaptionColumn() => GetTextOfElementsViaJs(newsCaption);

        public List<string> GetLinkTextColumn() => GetTextOfElementsViaJs(linkToItem);

        public List<string> GetDateColumn() => GetTextOfElementsViaJs(dateTime);
    }
}
