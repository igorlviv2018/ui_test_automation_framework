using NLog;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.PageObjects;

namespace Taf.UI.Steps
{
    public class NavigationBarSteps// : BaseSteps
    {
        private readonly NavigationBar navBar;

        private readonly ILogger log;

        public NavigationBarSteps(ILogger logger)
        {
            log = logger;

            navBar = new NavigationBar();
        }

        public string Find(string searchText, UiContentType resultSection)
        {
            string err = string.Empty;

            navBar.Search(searchText);

            if (navBar.IsSearchResultItemPresent(resultSection, searchText))
            {
                navBar.SelectSearchResultItem(resultSection, searchText);
            }
            else
            {
                navBar.ClearSearchInput();

                err = $"Search item not found: {searchText}, category: {resultSection}";
            }

            LogHelper.LogResult(log, $"Search text: {searchText}, {resultSection} selected", err);

            return err;
        }

        public bool NavigationBarIsPresent() => navBar.NavBarExists();
    }
}
