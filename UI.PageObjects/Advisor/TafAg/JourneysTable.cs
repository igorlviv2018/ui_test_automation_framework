using Taf.UI.Core.Element;
using Taf.UI.Core.Interfaces;
using Taf.UI.PageObjects.CommonPages.Authoring;
using System.Collections.Generic;

namespace Taf.UI.PageObjects.TafTest.Taf
{
    public class JourneysTable : BaseTable, ITafItemsTable
    {
        private readonly string journeyTitleByText = "//div[contains(@class,'list-block')]//div[@class='title' and starts-with(text(),'{0}')]";

        private readonly string journeyTitle = "//div[contains(@class,'list-block')]//div[@class='title']";

        private readonly string tableColumnName = "//th[@role='columnheader']";

        private readonly string articleCheckbox = "//td[1]//input[@type='checkbox']";

        private readonly string articleType = "//td[2]/div[contains(@class,'icon')]";

        private readonly string articleDescription = "//td[3]//div[contains(@class,'description')]";

        private readonly string articleVersion = "//td[5]/div";

        private readonly string articleOwner = "//td[6]";

        public void ClickItemTitle(string title)
        {
            Element titleElement = new Element(string.Format(journeyTitleByText, title));

            titleElement.ClickIfExists();
        }

        public bool IsItemPresent(string title) => IsItemPresent(title, journeyTitleByText); //new Element(string.Format(journeyTitleByText, title)).Exists();

        public List<string> GetTitleColumn() => GetTextOfElementsViaJs(journeyTitle);

        public List<string> GetArticleDescriptionColumn() => GetTextOfElementsViaJs(articleDescription);

        //public List<string> GetArticleIdColumn() => GetTextOfElementsViaJs(articleId);

        //public List<string> GetArticleStatusColumn() => GetTextOfElementsViaJs(articleStatus);

        //public List<string> GetArticleVersionColumn() => GetTextOfElementsViaJs(articleVersion);

        //public List<string> GetArticleOwnerColumn() => GetTextOfElementsViaJs(articleOwner);
    }
}
