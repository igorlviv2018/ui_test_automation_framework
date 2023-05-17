using OpenQA.Selenium;
using Taf.UI.Core.Element;
using System.Collections.Generic;
using System.Linq;
using Taf.UI.Core.Enums;

namespace Taf.UI.PageObjects
{
    public class NavigationBar : BasePage
    {
        private readonly string globalSearchInput = "//div[contains(@class,'global-search')]//input[contains(@class,'search')]"; //By.CssSelector("div.global-search input.search-input");

        private readonly string searchResultSections = "//div[@class='result-wrap']//li[@class='result-section-title']";

        private readonly string clearSearchButton = "//div[@class='search-input-wrap']/span[contains(@class,'clear-query')]";

        private readonly string globalSearchSpinner = "//div[@class='search-input-wrap']//span[contains(@class,'spinner-grow')]"; //pulsing black dot

        private const string resultSectionByTitle = "//div[@class='result-wrap']//li[@class='result-section-title' and contains(text(),'{0}')]";

        private readonly string resultItemBySectionAndTitle = resultSectionByTitle + "/../li[@class='result-item']//a[contains(text(),'{1}')]";

        private readonly string selectedDeviceInSearchBar = "//span[contains(@class,'sp-tag')]/span[@class='title']";

        private readonly string deviceInNavBarDismissButton = "//span[contains(@class,'sp-tag')]/span[@class='dismiss-button']";

        //private string searchResultSectionTitle = searchResultSection + "/li[@class='result-section-title']";

        private readonly string profileMenuButton = "//li[contains(@class, 'profile-menu')]";

        private readonly string speedperformLogo = "//div[contains(@class,'navbar')]//a[contains(@class,'navbar-brand')]";

        private readonly string speedperformLogoRedesign = "//div[contains(@class,'navbar')]//a";

        public bool IsLogoPresent(bool isRedesign=false) => ElementsPresent(isRedesign ? speedperformLogoRedesign : speedperformLogo);

        public void Search(string searchText)
        {
            new Element(globalSearchInput).SetText(searchText);

            Spinner.WaitSpinnerToDisappear("Global search spinner", globalSearchSpinner);
        }

        public void SelectSearchResultItem(UiContentType resultSection, string resultItemText)
        {
            Element resultItem = new Element(string.Format(resultItemBySectionAndTitle, resultSection, resultItemText));

            resultItem.ClickIfExists();
        }

        public bool IsSearchResultItemPresent(UiContentType resultSection, string resultItemText) =>
            new Element(string.Format(resultItemBySectionAndTitle, resultSection, resultItemText)).Exists();

        public string GetDeviceIdFromSearchResultItem(string resultItemText)
        {
            Element resultItem = new Element(string.Format(resultItemBySectionAndTitle, "Devices", resultItemText));

            return resultItem.GetAttribute("href").Split("/").Last();
        }

        //debug
        public List<string> ResultSections()
        {
            Element sections = new Element(searchResultSections);

            return sections.FindElements().Select(item => item.Text).ToList();
        }

        public void ClearSearchInput()
        {
            Element clearSearchInput = new Element(clearSearchButton);
            
            clearSearchInput.ClickIfExists();
        }

        /// <summary>
        /// Get selected device name (in global search bar)
        /// </summary>
        /// <returns>device name or empty string if no device is in the search bar</returns>
        public string GetDeviceInSearchBar()
        {
            Element selectedDevice = new Element(selectedDeviceInSearchBar);

            return selectedDevice.Exists() ? selectedDevice.Text : string.Empty;
        }

        /// <summary>
        /// Clear search input and current device
        /// </summary>
        public void ClearSearchInputIncludeDevice()
        {
            ClearSearchInput();

            ClearDeviceInSearch();
        }

        /// <summary>
        /// Clear device in global search bar
        /// </summary>
        /// <param name="useBackspace">
        /// if true - clear device by pressing 'Backspace' key otherwise clear by clicking a 'Dismiss' button (on a device rectangle)
        /// </param>
        /// <returns></returns>
        public string ClearDeviceInSearch(bool useBackspace = false)
        {
            if (useBackspace)
            {
                Element searchInput = new Element(globalSearchInput);

                searchInput.SetText("" + Keys.Backspace + Keys.Backspace);
            }
            else 
            {
                Element searchBarDeviceDismissButton = new Element(deviceInNavBarDismissButton);

                if (searchBarDeviceDismissButton.Exists())
                {
                    searchBarDeviceDismissButton.Click();
                }
            }

            return "send backspace";
        }

        public bool NavBarExists()
        {
            return ElementsPresent(globalSearchInput, profileMenuButton);
        }
    }
}
