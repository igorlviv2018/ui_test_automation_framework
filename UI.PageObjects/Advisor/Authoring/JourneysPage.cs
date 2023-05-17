using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;

namespace Taf.UI.PageObjects.TafTest.Authoring
{
    public class JourneysPage
    {
        private readonly string createJourneyButton = "//main[contains(@class,'advisor')]/button";

        private readonly string searchInput = "//div[contains(@class,'search-wrap')]/input";

        private readonly string clearSearchInputButton = "//div[contains(@class,'search-wrap')]/a[contains(@class,'icon-cross')]";

        public void ClickCreateJourneyButton() => new Element(createJourneyButton).ClickIfExists();

        // to update
        public void SearchArticles(string searchText) => new Element(searchInput).SetText(searchText);

        public void ClickCrossButtonInSearchInput() => new Element(clearSearchInputButton).ClickIfExists();

        public bool IsClearSearchButtonVisible() => new Element(clearSearchInputButton).Exists();
    }
}
