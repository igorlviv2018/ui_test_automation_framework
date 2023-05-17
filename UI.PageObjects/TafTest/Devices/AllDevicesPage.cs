using Taf.Core.Models.Devices;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Element;
using Taf.UI.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Taf.UI.PageObjects
{
    public class AllDevicesPage : BasePage
    {
        private string pageCaption = "//main[@class='devices-page']/h1";

        private string selectBrandButton = "//main[@class='devices-page']//button[contains(@class,'dropdown-toggle')]";

        private string selectBrandItems = "//main[@class='devices-page']//div[contains(@class, 'dropdown')]//a[@role='menuitem']";

        private string selectedBrand = "//main[@class='devices-page']//button[contains(@class,'dropdown-toggle')]/span";

        private string brandMenuItem = "//main[@class='devices-page']//div[contains(@class, 'dropdown')]//a[@role='menuitem' and contains(text(),'{0}')]";

        private string deviceItem = "//div[contains(@class,'device-item')]";

        private string deviceName = "//div[@class='container-fluid']//*[contains(@class,'device-tile-name')]";

        private string deviceBrand = "//div[@class='container-fluid']//*[contains(@class,'device-tile-manuf')]";

        private string deviceImage = "//div[contains(@class,'device-item')]//img";

        private string deviceByModel = "//div[@class='container-fluid']//h4[contains(@class,'device-tile-name') and contains(text(),'{0}')]";

        private string deviceById = "//a[@class='device-tile' and @href='/devices/{0}']";

        private string deviceNameById = "//a[@class='device-tile' and contains(@href, '/devices/{0}')]//*[contains(@class,'device-tile-name')]";

        private string tabByName = "//ul[contains(@class,'nav-tabs')]//a[contains(text(),'{0}')]";

        private string tab = "//ul[contains(@class,'nav-tabs')]//a";

        private string sortByButton = "//ul[contains(@class, 'sort-dd')]//a[@role='button']"; //to deb

        private string sortByOption = "//ul[contains(@class, 'sort-dd')]//a[@role='menuitem' and text()='{0}']"; //to deb

        public void SelectBrand(string brandName)
        {
            Element brand = new Element(string.Format(brandMenuItem, brandName));

            if (brand.Exists())
            {
                OpenSelectBrandMenu();

                brand.Click();
            }
            else 
            {
                CloseSelectBrandMenu();
            }
        }

        public bool IsBrandMenuOpen()
        {
            Element brandButton = new Element(selectBrandButton);

            return brandButton.GetAttribute("aria-expanded") == "true";
        }

        public List<string> GetBrands()
        {
            OpenSelectBrandMenu();

            List<string> brands = GetTextOfElements(selectBrandItems);

            if (brands.Count > 0)
            {
                brands.RemoveAt(0);
            }

            CloseSelectBrandMenu();

            return brands;
        }

        public List<string> GetTabs() => GetTextOfElements(tab);

        public void SortBy(string option) //to add check if menu is expanded
        {
            List<string> allowedOptions = new List<string>() { "Recent", "Popular", "Alphabetically"};

            //if(option)
            Element sortByBtn = new Element(sortByButton);

            if (sortByBtn.GetAttribute("aria-expanded") == "false")
            {
                sortByBtn.Click();
            }

            //new Element()
        }

        public string CheckPageCaption()
        {
            string pageName = new Element(pageCaption).Text;

            return pageName == "Devices" ? string.Empty : $"Expected page name is 'Devices' but actual is '{pageName}'";
        }

        public void OpenSelectBrandMenu()
        {
            Element selectBrand = new Element(selectBrandButton);

            if (selectBrand.Exists(WaitConstants.CheckElementExistInSec) && (selectBrand.GetAttribute("aria-expanded") == "false"))
            {
                selectBrand.Click();
            }
        }

        public void CloseSelectBrandMenu()
        {
            Element selectBrand = new Element(selectBrandButton);

            if (selectBrand.Exists(WaitConstants.CheckElementExistInSec) && (selectBrand.GetAttribute("aria-expanded") == "true"))
            {
                selectBrand.Click();
            }
        }

        public int GetDeviceCount() => new Element(deviceItem).Count;

        public List<string> GetDeviceNames() => GetTextOfElements(deviceName);

        public List<string> GetDeviceBrands() => GetTextOfElements(deviceBrand);

        public string GetDeviceName(int position) => new Element(IndexedXpath(deviceName, position)).Text;

        public string GetDeviceBrand(int position) => new Element(IndexedXpath(deviceBrand, position)).Text;

        public bool IsDeviceImageDisplayed(int position) => new Element(IndexedXpath(deviceImage, position)).IsImageVisible();

        public void ClickDevice(string deviceName)
        {
            Element device = new Element(string.Format(deviceByModel, deviceName));
            
            device.Click();
        }

        public void ClickTab(string tabName)
        {
            Element tab = new Element(string.Format(tabByName, tabName));

            tab.ClickIfExists();
        }

        public MasterDevice ClickDeviceUsingId(string deviceId)
        {
            Element device = new Element(string.Format(deviceById, deviceId));

            MasterDevice deviceInfo = new UiDeviceHelper().GetDeviceByIdViaApi(deviceId);

            device.Click();

            return deviceInfo;
        }

        public string GetSelectedBrand() => new Element(selectedBrand).Text;

        public void WaitTabActive(string tabName)
        {
            bool isActive = UiWaitHelper.Wait(() => new Element(
                string.Format(tabByName, tabName)).GetAttribute("class").Contains("active"), WaitConstants.PageLoadWaitInSec);

            if (!isActive)
            {
                throw new Exception($"Failed to open '{tabName}' tab on devices page");
            }
        }

        public void WaitPageLoad()
        {
            string tabAll = string.Format(tabByName, "All");

            WaitPageLoad("Devices", pageCaption, tabAll);
        }
    }
}
