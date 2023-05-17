//using Taf.Core.Models.Devices;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models;
using Taf.UI.Core.Models.Taf;
using Taf.UI.PageObjects;
using Taf.UI.PageObjects.Taf;
using Taf.UI.PageObjects.Taf.DashboardPage;
using System.Collections.Generic;

namespace Taf.UI.Steps
{
    public class DashboardSteps// : BaseSteps
    {
        private readonly MostPopularDevicesPage popularDevicesPage = new MostPopularDevicesPage();

        private readonly Sidebar sideMenu = new Sidebar();

        private readonly DevicePage devicePage = new DevicePage();

        private readonly NewsBlock newsBlock = new NewsBlock();

        //private readonly UiDeviceHelper deviceHelper = new UiDeviceHelper();

        public string SelectRandomPopularDevice()
        {
            sideMenu.ClickMenuItem("Home");

            popularDevicesPage.WaitPageLoad();

            string deviceId = popularDevicesPage.ClickRandomDeviceById();

            devicePage.WaitPageLoad($"Random device (id: {deviceId})");

            return deviceId;
        }

        // to rewrite and debug today
        public List<NewsTableRow> ReadNewsBlock()
        {
            List<NewsTableRow> rows = new List<NewsTableRow>();

            int rowsCount = newsBlock.GetRowCount();

            if (rowsCount == 0)
            {
                return rows;
            }

            List<string> newsCaptions = newsBlock.GetNewsCaptionColumn();

            List<string> itemTitles = newsBlock.GetItemTitleColumn();

            //List<string> newsDates = newsTable.GetDateColumn(); //to extend

            for (int i = 0; i < rowsCount; i++)
            {
                NewsTableRow row = new NewsTableRow()
                {
                    NewsCaption = newsCaptions[i],
                    ItemTitle = itemTitles[i],
                    ReleaseNotes = newsBlock.GetReleaseNotes(i + 1)
                };

                rows.Add(row);
            }

            return rows;
        }

        //    public string CheckPopularDevices()
        //    {
        //        sideMenu.ClickMenuItem("Home");

        //        popularDevicesPage.WaitPageLoad();

        //        PopularDevices popularDevices = popularDevicesPage.GetDevices();

        //        List<MasterDevice> actualDevices = popularDevices.Devices;

        //        List<MasterDevice> expectedDevices = deviceHelper.GetExpectedPopularDevices();
        //        //expectedDevices[0].Manufacturer += "*";

        //        string err = DataHelper.CompareObjects(actualDevices, expectedDevices);

        //        if (popularDevices.BrokenImageDevices.Count > 0)
        //        {
        //            string brokenImageDevices = ErrorHelper.ConvertErrorsToString(popularDevices.BrokenImageDevices);

        //            err = $"Some popular devices has a broken image: {brokenImageDevices};{err}";
        //        }

        //        return string.IsNullOrEmpty(err) 
        //            ? string.Empty 
        //            : $"Popular Devices (on Dashboards page) check failed: {err}";
        //    }
    }
}
