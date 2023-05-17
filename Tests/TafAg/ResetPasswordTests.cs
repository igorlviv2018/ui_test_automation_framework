using NLog;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models;
using Taf.UI.PageObjects;
using Taf.UI.Steps;
using Xunit;

namespace Tests
{
    public class ResetPasswordTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture fixture;

        private readonly ILogger log;

        private readonly string clientName;

        private readonly LoginSteps loginSteps;

        private readonly ResetPasswordSteps resetPasswordSteps;

        private readonly BrowserSteps browserSteps;

        //private readonly int? clientId;

        public ResetPasswordTests(TestFixture fixture)
        {
            this.fixture = fixture;

            log = LogManager.GetLogger("TafResetPasswordUI");

            loginSteps = new LoginSteps(log);

            resetPasswordSteps = new ResetPasswordSteps(log);

            browserSteps = new BrowserSteps(log);

            clientName = fixture.TestConfig["ClientName"];

            //clientId = fixture.DbHelper.GetClientIdByName(clientName);
        }

        [Fact(DisplayName = "Go to Reset password page from Login page and back to Login page")]
        public void GoToResetPasswordPageFromLoginPageAndBack()
        {
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            browserSteps.OpenAppDeepLink(App.Taf, isRedesign: true);

            bool isLoginPageOpened = loginSteps.IsLoginPageOpened(isRedesign: true);

            Assert.True(isLoginPageOpened, "Login page is not displayed");

            loginSteps.GoToRestorePasswordPage(isRedesign: true);

            string err = resetPasswordSteps.CheckResetPasswordPageOpened();

            Assert.True(string.IsNullOrEmpty(err), err);

            err = resetPasswordSteps.CheckJustRememberedLinkRedirectsToLoginPage();

            Assert.True(string.IsNullOrEmpty(err), err);

            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());
        }

        [Fact(DisplayName = "Try to reset password with invalid email (not in system)")]
        [Trait("Category", "ResetPassword")]
        public void TryResetPasswordWithInvalidEmail()
        {
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            browserSteps.OpenAppDeepLink(App.Taf, isRedesign: true);

            Assert.True(loginSteps.IsLoginPageOpened(isRedesign: true), "Login page is not displayed");

            loginSteps.GoToRestorePasswordPage(isRedesign: true);

            string err = resetPasswordSteps.CheckResetPasswordPageOpened();

            Assert.True(string.IsNullOrEmpty(err), err);

            string notExistingUserEmail = "not_existing_user@gmail.com";

            resetPasswordSteps.ResetPassword(notExistingUserEmail);

            err = resetPasswordSteps.CheckUserWithEmailNotFoundError(notExistingUserEmail);

            Assert.True(string.IsNullOrEmpty(err), err);

            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());
        }

        [Fact(DisplayName = "Try to reset password with valid email")]
        [Trait("Category", "ResetPassword")]
        public void TryResetPasswordWithValidEmail()
        {
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            browserSteps.OpenAppDeepLink(App.Taf, isRedesign: true);

            Assert.True(loginSteps.IsLoginPageOpened(isRedesign: true), "Login page is not displayed");

            loginSteps.GoToRestorePasswordPage(isRedesign: true);

            string err = resetPasswordSteps.CheckResetPasswordPageOpened();

            Assert.True(string.IsNullOrEmpty(err), err);

            string validUserEmail = "aqa.basic.user3@gmail.com";

            resetPasswordSteps.ResetPassword(validUserEmail);

            err = resetPasswordSteps.CheckEmailWasSentMessageDisplayed(validUserEmail);

            Assert.True(string.IsNullOrEmpty(err), err);

            err = resetPasswordSteps.CheckEmailSentPage();

            Assert.True(string.IsNullOrEmpty(err), err);

            err = resetPasswordSteps.CheckBackToLoginButtonRedirectsToLoginPage();

            Assert.True(string.IsNullOrEmpty(err), err);

            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());
        }

        [Fact(DisplayName = "Try to login with expired password redirects to Reset password")]
        [Trait("Category", "ResetPassword")]
        public void TryLoginWithExpiredPassword()
        {
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            browserSteps.OpenAppDeepLink(App.Taf, isRedesign: true);

            User userWithExpiredPass = UserHelper.GetBasicUser(fixture.TestUsers, fixture.TestEnvironment, client: "VF UK (AQA)_2");

            string err = loginSteps.Login(userWithExpiredPass.Email, userWithExpiredPass.Password, isRedesign: true);

            Assert.True(string.IsNullOrEmpty(err), err);

            err = resetPasswordSteps.CheckYourPasswordExpiredPage();

            Assert.True(string.IsNullOrEmpty(err), err);

            err = resetPasswordSteps.CheckResetPasswordButtonRedirectsToResetPasswordPage();

            Assert.True(string.IsNullOrEmpty(err), err);

            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());
        }

        [Fact(DisplayName = "Check Create a new password page")]
        [Trait("Category", "ResetPassword")]
        public void CheckCreateANewPasswordPage()
        {
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            browserSteps.OpenDeepLink(App.Taf, LinkConstants.ResetPasswordLinkWithGuid, isRedesign: true);

            string err = resetPasswordSteps.CheckCreateNewPasswordPage();

            Assert.True(string.IsNullOrEmpty(err), err);

            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());
        }
    }
}
