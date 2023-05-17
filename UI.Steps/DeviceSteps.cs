using NLog;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models;
using Taf.UI.PageObjects;
//using Taf.Api.Tests.Models;
//using Taf.Api.Tests.Common.Enums;
//using Taf.Core.Models.Devices;
using Taf.UI.Core.Constants;
//using Speedperform.Services.Entities.Enums;
using Taf.Core.Models.Devices;
using Taf.Api.Tests.Models;
using Taf.Api.Tests.Core.Enums;
using System.Linq;
using Taf.UI.Core.Models.Taf;
using System.Collections.Generic;
using System.Web;
using DeviceType = Taf.Api.Tests.Core.Enums.DeviceType;
using Taf.UI.PageObjects.Taf;

namespace Taf.UI.Steps
{
    public class DeviceSteps : BaseSteps
    {
        private readonly NavigationBar navBar;

        private readonly Sidebar sideMenu;

        private readonly DevicePage devicePage;

        private readonly AllDevicesPage allDevicesPage;

        private readonly DeviceSelectorPage deviceSelectorPopup;

        private readonly UiDeviceHelper uiDeviceHelper;

        private readonly DashboardSteps dashboardSteps;

        private readonly NewsSteps newsSteps;

        private readonly BrowserSteps browserSteps;

        private readonly Spinner spinner;

        private readonly bool isRedesign;

        public DeviceSteps(App app, ILogger logger, bool isRedesign=false) : base(app, logger)
        {
            navBar = new NavigationBar();

            sideMenu = new Sidebar(isRedesign);

            devicePage = new DevicePage(isRedesign);

            allDevicesPage = new AllDevicesPage();

            deviceSelectorPopup = new DeviceSelectorPage(app, isRedesign);

            uiDeviceHelper = new UiDeviceHelper(isRedesign);

            dashboardSteps = new DashboardSteps();

            newsSteps = new NewsSteps(log);

            browserSteps = new BrowserSteps(log);

            spinner = new Spinner(App.Taf);

            this.isRedesign = isRedesign;
        }

        //to use in dx flow steps
        public string SelectRandomDeviceInDeviceSelector()
        {
            if (!deviceSelectorPopup.IsDisplayed())
            {
                return string.Empty;
            }

            string randomDeviceName = string.Empty;

            bool deviceSelected = false;

            for (int i = 0; i < 10; i++)
            {
                string randomBrand = uiDeviceHelper.GetRandomDeviceBrand();

                //LogHelper.LogInfo(log, $"Selected brand '{randomBrand}'");

                deviceSelectorPopup.SearchDevices(randomBrand);

                List<string> searchResults = deviceSelectorPopup.GetFoundDevices();

                if (searchResults.Count > 0)
                {
                    randomDeviceName = DataHelper.GetRandomElement(searchResults, CommonConstants.MaxNumberOfSearchResults);

                    deviceSelectorPopup.ChooseDevice(randomDeviceName);

                    LogHelper.LogInfo(log, $"Selected device '{randomDeviceName}'");

                    deviceSelected = true;

                    break;
                }
            }

            if (!deviceSelected) // close Device selector popup
            {
                deviceSelectorPopup.Close();
            }

            return randomDeviceName;
        }

        public string SearchViaNavBarAndSelectDevice(string deviceName)
        {
            navBar.ClearSearchInputIncludeDevice();

            navBar.Search(deviceName);

            string deviceId = navBar.GetDeviceIdFromSearchResultItem(deviceName);

            navBar.SelectSearchResultItem(UiContentType.Device, deviceName);

            devicePage.WaitSpecificDevicePageLoad(deviceName);

            return deviceId;
        }

        public void SelectDeviceOnAllDevicesPage(string deviceId)
        {
            sideMenu.ClickMenuItem("Devices");

            allDevicesPage.WaitPageLoad();

            MasterDevice deviceInfo = allDevicesPage.ClickDeviceUsingId(deviceId);

            devicePage.WaitSpecificDevicePageLoad(deviceInfo.Model);
        }

        //comment out
        public string SelectRandomDevice(DeviceLocation deviceLocation, DeviceFilter filter = null)
        {
            string randomDeviceId = "";

            filter ??= new DeviceFilter();

            if (deviceLocation == DeviceLocation.DevicesPage)
            {
                randomDeviceId = uiDeviceHelper.GetRandomExpectedDeviceId(UiDeviceLocation.AllDevicesPageUi, filter);

                log.Info($"Device to be opened (location: '{deviceLocation}', id: '{randomDeviceId}')");

                if (!string.IsNullOrEmpty(randomDeviceId))
                {
                    SelectDeviceOnAllDevicesPage(randomDeviceId);
                }
            }

            if (deviceLocation == DeviceLocation.DashboardPopular)
            {
                log.Info($"Device to be opened (location: '{deviceLocation}')");

                randomDeviceId = dashboardSteps.SelectRandomPopularDevice();
            }

            if (deviceLocation == DeviceLocation.SearchResultsAutocomplete)
            {
                string randomDeviceName = uiDeviceHelper.GetRandomDeviceName();

                log.Info($"Device to be opened (location: '{deviceLocation}', name: '{randomDeviceName}')");

                if (!string.IsNullOrEmpty(randomDeviceName))
                {
                    randomDeviceId = SearchViaNavBarAndSelectDevice(randomDeviceName);
                }
            }

            if (deviceLocation == DeviceLocation.NewsPage)
            {
                randomDeviceId = uiDeviceHelper.GetRandomExpectedDeviceId(UiDeviceLocation.NewsUi, filter);

                log.Info($"Device to be opened. Device location: '{deviceLocation}', id: '{randomDeviceId}'");

                if (!string.IsNullOrEmpty(randomDeviceId))
                {
                    newsSteps.OpenDeviceFromNewsPage(randomDeviceId);
                }
            }

            if (deviceLocation == DeviceLocation.RecentDevicesList)
            {
                randomDeviceId = OpenAnyDeviceFromRecentDevicesList();

                log.Info($"Device to be opened. Device location: '{deviceLocation}', id: '{randomDeviceId}'");
            }

            return randomDeviceId;
        }

        public void SelectDeviceUsingDeeplink(string deviceId) => browserSteps.OpenDeepLink(App.Taf, string.Format(LinkConstants.TafDeviceLink, deviceId), isRedesign);

        public string SelectRandomDeviceUsingDeeplink()
        {
            string randomDeviceId = uiDeviceHelper.GetRandomExpectedDeviceId(UiDeviceLocation.AllDevicesPageUi, new DeviceFilter());

            MasterDevice device = uiDeviceHelper.GetDeviceByIdViaApi(randomDeviceId);

            LogHelper.LogInfo(log, $"Random device selected: {device.Manufacturer} {device.Model}, id={randomDeviceId}");

            SelectDeviceUsingDeeplink(randomDeviceId);

            return randomDeviceId;
        }

        public void SelectRandomDevicesUsingDeeplink(int numOfDevices)
        {
            for (int i = 0; i < numOfDevices; i++)
            {
                SelectRandomDeviceUsingDeeplink();
            }
        }

        public MasterDevice GetDeviceById(string deviceId) => uiDeviceHelper.GetDeviceByIdViaApi(deviceId);

        public void OpenDevicesPage()
        {
            sideMenu.ClickMenuItem("Devices");

            allDevicesPage.WaitPageLoad();
        }

        public string OpenAnyDeviceFromRecentDevicesList()
        {
            List<string> recentDeviceIds = sideMenu.GetRecentDeviceIds();

            if (recentDeviceIds.Count > 1)
            {
                recentDeviceIds.RemoveAt(0);
            }

            string randomId = "";

            if (recentDeviceIds.Count > 0)
            {
                randomId = DataHelper.GetRandomElement(recentDeviceIds);

                LogHelper.LogError(log, $"Recent devices list: Device to be opened (id): {randomId}");

                sideMenu.ClickRecentDeviceById(randomId);

                WaitDevicePageLoadById(randomId);
            }

            return randomId;
        }
        //end----
        public string CheckDeviceImage()
        {
            string actualDeviceName = devicePage.GetDeviceName();

            return !devicePage.IsDeviceImageDisplayed()
                ? $"Device image ({actualDeviceName}) is not visible (or broken)"
                : string.Empty;
        }

        //--start
        public string CheckDeviceName(string deviceId)
        {
            string actualDeviceName = devicePage.GetDeviceName();

            string expectedDeviceName = uiDeviceHelper.GetExpectedDeviceName(deviceId);

            return actualDeviceName != expectedDeviceName
                ? $"Invalid device name ('{actualDeviceName}') - expected name: '{expectedDeviceName}'"
                : string.Empty;
        }

        /// <summary>
        /// Check device page button panel(i.e.presence of Specifications, Virtual Device etc buttons)
        /// </summary>
        /// <param name = "deviceId" ></ param >
        /// < returns ></ returns >
        public string CheckButtonPanel(string deviceId)
        {
            List<Module> expectedDeviceModules = uiDeviceHelper.GetExpectedDeviceModules(deviceId);

            Dictionary<Module, string> moduleButtonId = new Dictionary<Module, string>()
            {
                { Module.Specifications, "specs-icon"},
                { Module.VirtualDevice, "vd-icon"},
                { Module.ThreeDModel, "threed-icon"}
            };

            Dictionary<string, string> buttonIdName = new Dictionary<string, string>()
            {
                { "specs-icon", "Specifications"},
                { "vd-icon", "Virtual Device"},
                { "threed-icon", "3D View"}
            };

            List<string> errors = new List<string>();

            foreach (var module in moduleButtonId.Keys)
            {
                string buttonId = moduleButtonId[module];

                if (expectedDeviceModules.Contains(module) && !devicePage.IsPanelButtonPresent(buttonId))
                {
                    ErrorHelper.AddToErrorList(errors,
                        $"'{buttonIdName[buttonId]}' button is not present on the device page");
                }
            }

            return string.Join(", ", errors);
        }

        public string CheckDeviceBreadcrumbs(string deviceId)
        {
            DeviceBreadcrumbs actualBreadcrumbs = devicePage.GetBreadcrumbs();

            // to debug - some manuf breadcrumb
            actualBreadcrumbs.ManufacturerLink = HttpUtility.UrlDecode(actualBreadcrumbs.ManufacturerLink);

            DeviceBreadcrumbs expectedBreadcrumbs = uiDeviceHelper.GetExpectedBreadcrumbs(deviceId);

            string err = DataHelper.CompareObjects(actualBreadcrumbs, expectedBreadcrumbs);

            return !string.IsNullOrEmpty(err)
                ? $"Device breadcrumbs check failed: {err}"
                : string.Empty;
        }

        public string CheckDeviceOsSelectors(string deviceId)
        {
            List<OsSelectorInfo> actualOsSelectors = devicePage.GetOsSelectors();

            List<OsSelectorInfo> expectedOsSelectors = uiDeviceHelper.GetExpectedOsSelectors(deviceId);

            return CompareOsSelectors(actualOsSelectors, expectedOsSelectors);
        }

        public string CheckDeviceMostViewedContent(string deviceId)
        {
            List<MostViewedContentItem> actualContentItems = devicePage.GetMostViewedContent();

            List<MostViewedContentItem> expectedContentItems = uiDeviceHelper.GetExpectedMostViewedContent(deviceId);

            return DataHelper.CompareObjects(actualContentItems, expectedContentItems);
        }

        public string CheckDeviceTopics(string deviceId)
        {
            List<TopicItem> actualTopics = devicePage.GetHelpTopics();

            List<TopicItem> expectedTopics = uiDeviceHelper.GetExpectedContentTopics(deviceId);

            string err = DataHelper.CompareObjects(actualTopics, expectedTopics);

            return !string.IsNullOrEmpty(err)
                ? $"Device help topics check failed: {err}"
                : string.Empty;
        }

        public string SelectAndCheckRandomDeviceFrom(DeviceLocation location, DeviceFilter filter = null)
        {
            string deviceId = SelectRandomDevice(location, filter);

            string deviceExtraInfo = filter != null
                ? uiDeviceHelper.GetDeviceFilterAsString(filter)
                : string.Empty;

            if (string.IsNullOrEmpty(deviceId))
            {
                LogHelper.Log(log, $"No devices in '{location}' found. Additional search criteria info (if any): {deviceExtraInfo}");

                return string.Empty;
            }

            string deviceName = uiDeviceHelper.GetDeviceByIdViaApi(deviceId).Model;

            LogHelper.Log(log, $"Device ('{deviceName}', id='{deviceId}') from '{location}' selected. Additional device info (if any): {deviceExtraInfo}");

            string err = CheckDevicePage(deviceId);

            LogHelper.Log(log, $"Device ('{deviceName}', id='{deviceId}') page in '{location}' checked", err);

            return err;// CheckDevicePage(deviceId);
        }

        public string CheckDevicePage(string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return string.Empty;
            }

            List<string> errors = new List<string>();

            string err = CheckDeviceBreadcrumbs(deviceId);
            ErrorHelper.AddToErrorList(errors, err);

            err = CheckDeviceName(deviceId);
            ErrorHelper.AddToErrorList(errors, err);

            err = CheckDeviceOsSelectors(deviceId);
            ErrorHelper.AddToErrorList(errors, err);

            err = CheckDeviceMostViewedContent(deviceId);
            ErrorHelper.AddToErrorList(errors, err);

            err = CheckDeviceImage();
            ErrorHelper.AddToErrorList(errors, err);

            err = CheckButtonPanel(deviceId); //Specification, 3D Model, Virtual Device buttons
            ErrorHelper.AddToErrorList(errors, err);

            err = CheckDeviceTopics(deviceId);
            ErrorHelper.AddToErrorList(errors, err);

            err = string.Join("; ", errors);

            if (!string.IsNullOrEmpty(err))
            {
                err = $"Device check failed (name='{devicePage.GetDeviceName()}', id='{deviceId}'): {err}";
            }

            return err;
        }

        public string CompareBreadcrumbs(DeviceBreadcrumbs actualBreadcrumbs, DeviceBreadcrumbs expectedBreadcrumbs)
        {
            string err = "";

            if (actualBreadcrumbs.AllDevicesLink != expectedBreadcrumbs.AllDevicesLink)
            {
                err = $"Actual 'devices' link ({actualBreadcrumbs.AllDevicesLink}) not equal to expected ({expectedBreadcrumbs.AllDevicesLink});";
            }

            if (actualBreadcrumbs.ManufacturerName != expectedBreadcrumbs.ManufacturerName)
            {
                err = $"{err} Actual manufacturer ({actualBreadcrumbs.ManufacturerName}) not equal to expected ({expectedBreadcrumbs.ManufacturerName});";
            }

            if (actualBreadcrumbs.ManufacturerLink != expectedBreadcrumbs.ManufacturerLink)
            {
                err = $"{err} Actual manufacturer link ({actualBreadcrumbs.ManufacturerLink}) not equal to expected ({expectedBreadcrumbs.ManufacturerLink});";
            }

            if (actualBreadcrumbs.DeviceModel != expectedBreadcrumbs.DeviceModel)
            {
                err = $"{err} Actual device model ({actualBreadcrumbs.DeviceModel}) not equal to expected ({expectedBreadcrumbs.DeviceModel});";
            }

            return string.IsNullOrEmpty(err) ? err : $"Device breadcrumbs check failed: {err}";
        }

        public string CheckAllDevicesBrands()
        {
            List<string> actualBrands = allDevicesPage.GetBrands();

            List<string> expectedBrands = uiDeviceHelper.GetExpectedManufacturers();

            string err = DataHelper.CompareObjects(actualBrands, expectedBrands);

            return !string.IsNullOrEmpty(err)
                ? $"Select brands dropdown check failed: {err}"
                : string.Empty;
        }

        public string CheckDevicesPageTabs()
        {
            List<string> actualTabs = allDevicesPage.GetTabs();

            List<string> expectedTabs = uiDeviceHelper.GetExpectedDeviceTypes()
                .Select(t => DataHelper.GetPluralDeviceType(t.ToString()))
                .ToList();

            expectedTabs.Add("All");

            string err = DataHelper.CompareListsIgnoreOrder(actualTabs, expectedTabs);

            return ErrorHelper.AddPrefixToError(err, "Device tabs check failed: ");
        }

        public string CheckDevicesInTab(DeviceType? deviceType, string brand)
        {
            int deviceCount = allDevicesPage.GetDeviceCount();

            List<MasterDevice> expectedDevices = uiDeviceHelper.GetExpectedDevicesOnDevicesPage(deviceType, brand);

            List<string> errors = new List<string>();

            string err;

            if (deviceCount != expectedDevices.Count)
            {
                err = $"Actual device count: {deviceCount} (expected: {expectedDevices.Count})";

                errors.Add(err);
            }

            List<DeviceOnUi> actualDevices = GetDevicesOnAllDevicesPage();

            err = uiDeviceHelper.CompareDevices(expectedDevices, actualDevices);

            errors.Add(err);

            List<string> brokenImageDevices = actualDevices.Where(d => !d.IsImageDisplayed).Select(d => $"{d.Manufacturer} {d.Model}").ToList();

            errors.Add(ErrorHelper.AddPrefixToError(string.Join(", ", brokenImageDevices), "Broken image devices: "));

            return ErrorHelper.ConvertErrorsToString(errors);
        }

        public string CheckDevicesInAnyTab(string brand)
        {
            DeviceType tab = OpenRandomTab();

            string err = CheckDevicesInTab(tab, brand);

            return err;
        }

        public List<DeviceOnUi> GetDevicesOnAllDevicesPage()
        {
            int deviceCount = allDevicesPage.GetDeviceCount();

            List<DeviceOnUi> devices = new List<DeviceOnUi>();

            for (int i = 0; i < deviceCount; i++)
            {
                DeviceOnUi device = new DeviceOnUi()
                {
                    Manufacturer = allDevicesPage.GetDeviceBrand(i + 1),
                    Model = allDevicesPage.GetDeviceName(i + 1),
                    IsImageDisplayed = allDevicesPage.IsDeviceImageDisplayed(i + 1)
                };

                devices.Add(device);
            }

            return devices;
        }

        public void SelectBrand(string brand)
        {
            allDevicesPage.SelectBrand(brand);

            UiWaitHelper.Wait(() => !allDevicesPage.IsBrandMenuOpen(), WaitConstants.CheckElementExistInSec);
        }

        public string SelectRandomBrand()
        {
            List<string> actualBrands = allDevicesPage.GetBrands();

            string brand = DataHelper.GetRandomElement(actualBrands);

            SelectBrand(brand);

            return brand;
        }

        public string CheckDevicesInTabs(string selectedBrand="All brands")
        {
            List<DeviceType> types = uiDeviceHelper.GetExpectedDeviceTypes();

            DeviceType type = DataHelper.GetRandomElement(types);

            OpenTab(type);

            List<DeviceOnUi> actualDevices = GetDevicesOnAllDevicesPage();

            List<MasterDevice> expectedDevices = new List<MasterDevice>();

            if (selectedBrand == "All brands")
            {
                expectedDevices = uiDeviceHelper.GetExpectedDevicesOnDevicesPage().Where(d => d.Type == type).ToList();
            }
            else
            {
                expectedDevices = uiDeviceHelper.GetExpectedDevicesOnDevicesPage().Where(d => d.Type == type && d.Manufacturer == selectedBrand).ToList();
            }

            string err = uiDeviceHelper.CompareDevices(expectedDevices, actualDevices);

            return err;
        }

        public void OpenTab(DeviceType tab)
        {
            string tabName = DataHelper.GetPluralDeviceType(tab.ToString());

            allDevicesPage.ClickTab(tabName);

            allDevicesPage.WaitTabActive(tabName);

            LogHelper.LogInfo(log, $"Tab '{tab}' opened");
        }

        public DeviceType OpenRandomTab()
        {
            DeviceType tab = DataHelper.GetRandomElement(uiDeviceHelper.GetExpectedDeviceTypes());

            OpenTab(tab);

            return tab;
        }

        public string CompareOsSelectors(List<OsSelectorInfo> actualSelectors, List<OsSelectorInfo> expectedSelectors)
        {
            string err = "";

            if (actualSelectors.Count != expectedSelectors.Count)
            {
                err = $"Actual OS selector count ({actualSelectors.Count}) not equal to expected count ({expectedSelectors.Count});" +
                      $"{err} Actual OS selectors: {uiDeviceHelper.GetOsSelectorsAsString(actualSelectors)}, " +
                      $"expected: {uiDeviceHelper.GetOsSelectorsAsString(expectedSelectors)}";
            }
            else
            {
                List<string> errors = new List<string>();

                for (int i = 0; i < actualSelectors.Count; i++)
                {
                    if (actualSelectors[i].Os != expectedSelectors[i].Os)
                    {
                        ErrorHelper.AddToErrorList(errors,
                            "Actual OS is { actualSelectors[i].Os }, expected OS: { expectedSelectors[i].Os}");
                    }
                }

                err = string.Join(", ", errors);
            }

            return err;
        }

        public string SelectRandomDeviceOsSelector()
        {
            List<OsSelectorInfo> actualOsSelectors = devicePage.GetOsSelectors();

            List<OsSelectorInfo> unselectedOsSelectors = actualOsSelectors.Where(s => s.IsActive == false).ToList();

            OsSelectorInfo randomOsSelector = DataHelper.GetRandomElement(unselectedOsSelectors) ?? new OsSelectorInfo();

            if (!string.IsNullOrEmpty(randomOsSelector.Os))
            {
                devicePage.SelectOsSelector(randomOsSelector.Os);

                spinner.WaitTopProgressBarToDisappear();
            }

            string deviceId = randomOsSelector.DeviceLink.Split("/").Last();

            return deviceId;
        }

        public void WaitDevicePageLoadById(string deviceId)
        {
            string model = uiDeviceHelper.GetDeviceByIdViaApi(deviceId).Model;

            devicePage.WaitSpecificDevicePageLoad(model);
        }

        public bool IsDevicePageOpened(string model) => devicePage.IsDevicePageLoaded(model);
    }
}

