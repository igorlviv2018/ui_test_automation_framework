using Taf.UI.Core.Element;

namespace Taf.UI.PageObjects.CommonPages.Authoring
{
    public class SearchBlock
    {
        private readonly string searchInput = "//div[contains(@class,'search-wrap')]/input";

        private readonly string clearSearchInputButton = "//div[contains(@class,'search-wrap')]/a[contains(@class,'icon-cross')]";

        public void SearchItems(string searchText) => new Element(searchInput).SetText(searchText);

        public void ClickCrossButtonInSearchInput() => new Element(clearSearchInputButton).ClickIfExists();

        public bool IsClearSearchButtonVisible() => new Element(clearSearchInputButton).Exists();

        public bool IsSearchInputVisible() => new Element(searchInput).IsDisplayed();
    }
}
