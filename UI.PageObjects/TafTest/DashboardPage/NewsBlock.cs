using Taf.UI.Core.Element;
using System.Collections.Generic;

namespace Taf.UI.PageObjects.Taf.DashboardPage
{
    public class NewsBlock : DashboardBlock
    {
        private const string newsCaption = "//h5";

        private const string newsRow = "//div[@class='news-item']";

        private const string itemTitleCaption = "//div[@class='news-item']//p[1]";

        private readonly string releaseNotes = "/p[@title]";

        private readonly string newsByItemTitle = blockItem + "//p[contains(@class,'sp-base-text') and starts-with(text(),'{1}')]/..";

        private string NewsByItemTitleXpath(string itemTitle) => string.Format(newsByItemTitle, "News", itemTitle);

        public string GetNewsCaption(string itemTitle) => new Element(NewsByItemTitleXpath(itemTitle) + newsCaption).Text;

        public string GetReleaseNotes(string itemTitle) => new Element(NewsByItemTitleXpath(itemTitle) + releaseNotes).Text;

        public bool IsNewsPresent(string itemTitle) => new Element(NewsByItemTitleXpath(itemTitle)).Exists();

        public List<string> GetNewsCaptionColumn() => GetTextOfElementsViaJs(newsCaption);

        public List<string> GetItemTitleColumn() => GetTextOfElementsViaJs(itemTitleCaption);

        public string GetReleaseNotes(int newsPosition) => new Element(IndexedXpath(newsRow + releaseNotes, newsPosition)).Text;

        public int GetRowCount() => new Element(newsRow).Count;
    }
}
