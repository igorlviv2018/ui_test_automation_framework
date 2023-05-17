using NLog;
using System.Collections.Generic;
using Taf.UI.Steps;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Enums;
using Taf.Api.Tests.Models;
using Taf.UI.Core.Models;
using Taf.Core.Models.Devices;
using Taf.UI.Core.Constants;
using Xunit;

namespace Tests
{
    public class DeviceTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture fixture;

        private readonly ILogger log;

        private readonly LoginSteps loginSteps;

        private readonly DeviceSteps deviceSteps;

        private readonly BrowserSteps browserSteps;

        private readonly SidebarRecentDevicesSteps recentDevicesSteps;

        private readonly AppearanceAndLayoutDashboardSettingsSteps dashboardSettingsSteps;

        public DeviceTests(TestFixture fixture)
        {
            this.fixture = fixture;

            log = LogManager.GetLogger("TafDeviceUI");

            loginSteps = new LoginSteps(log);

            deviceSteps = new DeviceSteps(App.Taf, log);

            browserSteps = new BrowserSteps(log);

            recentDevicesSteps = new SidebarRecentDevicesSteps(log);

            dashboardSettingsSteps = new AppearanceAndLayoutDashboardSettingsSteps(log);

            //clientId = fixture.DbHelper.GetClientIdByName(clientName);
        }

        [Fact(DisplayName = "D1: Open device from multiple locations")]
        public void OpenDeviceFromDevicesPage()
        {
            //Arrange
            string testDesc = "Open device from multiple locations";

            LogHelper.LogTestStart(log, testDesc);

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            //to do - change user (use basuc user)
            User user = UserHelper.GetAdvisorAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            //Act
            List<string> errors = new List<string>();

            //List<OsSelectorInfo> expectedOsSelectors = new UiDeviceHelper().GetExpectedOsSelectors("F-19190");

            // device from Dashboard Most Popular section
            err = deviceSteps.SelectAndCheckRandomDeviceFrom(DeviceLocation.DashboardPopular);
            ErrorHelper.AddToErrorList(errors, err);

            // device from 'Devices' page
            err = deviceSteps.SelectAndCheckRandomDeviceFrom(DeviceLocation.DevicesPage);
            ErrorHelper.AddToErrorList(errors, err);

            // device from 'Devices' page (if client has such devices) with multiple OS selectors
            err = deviceSteps.SelectAndCheckRandomDeviceFrom(DeviceLocation.DevicesPage, new DeviceFilter() { HasPeerSlaves = true });
            ErrorHelper.AddToErrorList(errors, err);

            // device from 'Devices' page (if client has such devices) with a specification
            err = deviceSteps.SelectAndCheckRandomDeviceFrom(DeviceLocation.DevicesPage, new DeviceFilter() { HasSpecs = true });
            ErrorHelper.AddToErrorList(errors, err);

            // device from 'Devices' page (if client has such devices) with a 3D model
            err = deviceSteps.SelectAndCheckRandomDeviceFrom(DeviceLocation.DevicesPage, new DeviceFilter() { HasThreeDModel = true });
            ErrorHelper.AddToErrorList(errors, err);

            // device from 'Devices' page (if client has such devices) with a virtual device
            //err = deviceSteps.SelectAndCheckRandomDeviceFrom(DeviceLocation.DevicesPage, new DeviceFilter() { HasVirtualDevice = true });
            //ErrorHelper.AddToErrorList(errors, err);

            // device from autocomplete search results 
            err = deviceSteps.SelectAndCheckRandomDeviceFrom(DeviceLocation.SearchResultsAutocomplete);
            ErrorHelper.AddToErrorList(errors, err);

            // device from 'News' page
            err = deviceSteps.SelectAndCheckRandomDeviceFrom(DeviceLocation.NewsPage);
            ErrorHelper.AddToErrorList(errors, err);

            // device from 'Recent devices' list
            //err = deviceSteps.SelectAndCheckRandomDeviceFrom(DeviceLocation.RecentDevicesList);
            //ErrorHelper.AddToErrorList(errors, err);

            //Assert
            string allErrors = string.Join("; ", errors.ToArray());

            LogHelper.LogTestEnd(log, errors, testDesc);

            Assert.True(errors.Count == 0, allErrors);
        }

        [Fact(DisplayName = "Device OS can be selected test")]
        public void SelectDeviceOsOnDevicePage()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //to do - change user (use basuc user)
            User user = UserHelper.GetAdvisorAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            //Act
            browserSteps.OpenAppDeepLink(App.Taf);

            // select device from 'Devices' page (if client has such devices) with multiple OS selectors
            deviceSteps.SelectRandomDevice(DeviceLocation.DevicesPage, new DeviceFilter() { HasPeerSlaves = true });

            string deviceId = deviceSteps.SelectRandomDeviceOsSelector();

            err = deviceSteps.CheckDevicePage(deviceId);

            //Assert
            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());

            Assert.True(string.IsNullOrEmpty(err), err);
        }

        //[Fact(DisplayName = "D7: Most popular devices on dashboard")]
        //public void MostPopularDevices()
        //{
        //    //Arrange
        //    string testDesc = "D7: Most popular devices on dashboard";

        //    LogHelper.LogTestStart(log, testDesc);

        //    loginSteps.OpenApp();

        //    string email = "aqa.movistar.pe.adm.dev@gmail.com"; // "aqa.swisscomch.adm.dev@gmail.com";

        //    string password = SecretsHelper.GetUserPasswordByEmail(fixture.TestConfig, email);

        //    loginSteps.Login(email, password);

        //    Assert.True(loginSteps.DashboardPageOpened(), "Not a home page!");

        //    log.Info($"Logged in successful");

        //    //Act
        //    string err = new DashboardSteps().CheckPopularDevices();

        //    LogHelper.LogTestEnd(log, err, testDesc);

        //    //Assert
        //    Assert.True(string.IsNullOrEmpty(err), err);
        //}

        [Fact(DisplayName = "D4: Devices page validation")]
        public void DevicesPage()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            //to do - change user (use basuc user)
            User user = UserHelper.GetBasicUser(fixture.TestUsers, fixture.TestEnvironment, 1, "VF UK (AQA)");

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            //Act
            browserSteps.OpenAppDeepLink(App.Taf);

            deviceSteps.OpenDevicesPage();

            List<string> errors = new List<string>();

            err = deviceSteps.CheckAllDevicesBrands();

            ErrorHelper.AddToErrorList(errors, err);

            err = deviceSteps.CheckDevicesPageTabs();

            ErrorHelper.AddToErrorList(errors, err);

            err = deviceSteps.CheckDevicesInTab(null, "All brands"); // All tab

            ErrorHelper.AddToErrorList(errors, err);

            //Assert
            string allErrors = string.Join("; ", errors);

            LogHelper.LogTestEnd(log, allErrors, XUnitHelper.FactDisplayName());

            Assert.True(string.IsNullOrEmpty(allErrors), allErrors);
        }

        [Fact(DisplayName = "Select brand on Devices page test")]
        public void SelectBrandOnDevicesPage()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            //to do - change user (use basuc user)
            User user = UserHelper.GetAdvisorAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            //Act
            browserSteps.OpenAppDeepLink(App.Taf);

            deviceSteps.OpenDevicesPage();

            List<string> errors = new List<string>();

            string selectedBrand = deviceSteps.SelectRandomBrand();

            err = deviceSteps.CheckDevicesInTab(null, selectedBrand); //All devices tab

            ErrorHelper.AddToErrorList(errors, err);

            err = deviceSteps.CheckDevicesInAnyTab(selectedBrand);

            ErrorHelper.AddToErrorList(errors, err);

            //Assert
            string allErrors = string.Join("; ", errors.ToArray());

            LogHelper.LogTestEnd(log, allErrors, XUnitHelper.FactDisplayName());

            Assert.True(string.IsNullOrEmpty(allErrors), allErrors);
        }

        [Fact(DisplayName = "D6: Recent Devices list configuration and functional test")]
        public void RecentDevicesListCheck()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            User user = UserHelper.GetAdminUser(fixture.TestUsers, fixture.TestEnvironment, 1, "VF UK (AQA)_2"); // "VF UK (AQA)");

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            //Act
            int recentDevicesListLength = 3;

            err = dashboardSettingsSteps.ChangeRecentDevicesNumber(recentDevicesListLength);

            Assert.True(string.IsNullOrEmpty(err), err);

            browserSteps.OpenAppDeepLink(App.Taf);

            int devicesToSelect = recentDevicesListLength + 1;

            recentDevicesSteps.SelectDevicesFromMultipleLocationsAndCheckRecentDevicesList(devicesToSelect, recentDevicesListLength);

            err = recentDevicesSteps.ClearRecentDeviceList(); //clear recent device list

            Assert.True(string.IsNullOrEmpty(err), err);

            recentDevicesListLength = 5;

            err = dashboardSettingsSteps.ChangeRecentDevicesNumber(recentDevicesListLength);

            Assert.True(string.IsNullOrEmpty(err), err);

            browserSteps.OpenAppDeepLink(App.Taf);

            devicesToSelect = recentDevicesListLength + 1;

            recentDevicesSteps.SelectDevicesFromMultipleLocationsAndCheckRecentDevicesList(devicesToSelect, recentDevicesListLength);

            //Assert

            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());

            Assert.True(string.IsNullOrEmpty(err), err);
        }

        [Fact(DisplayName = "D6_1: Romove device from Recent Devices list test")]
        public void RemoveDeviceFromRecentDevicesListCheck()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            User user = UserHelper.GetAdminUser(fixture.TestUsers, fixture.TestEnvironment, 1, "VF UK (AQA)_2"); // "VF UK (AQA)");

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            recentDevicesSteps.ClearExpectedRecentDevicesList();

            int devicesToSelect = 2;

            //Act
            recentDevicesSteps.SelectRandomDevicesUsingDeeplink(devicesToSelect, CommonConstants.RecentDevicesListDefaultMaxCount);

            recentDevicesSteps.RemoveRandomRecentDeviceAndCheckRecentDevicesList();

            //Assert
            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());

            Assert.True(string.IsNullOrEmpty(err), err);
        }

        //[Fact(DisplayName = "D5: Devices comparison")]
        //public void DevicesCompare()
        //{
        //    //Arrange
        //    string testDesc = "D5: Devices comparison";

        //    LogHelper.LogTestStart(log, testDesc);

        //    loginSteps.OpenApp();

        //    string email = "aqa.movistar.pe.adm.dev@gmail.com";// "aqa.swisscomch.adm.dev@gmail.com";

        //    string password = Taf.UI.Core.Helpers.SecretsHelper.GetUserPasswordByEmail(fixture.TestConfig, email);

        //    loginSteps.Login(email, password);

        //    Assert.True(loginSteps.DashboardPageOpened(), "Not a home page!");
        //    log.Info($"Logged in successful");

        //    //Act
        //    UiDeviceHelper uiDeviceHelper = new UiDeviceHelper();

        //    List<string> errors = new List<string>();

        //    List<MasterDevice> devices = uiDeviceHelper.GetExpectedDevicesOnDevicesPage();

        //    //Assert
        //    string allErrors = string.Join("; ", errors.ToArray());

        //    LogHelper.LogTestEnd(log, errors, testDesc);

        //    Assert.True(errors.Count == 0, allErrors);
        //}

        //[Fact(DisplayName = "Share link test")]
        //public void ShareLink()
        //{
        //    string testDesc = "Share link test";

        //    log.Info($"*** {testDesc} started ***");

        //    loginSteps.OpenApp();

        //    string email = "aqa.movistar.pe.adm.dev@gmail.com";// "aqa.swisscomch.adm.dev@gmail.com";

        //    string password = SecretsHelper.GetUserPasswordByEmail(fixture.TestConfig, email);

        //    loginSteps.Login(email, password);

        //    Assert.True(loginSteps.DashboardPageOpened(), "Not a home page!");

        //    log.Info($"Logged in successful");

        //    Dictionary<UrlTemplateType, string> urls = new ApiHelper().GetContentSharingUrls(new BrowserStorageHelper().GetAuthToken());

        //    deviceSteps.SelectDeviceOnAllDevicesPage("F-1403");

        //    List<string> errors = new List<string>();

        //    //Assert
        //    string allErrors = string.Join("; ", errors.ToArray());

        //    LogHelper.LogTestEnd(log, errors, testDesc);

        //    Assert.True(errors.Count == 0, allErrors);
        //}
    }
}
