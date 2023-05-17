using OpenQA.Selenium;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Element;
using Taf.UI.Core.Exceptions;
using Taf.UI.Core.Models;
using System.Collections.Generic;

namespace Taf.UI.PageObjects
{
    public class DevicePage : BasePage
    {
        private readonly string breadcrumbLinks = "//ol[@class='breadcrumb']//li/a";

        private readonly string breadcrumbActive = "//ol[@class='breadcrumb']//li/span"; //device model

        private readonly string deviceName = "//section/div[@class='ml-3']/h1";

        private readonly string deviceModel = "//div[@class='device-page']/div[contains(@class,'card')]//h2";

        private readonly string specificDeviceName = "//section/div[@class='ml-3']/h1[contains(text(),'{0}')]";

        private readonly string specificDeviceNameRedesign = "//div[@class='device-page']/div[contains(@class,'card')]//h2[contains(text(),'{0}')]";

        private readonly string deviceImage = "//section[contains(@class,'d-flex')]/img";

        private readonly string deviceImageRedesign = "//div[@class='device-page']/div[contains(@class,'card')]//img";
        
        private readonly string deviceOsSelectors = "//div[contains(@class,'btn-group')]/a";

        private readonly string deviceOsSelectorByName = "//div[contains(@class,'btn-group')]/a[contains(text(),'{0}')]";

        private readonly string mostViewedContent = "//div[@class='col-8']//div[contains(@class,'list-group')]/a";

        private readonly string panelButton = "//div[@class='col-4']//div[@class='{0}']";

        private readonly string helpTopicLink = "//div[contains(@class,'topics-wrap')]//a";

        private bool isRedesign;
        //private string helpTopicName = "//div[contains(@class,'topics-wrap')]//a/h4";

        public DevicePage(bool isRedesign = false)
        {
            this.isRedesign = isRedesign;
        }

        public string GetDeviceName()
        {
            Element deviceNameElem = new Element(deviceName);

            return deviceNameElem.Text;
        }

        public string GetDeviceModel() => new Element(deviceModel).Text;

        public bool IsPanelButtonPresent(string buttonId)
        {
            Element buttonElement = new Element(string.Format(panelButton, buttonId));

            return buttonElement.Exists();
        }

        public bool IsDeviceImageDisplayed() => new Element(deviceImage).IsImageVisible();

        public List<OsSelectorInfo> GetOsSelectors()
        {
            Element osSelector = new Element(deviceOsSelectors);

            List<OsSelectorInfo> selectors = new List<OsSelectorInfo>();

            if (osSelector.Exists())
            {
                List<IWebElement> foundSelectors = osSelector.FindElements();

                foreach (var foundSelector in foundSelectors)
                {
                    OsSelectorInfo osSelectorInfo = new OsSelectorInfo()
                    {
                        Os = foundSelector.Text,
                        DeviceLink = foundSelector.GetAttribute("href"),
                        IsActive = foundSelector.GetAttribute("class").Contains("nuxt-link-active")
                    };

                    selectors.Add(osSelectorInfo);
                }
            }

            return selectors;
        }

        public DeviceBreadcrumbs GetBreadcrumbs()
        {
            DeviceBreadcrumbs deviceBreadcrumbs = new DeviceBreadcrumbs();

            Element breadcrumbs = new Element(breadcrumbLinks);

            if (breadcrumbs.Exists())
            {
                List<IWebElement> links = breadcrumbs.FindElements();

                deviceBreadcrumbs.AllDevicesLink = links[0].GetAttribute("href");

                if (links.Count > 1)
                {
                    deviceBreadcrumbs.ManufacturerLink = links[1].GetAttribute("href");

                    deviceBreadcrumbs.ManufacturerName = links[1].Text;
                }
            }

            Element deviceModel = new Element(breadcrumbActive);

            if (deviceModel.Exists())
            {
                deviceBreadcrumbs.DeviceModel = deviceModel.Text;
            }

            return deviceBreadcrumbs;
        }

        public List<MostViewedContentItem> GetMostViewedContent()
        {
            List<MostViewedContentItem> contentItems = new List<MostViewedContentItem>();

            Element mostViewedContentItems = new Element(mostViewedContent);

            if (mostViewedContentItems.Exists())
            {
                List<IWebElement> contentElements = mostViewedContentItems.FindElements();

                foreach (var element in contentElements)
                {
                    MostViewedContentItem item = new MostViewedContentItem()
                    {
                        Name = element.Text,
                        Url = element.GetAttribute("href")
                    };
                    
                    contentItems.Add(item);
                }
            }

            return contentItems;
        }

        public List<TopicItem> GetHelpTopics()
        {
            List<TopicItem> topicItems = new List<TopicItem>();

            Element helpTopicItems = new Element(helpTopicLink);

            if (helpTopicItems.Exists())
            {
                List<IWebElement> topicElements = helpTopicItems.FindElements();

                foreach (var element in topicElements)
                {
                    TopicItem item = new TopicItem()
                    {
                        Name = element.Text,
                        Url = element.GetAttribute("href")
                    };

                    if (item.Url.EndsWith("9999999"))
                    {
                        item.Name = "Troubleshooting";
                    }

                    topicItems.Add(item);
                }
            }

            return topicItems;
        }

        public void SelectOsSelector(string os) => new Element(string.Format(deviceOsSelectorByName, os)).ClickIfExists();

        public void WaitSpecificDevicePageLoad(string deviceModel)
        {
            string deviceName = string.Format(isRedesign ? specificDeviceNameRedesign : specificDeviceName, deviceModel);

            if (!ElementsDisplayed(deviceName, isRedesign? deviceImageRedesign : deviceImage))
            {
                throw new PageNotLoadedException(
                    $"'{deviceModel}' device page not loaded within {WaitConstants.CheckElementExistInSec} s!");
            }
        }

        public void WaitPageLoad(string deviceModel)
        {
            if (!ElementsDisplayed(deviceName, deviceImage))
            {
                throw new PageNotLoadedException(
                    $"'{deviceModel}' device page not loaded within {WaitConstants.CheckElementExistInSec} s!");
            }
        }

        public bool IsDevicePageLoaded(string deviceModel)
        {
            string deviceName = string.Format(isRedesign ? specificDeviceNameRedesign : specificDeviceName, deviceModel);

            string image = isRedesign ? deviceImageRedesign : deviceImage;

            return ElementsDisplayed(deviceName, image);
        }
    }
}
