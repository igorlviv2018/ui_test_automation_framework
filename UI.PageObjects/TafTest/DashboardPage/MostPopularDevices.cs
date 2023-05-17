//using Taf.Core.Models.Devices;
using Taf.UI.Core.Element;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models;
using System.Collections.Generic;

namespace Taf.UI.PageObjects
{
    public class MostPopularDevicesPage : BasePage
    {
        private string deviceNames = "//div[@class='device-wrap']//*[contains(@class, 'device-tile-name')]";

        private string deviceIds = "//div[contains(@class,'device-wrap')]/a";

        private string deviceByModel = "//div[@class='device-wrap']//*[contains(@class, 'device-tile-name') and contains(text(),'{0}')]";

        private string deviceById = "//div[@class='device-wrap']/a[@class='device-tile' and @href='/devices/{0}']";

        private string deviceModel = "//div[@class='device-wrap']/a[@href='/devices/{0}']//h4[contains(@class, 'device-tile-name')]";

        private string deviceManufacturer = "//div[@class='device-wrap']/a[@href='/devices/{0}']//div[contains(@class, 'device-tile-manuf')]";

        private string deviceImage = "//div[@class='device-wrap']/a[@href='/devices/{0}']/div[@class='device-tile-img']/img";

        private string mostPopularCaption = "//*[contains(text(), 'Most popular')]";

        private string viewAllDevicesLink = "//div[contains(@class,'panel-footer')]/a[@href='/devices']";

        public List<string> GetDeviceNames()
        {
            return GetTextOfElements(deviceNames);
        }

        public List<string> GetDeviceIds()
        {
            return GetDeviceListIds(deviceIds);
        }

        //public PopularDevices GetDevices()
        //{
        //    List<string> popularDeviceIds = GetDeviceIds();

        //    PopularDevices popularDevices = new PopularDevices();

        //    foreach (var deviceId in popularDeviceIds)
        //    {
        //        string model = new Element(string.Format(deviceModel, deviceId)).Text;

        //        string manuf = new Element(string.Format(deviceManufacturer, deviceId)).Text;

        //        string imageUrl = GetImageSrc(string.Format(deviceImage, deviceId));

        //        // is image displyed
        //        bool isImageVisible = new Element(string.Format(deviceImage, deviceId)).IsImageVisible();

        //        if (!isImageVisible)
        //        {
        //            popularDevices.BrokenImageDevices.Add(model);
        //        }

        //        //remove query string from image url
        //        int queryIndex = imageUrl.IndexOf("?");

        //        if (queryIndex > 0)
        //        {
        //            imageUrl = imageUrl.Substring(0, queryIndex);
        //        }

        //        popularDevices.Devices.Add(
        //            new MasterDevice() 
        //            { 
        //                Id = deviceId,
        //                Model = model,
        //                Manufacturer = manuf,
        //                Image = imageUrl
        //            });
        //    }

        //    return popularDevices;
        //}

        public void ClickDevice(string model)
        {
            ClickElementWithTextIfExists(deviceByModel, model);
        }

        public void ClickDeviceUsingId(string id)
        {
            new Element(string.Format(deviceById, id)).ClickIfExists();
        }

        public string ClickRandomDeviceById()
        {
            List<string> popularDeviceIds = GetDeviceIds();

            if (popularDeviceIds.Count > 0)
            {
                string randomId = DataHelper.GetRandomElement(popularDeviceIds);

                ClickDeviceUsingId(randomId);

                return randomId;
            }

            return string.Empty;
        }

        public void ViewAllDevices()
        {
            new Element(viewAllDevicesLink).ClickIfExists();
        }

        public void WaitPageLoad() => WaitPageLoad("Most popular devices", mostPopularCaption, viewAllDevicesLink);
    }
}
