using NLog;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models;
using Taf.UI.Steps;
using Xunit;

namespace Tests
{
    public class LoginSignout : IClassFixture<TestFixture>
    {
        private readonly TestFixture fixture;

        private readonly ILogger log;

        private readonly string clientName;

        private readonly LoginSteps loginSteps;

        private readonly ProfileMenuSteps profileMenuSteps;

        private readonly BrowserSteps browserSteps;

        //private readonly int? clientId;

        public LoginSignout(TestFixture fixture)
        {
            this.fixture = fixture;

            log = LogManager.GetLogger("TafLoginUI");

            loginSteps = new LoginSteps(log);

            profileMenuSteps = new ProfileMenuSteps(log);

            browserSteps = new BrowserSteps(log);

            clientName = fixture.TestConfig["ClientName"];

            //clientId = fixture.DbHelper.GetClientIdByName(clientName);
        }
        
        [Fact(DisplayName = "Login with 'Stay signed in' option enabled")]
        [Trait("Category", "LoginSignout")]
        public void StaySignedInTest()
        {
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            User user = UserHelper.GetBasicUser(fixture.TestUsers, fixture.TestEnvironment, 2, "VF UK (AQA)");

            string err = loginSteps.Login(user, rememberMe: false);

            Assert.True(string.IsNullOrEmpty(err), err);

            Assert.True(loginSteps.IsSpLogoPresent(), "Login failed. Home page is not opened!");

            loginSteps.SaveLocalStorage();

            loginSteps.RestartBrowser();

            browserSteps.OpenAppDeepLink(App.Taf);

            loginSteps.WaitPageLoad();

            loginSteps.RestoreLocalStorage();

            loginSteps.Refresh();

            loginSteps.ClearLocalStorage();

            Assert.True(loginSteps.WaitSpLogoPresent(), "Browser page refreshed: Not a home page!");

            profileMenuSteps.SignOut();

            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());
        }

        [Fact(DisplayName = "Not implemented yet in redesign: Login with 'Keep me logged in' option enabled - redesign")]
        [Trait("Category", "LoginSignout")]
        public void KeepMeLoggedInTest()
        {
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            User user = UserHelper.GetBasicUser(fixture.TestUsers, fixture.TestEnvironment, 2, "VF UK (AQA)");

            string err = loginSteps.Login(user, rememberMe: false, isRedesign: true);

            Assert.True(string.IsNullOrEmpty(err), err);

            Assert.True(loginSteps.IsSpLogoPresent(isRedesign: true), "Login failed. Home page is not opened!");

            loginSteps.SaveLocalStorage();

            loginSteps.RestartBrowser();

            browserSteps.OpenAppDeepLink(App.Taf, isRedesign: true);

            loginSteps.WaitPageLoad(isRedesign: true);

            loginSteps.RestoreLocalStorage();

            loginSteps.Refresh();

            loginSteps.ClearLocalStorage();

            Assert.True(loginSteps.WaitSpLogoPresent(isRedesign: true), "Browser page refreshed: Not a home page!");

            profileMenuSteps.SignOut(isRedesign: true);

            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());
        }
        
        [Fact(DisplayName = "Login and signout")]
        [Trait("Category", "LoginSignout")]
        public void LoginAndSignout()
        {
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            User user = UserHelper.GetBasicUser(fixture.TestUsers, fixture.TestEnvironment, 2, "VF UK (AQA)");

            string err = loginSteps.Login(user, rememberMe: false);

            Assert.True(string.IsNullOrEmpty(err), err);

            profileMenuSteps.SignOut();

            Assert.True(loginSteps.IsLoginPageOpened(), "Login page is not displayed");

            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());
        }
        
        [Fact(DisplayName = "Login using invalid password (Neg)")]
        [Trait("Category", "LoginSignout")]
        public void LoginWithInvalidEmailPass()
        {
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            browserSteps.OpenAppDeepLink(App.Taf);

            string err = loginSteps.Login("aqa.sp.user.dev@protonmail.com", "121323443");

            Assert.True(string.IsNullOrEmpty(err), err);

            err = loginSteps.CheckInvalidEmailOrPassError(CommonConstants.InvalidUsernameOrPassword);

            Assert.True(string.IsNullOrEmpty(err), err);

            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());
        }
        
        [Fact(DisplayName = "Redesign: Login and signout")]
        [Trait("Category", "LoginSignoutRedesign")]
        public void LoginAndSignoutRedesign()
        {
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            User user = UserHelper.GetBasicUser(fixture.TestUsers, fixture.TestEnvironment, 2, "VF UK (AQA)");

            string err = loginSteps.Login(user, rememberMe: false, isRedesign: true);

            Assert.True(string.IsNullOrEmpty(err), err);

            profileMenuSteps.SignOut(isRedesign:true);

            Assert.True(loginSteps.IsLoginPageOpened(isRedesign:true), "Login page is not displayed after log out");

            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());
        }

        [Fact(DisplayName = "Redesign: Login using invalid password (Neg)")]
        [Trait("Category", "LoginSignoutRedesign")]
        public void LoginWithInvalidEmailPassRedesign()
        {
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            browserSteps.OpenAppDeepLink(App.Taf, isRedesign: true);

            string err = loginSteps.Login("aqa.sp.user.dev@protonmail.com", "121323443", isRedesign: true);

            Assert.True(string.IsNullOrEmpty(err), err);

            err = loginSteps.CheckInvalidEmailOrPassError(CommonConstants.InvalidUsernameOrPassword, isRedesign:true);

            Assert.True(string.IsNullOrEmpty(err), err);

            bool isLoginPageOpened = loginSteps.IsLoginPageOpened(isRedesign: true);

            Assert.True(isLoginPageOpened, "Login page is not displayed after logging in with invalid email/password");

            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());
        }
    }
}
