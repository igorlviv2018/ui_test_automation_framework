using NLog;
using Taf.Api.Tests.Core.Enums;
using Taf.Core.Models.Devices;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.PageObjects;
using Taf.UI.PageObjects.Taf;
using Taf.UI.Steps.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Taf.UI.Steps
{
    public class SidebarRecentDevicesSteps : SidebarBaseSteps
    {
        private readonly DeviceSteps deviceSteps;

        private readonly Sidebar sidebar;

        private readonly Random random = new Random();

        private readonly UiDeviceHelper uiDeviceHelper;

        private readonly Spinner spinner = new Spinner(App.Taf);

        private Queue<string> ExpectedRecentDeviceIds = new Queue<string>();

        private readonly bool isRedesign;

        public SidebarRecentDevicesSteps(ILogger logger, bool isRedesign=false) : base(logger)
        {
            deviceSteps = new DeviceSteps(App.Taf, logger, isRedesign);

            sidebar = new Sidebar(isRedesign);

            uiDeviceHelper = new UiDeviceHelper(isRedesign);

            this.isRedesign = isRedesign;
        }

        public void SelectRandomDeviceAndCheckRecentDevicesList(DeviceLocation deviceLocation, int recentDevicesListLength)
        {
            string randomDeviceId = deviceSteps.SelectRandomDevice(deviceLocation);

            log.Info($"Device selected. Location: '{deviceLocation}', device id: '{randomDeviceId}'");

            sidebar.WaitRecentDeviceItemLoading(randomDeviceId);

            AddToExpectedRecentDeviceIds(randomDeviceId, recentDevicesListLength);

            string err = ValidateRecentDevicesList();

            if (!string.IsNullOrEmpty(err))
            {
                throw new Exception(err);
            }
        }

        public void SelectRandomDeviceUsingDeeplinkAndCheckRecentDevicesList(string randomDeviceId, int recentDevicesListLength)
        {
            deviceSteps.SelectDeviceUsingDeeplink(randomDeviceId);

            log.Info($"Device selected. Device id: '{randomDeviceId}'");

            sidebar.WaitRecentItemLoading(randomDeviceId);

            AddToExpectedRecentDeviceIds(randomDeviceId, recentDevicesListLength);

            string err = ValidateRecentDevicesList();

            if (!string.IsNullOrEmpty(err))
            {
                throw new Exception(err);
            }
        }

        public void SelectRandomDeviceUsingDeeplink(int recentDevicesListLength)
        {
            string randomDeviceId = deviceSteps.SelectRandomDeviceUsingDeeplink();

            AddToExpectedRecentDeviceIds(randomDeviceId, recentDevicesListLength);
        }

        public void SelectRandomDevicesUsingDeeplink(int deviceCount, int recentDevicesListLength)
        {
            for (int i = 0; i < deviceCount; i++)
            {
                SelectRandomDeviceUsingDeeplink(recentDevicesListLength);
            }
        }

        //todo - refactor
        //public void SelectDevicesFromMultipleLocationsAndCheckRecentDevicesList()
        //{
        //    foreach (DeviceLocation location in Enum.GetValues(typeof(DeviceLocation)))
        //    {
        //        SelectRandomDeviceAndCheckRecentDevicesList(location);
        //    }

        //    foreach (DeviceLocation location in Enum.GetValues(typeof(DeviceLocation)))
        //    {
        //        SelectRandomDeviceAndCheckRecentDevicesList(location);
        //    }
        //}

        public void SelectDevicesFromMultipleLocationsAndCheckRecentDevicesList(int devicesCount, int recentDevicesListLength=CommonConstants.RecentDevicesListDefaultMaxCount)
        {
            ExpectedRecentDeviceIds.Clear();

            for (int i = 0; i < devicesCount; i++)
            {
                DeviceLocation location = DataHelper.GetRandomEnum<DeviceLocation>();

                SelectRandomDeviceAndCheckRecentDevicesList(location, recentDevicesListLength);
            }
        }

        public void SelectRandomDevicesUsingDeeplinkAndCheckRecentDevicesList(int devicesCount, int recentDevicesListLength = CommonConstants.RecentDevicesListDefaultMaxCount)
        {
            ExpectedRecentDeviceIds.Clear();

            List<string > randomDeviceIds = uiDeviceHelper.GetRandomExpectedDeviceIds(UiDeviceLocation.AllDevicesPageUi, devicesCount);

            if (randomDeviceIds.Count != devicesCount)
            {
                throw new Exception($"Check failed: expected devices count is {randomDeviceIds.Count} but should be {devicesCount}");
            }

            for (int i = 0; i < devicesCount; i++)
            {
                SelectRandomDeviceUsingDeeplinkAndCheckRecentDevicesList(randomDeviceIds[i], recentDevicesListLength);
            }
        }

        public void AddToExpectedRecentDeviceIds(string deviceId, int recentDevicesListLength = CommonConstants.RecentDevicesListDefaultMaxCount)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return;
            }

            if (!ExpectedRecentDeviceIds.Contains(deviceId))
            {
                ExpectedRecentDeviceIds.Enqueue(deviceId);
            }

            if (ExpectedRecentDeviceIds.Count > recentDevicesListLength)
            {
                ExpectedRecentDeviceIds.Dequeue();
            }
        }

        public void RemoveFromExpectedRecentDeviceIds(string deviceIdToRemove)
        {
            Queue<string> updatedQueue = new Queue<string>();

            foreach (var expDeviceId in ExpectedRecentDeviceIds)
            {
                if (expDeviceId != deviceIdToRemove)
                {
                    updatedQueue.Enqueue(expDeviceId);
                }
            }

            ExpectedRecentDeviceIds = updatedQueue;
        }

        public string ValidateRecentDevicesList()
        {
            List<string> actualDeviceIdsList = isRedesign ? sidebar.GetRecentItemIds() : sidebar.GetRecentDeviceIds();

            List<string> actualDeviceNamesList = isRedesign ? sidebar.GetRecentItemsTitles() : sidebar.GetRecentDeviceNames();

            actualDeviceIdsList.Reverse();

            actualDeviceNamesList.Reverse();

            List<string> expectedDeviceIdsList = ExpectedRecentDeviceIds.ToList();

            string err = "";

            if (actualDeviceIdsList.Count != expectedDeviceIdsList.Count)
            {
                string actualDevices = uiDeviceHelper.GetDeviceModelsByIds(actualDeviceIdsList);

                string expectedDevices = uiDeviceHelper.GetDeviceModelsByIds(expectedDeviceIdsList);

                err = $"Actual: {actualDevices}, Expected: {expectedDevices}.";

                err = $"Count of recent devices is invalid: {actualDeviceIdsList.Count} but expected - {expectedDeviceIdsList.Count}. {err}";

                LogHelper.LogError(log, err);

                return err;
            }

            int actualDeviceIdsCount = actualDeviceIdsList.Count;

            for (int i = 0; i < actualDeviceIdsCount; i++)
            {
                MasterDevice expectedDevice = uiDeviceHelper.GetDeviceByIdViaApi(expectedDeviceIdsList[i]);

                string expectedDeviceNameAndOs = GetExpectedRecentDeviceNameAndOs(expectedDevice);

                expectedDeviceNameAndOs ??= "DeviceModelCannotBeNull";

                expectedDeviceNameAndOs = Regex.Replace(expectedDeviceNameAndOs, @"\s{2,}", " "); //replace multiple adjacent whitespaces by one space

                if (!actualDeviceNamesList[i].StartsWith(expectedDeviceNameAndOs))
                {
                    err = $"{err} Position {actualDeviceIdsCount - i} in 'Recent devices' menu: actual - {actualDeviceNamesList[i]}, expected - {expectedDeviceNameAndOs};";
                }

                if (actualDeviceIdsList[i] != expectedDeviceIdsList[i])
                {
                    err = $"{err} Position {actualDeviceIdsCount - i} in 'Recent devices' menu: actual id - {actualDeviceIdsList[i]}, expected id - {expectedDeviceIdsList[i]}; ";
                }
            }

            LogHelper.Log(log, "Recent devices list validated", err);

            return err;
        }

        public string ClearRecentDeviceList()
        {
            string err = string.Empty;

            int count = sidebar.GetRemoveButtonCount();

            while (count > 0)
            {
                sidebar.RemoveRecentDevice();

                // wait recent device count decreased
                bool isCountDecreasedByOne = UiWaitHelper.Wait(() => count - sidebar.GetRemoveButtonCount() == 1, WaitConstants.ImplicitWaitInSec);

                if (!isCountDecreasedByOne)
                {
                    err = $"Item count did not decrease by 1 (expected: {count - 1 }, actual: {sidebar.GetRemoveButtonCount()})";

                    break;
                }

                count = sidebar.GetRemoveButtonCount();
            }

            return err;
        }

        public void ClearExpectedRecentDevicesList() => ExpectedRecentDeviceIds.Clear();

        public void RemoveRandomRecentDeviceById()
        {
            List<string> actualRecentDeviceIds = sidebar.GetRecentDeviceIds();

            int recentDeviceIdsCount = actualRecentDeviceIds.Count;

            LogHelper.Log(log, $"Recent devices count: {recentDeviceIdsCount}");

            if (recentDeviceIdsCount > 0)
            {
                string randomDeviceId = DataHelper.GetRandomElement(actualRecentDeviceIds);

                sidebar.RemoveRecentDeviceById(randomDeviceId);

                MasterDevice randomDevice = deviceSteps.GetDeviceById(randomDeviceId);

                LogHelper.Log(log, $"Device removed : {randomDevice.Manufacturer} {randomDevice.Model}, id={randomDeviceId}");

                RemoveFromExpectedRecentDeviceIds(randomDeviceId);
            }
        }

        public void RemoveRandomRecentDeviceAndCheckRecentDevicesList()
        {
            RemoveRandomRecentDeviceById();

            string err = ValidateRecentDevicesList();

            LogHelper.LogResult(log, "Device removed from recent devices", err);

            if (!string.IsNullOrEmpty(err))
            {
                throw new Exception(err);
            }
        }

        //----- Helpers -----
        public int RandomOperation(int lastOperation)
        {
            int randomOperation = random.Next(Enum.GetValues(typeof(DeviceLocation)).Length + 1);

            int trialNumber = 0;

            while (randomOperation == lastOperation && trialNumber < 20)
            {
                randomOperation = random.Next(Enum.GetValues(typeof(DeviceLocation)).Length + 1);

                trialNumber++;
            }

            return randomOperation;
        }

        public string GetExpectedRecentDeviceNameAndOs(MasterDevice device)
        {
            string expectedDeviceOs = null;

            if (!(string.IsNullOrEmpty(device.OsName) && string.IsNullOrEmpty(device.OsVersion)))
            {
                expectedDeviceOs = $"{device.OsName} {device.OsVersion}".Trim();
            }

            string deviceNameAndOs = expectedDeviceOs != null && !device.Model.Contains(expectedDeviceOs)
                ? $"{device.Model} ({expectedDeviceOs})"
                : $"{device.Model}";

            deviceNameAndOs = isRedesign ? device.Manufacturer + " " + deviceNameAndOs : deviceNameAndOs;

            return deviceNameAndOs;
        }

        public void CheckHomeMenuItemActive()
        {
            if (sidebar.GetCurrentMenuItem() != "home")
            {
                throw new Exception("Home menu item is not active!");
            }
        }

        public bool IsHomeMenuItemActive() => sidebar.GetCurrentMenuItem() == "home";
    }
}
