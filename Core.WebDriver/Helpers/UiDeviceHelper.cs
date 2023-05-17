using Taf.Api.Tests.Core.Enums;
using Taf.Api.Tests.Helpers;
using Taf.Api.Tests.Models;
using Taf.Core.Models.Devices;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Exceptions;
using Taf.UI.Core.Models;
using Taf.UI.Core.Models.Taf;
using System.Collections.Generic;
using System.Linq;
using DeviceType = Taf.Api.Tests.Core.Enums.DeviceType;

namespace Taf.UI.Core.Helpers
{
    public class UiDeviceHelper
    {
        private readonly DeviceHelper deviceHelper;

        private readonly BrowserStorageHelper browserStorage;

        private readonly string testEnvUrl;

        private readonly bool isRedesign;

        private List<MasterDevice> ExpectedDevicesOnDevicesPage { get; set; } = null;

        public UiDeviceHelper(bool isRedesign=false)
        {
            deviceHelper = new DeviceHelper();

            browserStorage = new BrowserStorageHelper();

            testEnvUrl = new SecretsHelper().GetTestEnvUrl(App.Taf);// UiTestEnvUrl();

            this.isRedesign = isRedesign;
        }

        public List<MasterDevice> GetExpectedDevicesOnDevicesPage()
        {
            if (ExpectedDevicesOnDevicesPage == null)
            {
                string authToken = GetAuthTokenFromBrowserStorage();

                ExpectedDevicesOnDevicesPage = deviceHelper.DevicesOnUi(authToken).ToList();
            }

            return ExpectedDevicesOnDevicesPage;
        }

        public List<MasterDevice> GetExpectedDevicesOnDevicesPage(DeviceType? deviceType, string brand="All brands")
        {
            List<MasterDevice> devices = GetExpectedDevicesOnDevicesPage();

            if (deviceType != null)
            {
                devices = devices.Where(d => d.Type == deviceType).ToList();
            }

            if (brand != "All brands")
            {
                devices = devices.Where(d => d.Manufacturer == brand).ToList();
            }

            return devices;
        }

        public List<string> GetExpectedDeviceNamesOnDevicesPage() =>
            GetExpectedDevicesOnDevicesPage().Select(m => m.Model).ToList();

        public List<string> GetExpectedManufacturers()
        {
            List<MasterDevice> devices = GetExpectedDevicesOnDevicesPage();

            return devices.Select(d => d.Manufacturer).Distinct().OrderBy(m => m).ToList();
        }

        public List<string> GetExpectedDeviceTypesAsString()
        {
            List<MasterDevice> devices = GetExpectedDevicesOnDevicesPage();

            return devices.Select(d => d.Type.ToString()).Distinct().OrderBy(t => t).ToList();
        }

        public List<DeviceType> GetExpectedDeviceTypes()
        {
            List<MasterDevice> devices = GetExpectedDevicesOnDevicesPage();

            return devices.Select(d => d.Type).Distinct().OrderBy(t => t).ToList();
        }

        public List<string> GetExpectedDevicesPageTabs()
        {
            List<MasterDevice> expectedDevices = GetExpectedDevicesOnDevicesPage();

            List<string> types = new List<string>();

            for (int i = 0; i < expectedDevices.Count; i++)
            {
                string type = DataHelper.GetPluralDeviceType(expectedDevices[i].Type.ToString());

                if (!types.Contains(type))
                {
                    types.Add(type);
                }
            }

            return types;
        }

        public MasterDevice GetDeviceByIdViaApi(string id)
        {
            string authToken = GetAuthTokenFromBrowserStorage();

            return deviceHelper.GetDeviceById(id, authToken);
        }

        public string GetDeviceModelsByIds(List<string> ids, bool reverseIdsList = true)
        {
            if (reverseIdsList)
            {
                ids.Reverse();
            }

            List<string> deviceModelAndId = ids.Select(id => GetDeviceByIdViaApi(id).Model + " (id: " + id + ")").ToList();

            return string.Join(", ", deviceModelAndId);
        }

        //use only this method and remove other similar methods
        public string GetRandomExpectedDeviceId(UiDeviceLocation deviceLocation, DeviceFilter filter = null)
        { 
            List<string> randomIds = GetRandomExpectedDeviceIds(deviceLocation, 1, filter);

            return randomIds.Count > 0 ? randomIds[0] : string.Empty;
        }
        
        //{
        //    if (filter == null)
        //    {
        //        filter = new DeviceFilter();
        //    }

        //    List<string> deviceIds = new List<string>();

        //    string authToken = GetAuthTokenFromBrowserStorage();

        //    if (deviceLocation == UiDeviceLocation.NewsUi)
        //    {
        //        deviceIds = deviceHelper.GetDeviceIdsFromNews(authToken);
        //    }
        //    else
        //    {
        //        deviceIds = deviceHelper.GetFilteredDevicesOnUi(deviceLocation, filter, authToken)
        //            .Select(d => d.Id)
        //            .ToList();
        //    }

        //    string randomId = "";

        //    if (deviceIds.Count > 0)
        //    {
        //        randomId = DataHelper.GetRandomElement(deviceIds);
        //    }

        //    return randomId;
        //}

        public List<string> GetRandomExpectedDeviceIds(UiDeviceLocation deviceLocation, int randomDevicesCount, DeviceFilter filter = null)
        {
            if (filter == null)
            {
                filter = new DeviceFilter();
            }

            List<string> deviceIds = new List<string>();

            string authToken = GetAuthTokenFromBrowserStorage();

            if (deviceLocation == UiDeviceLocation.NewsUi)
            {
                deviceIds = deviceHelper.GetDeviceIdsFromNews(authToken);
            }
            else
            {
                deviceIds = deviceHelper.GetFilteredDevicesOnUi(deviceLocation, filter, authToken)
                    .Select(d => d.Id)
                    .ToList();
            }

            List<string> randomIds = DataHelper.GetRandomElements(deviceIds, randomDevicesCount);

            return randomIds;
        }

        public List<MasterDevice> GetRandomExpectedDevices(int randomDevicesCount, DeviceFilter filter = null)
        {
            if (filter == null)
            {
                filter = new DeviceFilter();
            }

            List<MasterDevice> devices = new List<MasterDevice>();

            string authToken = GetAuthTokenFromBrowserStorage();

            
            devices = deviceHelper.GetFilteredDevicesOnUi(UiDeviceLocation.AllDevicesPageUi, filter, authToken)
                .ToList();

            List<MasterDevice> randomDevices = DataHelper.GetRandomElements(devices, randomDevicesCount);

            return randomDevices;
        }

        public string GetRandomDeviceName()
        {
            List<string> expectedDeviceNamesOnDevicesPage = GetExpectedDeviceNamesOnDevicesPage();

            string randomName = expectedDeviceNamesOnDevicesPage.Count > 0
                ? DataHelper.GetRandomElement(expectedDeviceNamesOnDevicesPage)
                : string.Empty;

            return randomName;
        }

        public string GetRandomDeviceBrand()
        {
            List<string> brands = new List<string>() { "samsung", "sony", "apple", "oppo", "xiaomi" };

            return DataHelper.GetRandomElement(brands);
        }

        public string GetExpectedDeviceName(string deviceId)
        {
            MasterDevice device = GetDeviceByIdViaApi(deviceId);

            return $"{device.Manufacturer} {device.Model}";
        }

        public List<Module> GetExpectedDeviceModules(string deviceId)
        {
            MasterDevice device = GetDeviceByIdViaApi(deviceId);

            return device.Modules.ToList();
        }

        public List<MostViewedContentItem> GetExpectedMostViewedContent(string deviceId)
        {
            string authToken = GetAuthTokenFromBrowserStorage();

            DeviceDashboardsGetResponse response = deviceHelper.GetDeviceDashboards(deviceId, authToken);

            if (response.Errors.Count > 0)
            {
                throw new ApiHelperException(string.Join(", ", response.Errors));
            }

            List<MostViewedContentItem> mostViewedContentItems = new List<MostViewedContentItem>();

            if (response.Errors.Count == 0 && response.DeviceDashboards.MostViewedContent != null)
            {
                foreach (var item in response.DeviceDashboards.MostViewedContent)
                {
                    string contentType = item.Type == "Guide" ? "guides" : "problems";

                    string expectedUrl = $"{testEnvUrl}/devices/{deviceId}/topics/{item.TopicId}/{contentType}/{item.CmtReferenceId}";

                    mostViewedContentItems.Add(new MostViewedContentItem() { Name = item.Name, Url = expectedUrl });
                }
            }

            return mostViewedContentItems;
        }

        public List<TopicItem> GetExpectedContentTopics(string deviceId)
        {
            string authToken = GetAuthTokenFromBrowserStorage();

            DeviceDashboardsGetResponse response = deviceHelper.GetDeviceDashboards(deviceId, authToken);

            List<TopicItem> topicItems = new List<TopicItem>();

            if (response.Errors.Count == 0 && response.DeviceDashboards.Topics != null)
            {
                foreach (var item in response.DeviceDashboards.Topics)
                {
                    string expectedUrl = $"{testEnvUrl}/devices/{deviceId}/topics/{item.Id}";

                    topicItems.Add(new TopicItem() { Name = item.Name, Url = expectedUrl });
                }
            }

            return topicItems;
        }

        public List<OsSelectorInfo> GetExpectedOsSelectors(string deviceId)
        {
            string authToken = GetAuthTokenFromBrowserStorage();

            List<MasterDevice> peerSlaves = deviceHelper.GetSlavesByDeviceId(authToken, deviceId);

            if (peerSlaves.Count == 0)
            {
                peerSlaves.Add(GetDeviceByIdViaApi(deviceId));
            }

            List<OsSelectorInfo> osSelectors = new List<OsSelectorInfo>();

            foreach (var peerSlave in peerSlaves)
            {
                string expectedOsNameVersion = null;

                if (!string.IsNullOrEmpty(peerSlave.OsName) || !string.IsNullOrEmpty(peerSlave.OsVersion))
                {
                    expectedOsNameVersion = $"{peerSlave.OsName} {peerSlave.OsVersion}".Trim();
                }

                if (!string.IsNullOrEmpty(expectedOsNameVersion))
                {
                    OsSelectorInfo selectorInfo = new OsSelectorInfo()
                    {
                        Os = expectedOsNameVersion,
                        DeviceLink = $"/devices/{peerSlave.Id}",
                        IsActive = peerSlave.Id == deviceId
                    };

                    osSelectors.Add(selectorInfo);
                }
            }

            return osSelectors;
        }

        public DeviceBreadcrumbs GetExpectedBreadcrumbs(string deviceId)
        {
            MasterDevice device = GetDeviceByIdViaApi(deviceId);

            DeviceBreadcrumbs breadcrumbs = new DeviceBreadcrumbs();

            if (!string.IsNullOrEmpty(device.Manufacturer))
            {
                breadcrumbs.ManufacturerName = device.Manufacturer;

                breadcrumbs.ManufacturerLink = $"{testEnvUrl}/devices?manufacturer={device.Manufacturer}";
            }

            if (!string.IsNullOrEmpty(device.Model))
            {
                breadcrumbs.DeviceModel = device.Model;
            }

            breadcrumbs.AllDevicesLink = $"{testEnvUrl}/devices";

            return breadcrumbs;
        }

        public List<MasterDevice> GetExpectedPopularDevices()
        {
            string authToken = GetAuthTokenFromBrowserStorage();

            DeviceGetResponse response = deviceHelper.GetPopularDevices(authToken);

            if (response.Errors.Count > 0)
            {
                throw new ApiHelperException(string.Join(", ", response.Errors));
            }

            return response.Devices;
        }

        public string GetOsSelectorsAsString(List<OsSelectorInfo> osSelectors) =>

            string.Join(", ", osSelectors.Select(s => s.Os).ToArray());

        public string GetDeviceFilterAsString(DeviceFilter filter)
        {
            List<string> filterValues = new List<string>();

            if (filter.HasPeerSlaves)
            {
                filterValues.Add("Has peer slaves");
            }

            if (filter.HasSpecs)
            {
                filterValues.Add("Has specs");
            }

            if (filter.HasThreeDModel)
            {
                filterValues.Add("Has 3D model");
            }

            if (filter.HasVirtualDevice)
            {
                filterValues.Add("Has virtual device");
            }

            return string.Join(", ", filterValues);
        }

        public string GetAuthTokenFromBrowserStorage()
        {
            return browserStorage.GetAuthToken(BrowserStorage.Session, isRedesign);
        }

        public string CompareDevices(List<MasterDevice> expectedDevices, List<DeviceOnUi> actualDevices)
        {
            List<string> missingDevices = expectedDevices
                    .Where(d => !actualDevices.Any(a => a.Manufacturer == d.Manufacturer && a.Model == d.Model))
                    .Select(d => $"{d.Manufacturer} {d.Model}").ToList();

            List<string> unexpectedDevices = actualDevices
                    .Where(d => !expectedDevices.Any(a => a.Manufacturer == d.Manufacturer && a.Model == d.Model))
                    .Select(d => $"{d.Manufacturer} {d.Model}").ToList();

            List<string> errors = new List<string>
            {
                ErrorHelper.AddPrefixToError($"{string.Join(", ", missingDevices)}", "Missing devices: "),

                ErrorHelper.AddPrefixToError($"{string.Join(", ", unexpectedDevices)}", "Unexpected devices: ")
            };

            return ErrorHelper.ConvertErrorsToString(errors);
        }
    }
}
