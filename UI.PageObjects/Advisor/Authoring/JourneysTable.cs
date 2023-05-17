using Taf.UI.Core.Element;
using Taf.UI.Core.Interfaces;
using Taf.UI.PageObjects.CommonPages.Authoring;
using System.Collections.Generic;

namespace Taf.UI.PageObjects.Authoring
{
    public class JourneysTable : BaseTable, IItemsTable
    {
        private readonly string journeyTitleByText = "//td[3]/div/div[contains(@class,'title') and starts-with(normalize-space(text()),'{0}')]";

        private readonly string journeyTitle = "//td[3]/div/div[contains(@class,'title')]";

        private readonly string tableColumnName = "//th[@role='columnheader']";

        private readonly string journeyId = "//td[3]//div[contains(@class,'title')]/span[contains(@class,'secondary-text')]";

        private readonly string itemStatus = "//td[4]/span[contains(@class,'status')]";

        private readonly string journeyOwner = "//td[5]";

        private readonly string journeyOpenPreview = "//td[8]/button/span[@class='ss-view']";

        private string HeaderColumnName(int position) => IndexedXpath(tableColumnName, position);

        public void ClickItemTitle(string title)
        {
            Element titleElement = new Element(string.Format(journeyTitleByText, title));

            titleElement.ClickIfExists();
        }

        public bool IsItemPresent(string title) => new Element(string.Format(journeyTitleByText, title)).Exists();

        public string GetHeaderColumnSorting(int headerColumnPosition) => new Element(HeaderColumnName(headerColumnPosition)).GetAttribute("aria-sort");

        public void ClickHeaderColumnName(int headerColumnPosition) => new Element(HeaderColumnName(headerColumnPosition)).ClickIfExists();

        public List<string> GetTitleColumn() => GetTextOfElementsViaJs(journeyTitle);

        public List<string> GetItemStatusColumn() => GetTextOfElementsViaJs(itemStatus);

        public List<string> GetItemIdColumn() => GetTextOfElementsViaJs(journeyId);

        public List<string> GetOwnerColumn() => GetTextOfElementsViaJs(journeyOwner);

        public void ClickJourneyPreviewButton(int rowPosition) => new Element(IndexedXpath(journeyOpenPreview, rowPosition)).ClickIfExists();
    }
}
