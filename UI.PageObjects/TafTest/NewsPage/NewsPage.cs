using Taf.UI.Core.Element;
using System.Linq;

namespace Taf.UI.PageObjects
{
    public class NewsPage : BasePage
    {
        private readonly string pageCaption = "//main[@class='news-page']/h1";

        private readonly string deviceNews = "//div[contains(@class,'list-card-item')]/a[contains(@href,'/devices/{0}')]//a";

        private readonly string tabAll = "(//div[contains(@class,'card-header')]//a)[1]";

        public void ClickDeviceNews(string deviceId)
        {
            Element newsItem = new Element(string.Format(deviceNews, deviceId));

            newsItem.Click();
        }

        public string GetDeviceIdFromSearchResultItem(string resultItemText)
        {
            Element resultItem = new Element(string.Format("r", "Devices", resultItemText));

            string deviceId = "";

            if (resultItem.Exists())
            {
                deviceId = resultItem.GetAttribute("href").Split("/").Last();
            }

            return deviceId;
        }

        public void WaitPageLoad()
        {
            WaitPageLoad("News", pageCaption, tabAll);
        }
    }
}
