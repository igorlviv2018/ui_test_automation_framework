using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Taf.UI.PageObjects
{
    public class DeviceSelectorPage : BasePage
    {
        private readonly Dictionary<string, string> locators;

        private readonly Dictionary<string, string> locatorsCommon =

            new Dictionary<string, string>()
            {
                //{ "device search dialog caption", "//div[@class='modal-body']//h1[contains(text(),'Please choose a device')]"}
            };

        private readonly Dictionary<string, string> locatorsTaf =

            new Dictionary<string, string>()
            {
                { "device search dialog caption", "//div[@class='modal-body']//h1[contains(text(),'Please choose a device')]"},
                { "device search input", "//div[@class='modal-body']//div[@class='search-input-wrap']//input[@type='text']"},
                { "search results", "//div[@class='devices']//li/a[@class='nav-link']"},
                { "search results item", "//div[@class='devices']//a[contains(text(),'{0}')]"},
                { "close button", "//div[contains(@class, 'modal-dialog')]//a[@class='close-modal']"}
            };

        private readonly Dictionary<string, string> locatorsEmbed =

            new Dictionary<string, string>()
            {
                { "device search dialog caption", "//div[@class='modal-body']//h1[contains(text(),'Please choose a device')]"},
                { "device search input", "//div[@class='modal-body']//div[@class='search-input-wrap']//input[contains(@class,'search-input')]"},
                { "search results", "//ul[@class='search-results-list']//li/span[@class='device-name']"},
                { "search results item", "//ul[@class='search-results-list']//li/span[@class='device-name' and contains(text(),'{0}')]"},
                { "close button", "//div[contains(@class,'modal-dialog')]//h5/../button[@class='close']"}
            };
        
        private readonly Dictionary<string, string> locatorsTafRedesign =

            new Dictionary<string, string>()
            {
                { "device search dialog caption", "//div[contains(@id,'dialog-panel')]//h3[contains(text(),'Please choose a device')]"},
                { "device search input", "//div[contains(@id,'dialog-panel')]//div[contains(@class,'search')]//input"},
                { "search results", "//div[@class='results']//ul/li"},
                { "search results item", "//div[@class='results']//ul/li/button[contains(text(),'{0}')]"},
                { "close button", "//div[contains(@id,'dialog-panel')]//button[contains(@class,'close-button')]"}
            };

        private readonly Spinner spinner;

        public DeviceSelectorPage(App app, bool isRedesign=false)
        {
            //locators = SetLocators(app, locatorsCommon, locatorsEmbed, locatorsTaf);

            locators = SetLocators(app, locatorsCommon, locatorsEmbed, locatorsTaf,
               agentsRedesign: locatorsTafRedesign, isRedesign: isRedesign);

            spinner = new Spinner(app, isRedesign);
        }

        public bool IsDisplayed() => ElementsDisplayed(GetXpath("device search dialog caption", locators));

        public void SearchDevices(string searchText)
        {
            new Element(GetXpath("device search input", locators)).SetText(searchText);

            spinner.WaitSpinnerToAppear(SpinnerType.SearchIndicator);

            spinner.WaitSpinnerToDisappear(SpinnerType.SearchIndicator);
        }

        public string ChooseDevice(string deviceName)
        {
            string err = string.Empty;

            Element device = new Element(string.Format(GetXpath("search results item", locators), deviceName));

            if (device.Exists())
            {
                device.Click();
            }
            else
            {
                err = $"'{deviceName}' not found in the device search results ('Choose a device' dialog)";
            }

            return err;
        }

        public List<string> GetFoundDevices() => GetTextOfElements(GetXpath("search results", locators));

        public string CheckExpectedDeviceFound(string expectedDeviceName, List<string> searchResults)
        {
            bool found = searchResults.Any(r => r.ToLower() == expectedDeviceName.ToLower());

            return found ? string.Empty : $"'{expectedDeviceName}' device not found in search results";
        }

        public void Close() => new Element(GetXpath("close button", locators)).Click();
    }
}