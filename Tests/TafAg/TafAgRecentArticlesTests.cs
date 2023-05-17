using NLog;
using Taf.UI.Steps;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Models;
using Taf.UI.Core.Constants;
using Taf.UI.PageObjects.Taf;
using Xunit;
using System.Collections.Generic;
using Taf.UI.Core.Models.Taf;
using Taf.UI.Core.Element;

namespace Tests
{
    public class TafRecentArticlesListTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture fixture;

        private readonly ILogger log;

        private readonly LoginSteps loginSteps;

        private readonly SidebarSteps sidebarSteps;

        private readonly SidebarRecentArticlesSteps sidebarRecentArticlesSteps;

        private readonly SidebarRecentDevicesSteps sidebarRecentDevicesSteps;

        private readonly SidebarRecentItemsSteps sidebarRecentItemsSteps;

        private readonly AppearanceAndLayoutDashboardSettingsSteps dashboardSettingsSteps;

        private readonly AppearanceAndLayoutSidebarSettingsSteps sidebarSettingsSteps;

        public TafRecentArticlesListTests(TestFixture fixture)
        {
            this.fixture = fixture;

            log = LogManager.GetLogger("TafRecentArticlesListUI");

            loginSteps = new LoginSteps(log);

            sidebarSteps = new SidebarSteps(log);

            sidebarRecentArticlesSteps = new SidebarRecentArticlesSteps(log, isRedesign: true);

            sidebarRecentDevicesSteps = new SidebarRecentDevicesSteps(log, isRedesign: true);

            sidebarRecentItemsSteps = new SidebarRecentItemsSteps(log, isRedesign: true);

            dashboardSettingsSteps = new AppearanceAndLayoutDashboardSettingsSteps(log);

            sidebarSettingsSteps = new AppearanceAndLayoutSidebarSettingsSteps(log);

            //clientId = fixture.DbHelper.GetClientIdByName(clientName);
        }

        [Fact(DisplayName = "Delete article from recent articles list test")]
        public void DeleteArticleFromRecentArticleList()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            //to do - check if user not used in other test suite
            User user = UserHelper.GetAdminUser(fixture.TestUsers, fixture.TestEnvironment);

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            err = sidebarRecentArticlesSteps.ClearRecentArticleList();

            Assert.True(string.IsNullOrEmpty(err), err);

            int articlesToSelect = 3;

            //Act
            sidebarRecentArticlesSteps.SelectRandomArticles(articlesToSelect, CommonConstants.RecentArticlesListDefaultMaxCount);

            sidebarRecentArticlesSteps.RemoveRandomArticleFromRecentArticlesList();

            err = sidebarRecentArticlesSteps.ValidateRecentArticlesList();

            //Assert
            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());

            Assert.True(string.IsNullOrEmpty(err), err);
        }

        [Fact(DisplayName = "Recent articles list configuration and functional test")]
        public void RecentArticlesListCheck()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            //to do - check if user not used in other test suite
            User user = UserHelper.GetAdminUser(fixture.TestUsers, fixture.TestEnvironment); //, 1, "VF UK (AQA)_2"); // "VF UK (AQA)");

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            //Act
            //appsMenuSteps.OpenApp(AppLinkType.Taf);
            int recentArticlesListLength = 3;

            err = dashboardSettingsSteps.ChangeRecentArticlesNumber(recentArticlesListLength);

            Assert.True(string.IsNullOrEmpty(err), err);

            dashboardSettingsSteps.OpenAppDeepLink(App.Taf);

            err = sidebarRecentArticlesSteps.ClearRecentArticleList(); //clear recent article list

            Assert.True(string.IsNullOrEmpty(err), err);

            int articlesToSelectCount = recentArticlesListLength + 1;

            sidebarRecentArticlesSteps.SelectRandomArticlesAndCheckRecentArticlesList(articlesToSelectCount, recentArticlesListLength);

            err = sidebarRecentArticlesSteps.ClearRecentArticleList(); //clear recent article list

            Assert.True(string.IsNullOrEmpty(err), err);

            recentArticlesListLength = 0;

            err = dashboardSettingsSteps.ChangeRecentArticlesNumber(recentArticlesListLength);

            Assert.True(string.IsNullOrEmpty(err), err);

            dashboardSettingsSteps.OpenAppDeepLink(App.Taf);

            articlesToSelectCount = recentArticlesListLength + 1;

            sidebarRecentArticlesSteps.SelectRandomArticlesAndCheckRecentArticlesList(articlesToSelectCount, recentArticlesListLength);

            err = sidebarRecentArticlesSteps.ClearRecentArticleList(); //clear recent article list

            Assert.True(string.IsNullOrEmpty(err), err);

            recentArticlesListLength = 5;

            err = dashboardSettingsSteps.ChangeRecentArticlesNumber(recentArticlesListLength);

            Assert.True(string.IsNullOrEmpty(err), err);

            dashboardSettingsSteps.OpenAppDeepLink(App.Taf);

            articlesToSelectCount = recentArticlesListLength + 1;

            sidebarRecentArticlesSteps.SelectRandomArticlesAndCheckRecentArticlesList(articlesToSelectCount, recentArticlesListLength);

            //Assert
            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());

            Assert.True(string.IsNullOrEmpty(err), err);
        }

        [Fact(DisplayName = "Redesign: Recent Items (Articles, Devices) list configuration and functional test")]
        public void RecentItemsListCheck()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            User user = UserHelper.GetAdminUser(fixture.TestUsers, fixture.TestEnvironment, 1, "VF UK (AQA)_2"); // "VF UK (AQA)");

            string err = loginSteps.Login(user, isRedesign: true);

            Assert.True(string.IsNullOrEmpty(err), err);

            //Act
            int recentItemsListLength = 3;

            err = sidebarSettingsSteps.ChangeRecentItemsNumber(recentItemsListLength);

            Assert.True(string.IsNullOrEmpty(err), err);

            sidebarSettingsSteps.OpenAppDeepLink(App.Taf, isRedesign: true);

            err = sidebarRecentItemsSteps.ClearRecentItemList();

            Assert.True(string.IsNullOrEmpty(err), err);

            int itemsToSelect = recentItemsListLength + 1;

            err = sidebarRecentItemsSteps.ClearRecentItemList();

            Assert.True(string.IsNullOrEmpty(err), err);

            sidebarSteps.LockSidebar();

            sidebarRecentItemsSteps.OpenRandomItemsUsingDeeplinkAndCheckRecentItemsList(itemsToSelect, recentItemsListLength);

            err = sidebarRecentItemsSteps.ClearRecentItemList(); //clear recent items list

            Assert.True(string.IsNullOrEmpty(err), err);

            recentItemsListLength = 5;

            err = sidebarSettingsSteps.ChangeRecentItemsNumber(recentItemsListLength);

            Assert.True(string.IsNullOrEmpty(err), err);

            sidebarSettingsSteps.OpenAppDeepLink(App.Taf, isRedesign: true);

            err = sidebarRecentItemsSteps.ClearRecentItemList();

            Assert.True(string.IsNullOrEmpty(err), err);

            itemsToSelect = recentItemsListLength + 1;

            sidebarRecentItemsSteps.OpenRandomItemsUsingDeeplinkAndCheckRecentItemsList(itemsToSelect, recentItemsListLength);

            //Assert

            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());

            Assert.True(string.IsNullOrEmpty(err), err);
        }

        [Fact(DisplayName = "Redesign: Delete item from recently viewed items list test")]
        public void DeleteItemFromRecentItemListRedesign()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //to do - check if user not used in other test suite
            User user = UserHelper.GetAdminUser(fixture.TestUsers, fixture.TestEnvironment);

            string err = loginSteps.Login(user, isRedesign: true);

            Assert.True(string.IsNullOrEmpty(err), err);

            sidebarSteps.LockSidebar();

            err = sidebarRecentItemsSteps.ClearRecentItemList();

            Assert.True(string.IsNullOrEmpty(err), err);

            int itemsToSelect = 3;

            //Act
            sidebarRecentItemsSteps.OpenRandomItemsUsingDeeplink(itemsToSelect, CommonConstants.RecentItemsListDefaultCount);

            sidebarRecentItemsSteps.RemoveRandomRecentItem();

            err = sidebarRecentItemsSteps.ValidateRecentlyViewedList();

            //Assert
            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());

            Assert.True(string.IsNullOrEmpty(err), err);
        }

        [Fact(DisplayName = "Redesign: Open item from recently viewed items list test")]
        public void OpenItemFromRecentItemListRedesign()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //to do - check if user not used in other test suite
            User user = UserHelper.GetAdminUser(fixture.TestUsers, fixture.TestEnvironment);

            string err = loginSteps.Login(user, isRedesign: true);

            Assert.True(string.IsNullOrEmpty(err), err);

            sidebarSteps.LockSidebar();

            err = sidebarRecentItemsSteps.ClearRecentItemList();

            Assert.True(string.IsNullOrEmpty(err), err);

            int itemsToSelect = 3;

            sidebarRecentItemsSteps.OpenRandomItemsUsingDeeplink(itemsToSelect, CommonConstants.RecentItemsListDefaultCount);

            //Act
            err = sidebarRecentItemsSteps.CheckRandomItemCanBeOpenedFromRecentlyViewedList();

            //Assert
            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());

            Assert.True(string.IsNullOrEmpty(err), err);
        }
    }
}
