using NLog;
using Taf.UI.Steps;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Constants;
using System.Collections.Generic;
using Taf.UI.Core.Enums;
using Xunit;
using Taf.UI.Core.Models;
using Taf.UI.PageObjects.Administration.SystemConfiguration.Taf;

namespace Tests
{
    public class TafConfigurationTestsRedesign : IClassFixture<TestFixture>
    {
        private readonly TestFixture fixture;

        private readonly ILogger log;

        private readonly LoginSteps loginSteps;

        private readonly ProfileMenuSteps profileMenuSteps;

        private readonly SystemConfigSideBarSteps sideBarSteps;

        private readonly GeneralSettingsGuidesSteps guideSettingsSteps;

        private readonly GuideSteps guideSteps;

        private readonly SystemConfigCommonSteps commonSteps;

        private readonly BrowserSteps browserSteps;

        private readonly string clientName;

        public TafConfigurationTestsRedesign(TestFixture fixture)
        {
            this.fixture = fixture;

            log = LogManager.GetLogger("TafConfigUIRedesign");

            clientName = fixture.TestConfig["ClientName"];

            loginSteps = new LoginSteps(log);

            profileMenuSteps = new ProfileMenuSteps(log);

            sideBarSteps = new SystemConfigSideBarSteps(log, isRedesign: true);

            guideSettingsSteps = new GeneralSettingsGuidesSteps(log);

            guideSteps = new GuideSteps(log);

            commonSteps = new SystemConfigCommonSteps(log, isRedesign: true);

            browserSteps = new BrowserSteps(log);

            //clientId = fixture.DbHelper.GetClientIdByName(clientName);
        }

        [Fact(DisplayName = "Features activation check test")]
        [Trait("Category", "TafCheckConfig")]
        public void FeaturesActivationCheck()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            User user = UserHelper.GetAdminUser(fixture.TestUsers, fixture.TestEnvironment, 1, "VF UK (AQA)_2");//, "VF UK (AQA)");

            string err = loginSteps.Login(user, isRedesign: true);

            Assert.True(string.IsNullOrEmpty(err), err);

            //new BrowserSteps(log).OpenDeepLink("/admin/config/content/approval-rules"); ///admin/config/content/approval-rules

            browserSteps.OpenAppDeepLink(App.SystemConfiguration, isRedesign: true);

            sideBarSteps.LockSidebar();

            sideBarSteps.OpenConfigurationPage(App.Taf, "Appearance & layout");

            commonSteps.OpenConfigurationTab("Sidebar");

            string e2 = new AppearanceAndLayoutSidebarSettingsSteps(log).ChangeRecentItemsNumber(6);

            sideBarSteps.OpenConfigurationPage(App.Taf, "Appearance & layout");

            commonSteps.OpenConfigurationTab("Dashboard settings");

            //Act
            err = profileMenuSteps.CheckUserNameAndEmail();

            LogHelper.LogResult(log, "User name and email checked", err);

            //Assert
            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());

            Assert.True(string.IsNullOrEmpty(err), err);
        }

        [Fact(DisplayName = "Guides view mode check test")]
        [Trait("Category", "TafCheckConfig")]
        public void GuidesViewModeCheck()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            //to change user
            User user = UserHelper.GetAdminUser(fixture.TestUsers, fixture.TestEnvironment, 1, "VF UK (AQA)_2");//, "VF UK (AQA)");

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            //Act
            guideSettingsSteps.OpenPageViaDeepLink();

            err = guideSettingsSteps.SetGuidesViewMode(GuideViewType.List);

            Assert.True(string.IsNullOrEmpty(err), err);

            commonSteps.OpenAppDeepLink(App.Taf);

            err = guideSteps.OpenContentItemForRandomDeviceViaSearch("Insert SIM", UiContentType.Guide);

            Assert.True(string.IsNullOrEmpty(err), err);

            err = guideSteps.CheckGuideOpened();

            Assert.True(string.IsNullOrEmpty(err), err);

            err = guideSteps.CheckGuideViewMode(GuideViewType.List);

            Assert.True(string.IsNullOrEmpty(err), err);

            guideSettingsSteps.OpenPageViaDeepLink();

            err = guideSettingsSteps.SetGuidesViewMode(GuideViewType.Slider);

            Assert.True(string.IsNullOrEmpty(err), err);

            commonSteps.OpenAppDeepLink(App.Taf);

            err = guideSteps.OpenContentItemForRandomDeviceViaSearch("Insert SIM", UiContentType.Guide);

            Assert.True(string.IsNullOrEmpty(err), err);

            err = guideSteps.CheckGuideOpened();

            Assert.True(string.IsNullOrEmpty(err), err);

            err = guideSteps.CheckGuideViewMode(GuideViewType.Slider);

            //Assert
            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());

            Assert.True(string.IsNullOrEmpty(err), err);
        }

        [Fact(DisplayName = "News settings check test")]
        [Trait("Category", "TafCheckConfig")]
        public void NewsSettingsCheck()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            //to change user
            User user = UserHelper.GetAdminUser(fixture.TestUsers, fixture.TestEnvironment, 1, "VF UK (AQA)_2");//, "VF UK (AQA)");

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            //new AppsMenuSteps(log).OpenApp(AppLinkType.SystemConfiguration);

            //Act
            new GeneralSettingsNewsSteps(log).SetPostIntoNewsFeed("Article", false);

            new GeneralSettingsNewsPage().ClickSwitcher("Article");

            guideSettingsSteps.OpenPageViaDeepLink();

            err = guideSettingsSteps.SetGuidesViewMode(GuideViewType.List);

            Assert.True(string.IsNullOrEmpty(err), err);

            commonSteps.OpenAppDeepLink(App.Taf);

            err = guideSteps.OpenContentItemForRandomDeviceViaSearch("Insert SIM", UiContentType.Guide);

            Assert.True(string.IsNullOrEmpty(err), err);

            err = guideSteps.CheckGuideOpened();

            Assert.True(string.IsNullOrEmpty(err), err);

            err = guideSteps.CheckGuideViewMode(GuideViewType.List);

            Assert.True(string.IsNullOrEmpty(err), err);

            guideSettingsSteps.OpenPageViaDeepLink();

            err = guideSettingsSteps.SetGuidesViewMode(GuideViewType.Slider);

            Assert.True(string.IsNullOrEmpty(err), err);

            commonSteps.OpenAppDeepLink(App.Taf);

            err = guideSteps.OpenContentItemForRandomDeviceViaSearch("Insert SIM", UiContentType.Guide);

            Assert.True(string.IsNullOrEmpty(err), err);

            err = guideSteps.CheckGuideOpened();

            Assert.True(string.IsNullOrEmpty(err), err);

            err = guideSteps.CheckGuideViewMode(GuideViewType.Slider);

            //Assert
            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());

            Assert.True(string.IsNullOrEmpty(err), err);
        }
    }
}
