using NLog;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.PageObjects;
using System.Collections.Generic;

namespace Taf.UI.Steps
{
    public class ResetPasswordSteps : BaseSteps
    {
        public ResetPasswordSteps(ILogger logger) : base(App.Taf, logger)
        {
            loginPage = new LoginPage();

            resetPasswordPage = new ResetPasswordPage();

            pageSteps = new PageSteps(logger);

            loginSteps = new LoginSteps(logger);

            spinner = new Spinner(App.Taf, isRedesign: true);
        }

        private readonly LoginPage loginPage;

        private readonly ResetPasswordPage resetPasswordPage;

        private readonly PageSteps pageSteps;

        private readonly LoginSteps loginSteps;

        private readonly Spinner spinner;

        public string CheckResetPasswordPageOpened() => pageSteps.CheckPageElementsDisplayed(resetPasswordPage.GetExpectedPageElements());

        public string CheckJustRememberedLinkRedirectsToLoginPage()
        {
            resetPasswordPage.ClickJustRememberedLink();

            return loginPage.IsAt(isRedesign: true) ? string.Empty : "Just remembered link check failed: login page is not displayed after link click";
        }

        public void GoToRestorePasswordPage(bool isRedesign=false) => loginPage.ClickForgotYourPassLink(isRedesign);

        public void ResetPassword(string email)
        {
            resetPasswordPage.SetEmail(email);

            resetPasswordPage.ClickGetResetLinkButton();
        }

        public string CheckUserWithEmailNotFoundError(string email)
        {
            UiWaitHelper.Wait(() => resetPasswordPage.IsErrorDisplayed(), WaitConstants.CheckElementExistInSec);

            string actualError = resetPasswordPage.GetErrorText();

            string expectedError = string.Format(MessageConstants.UserWithSuchEmailNotFoundMessage, email);

            return actualError == expectedError
                ? string.Empty
                : $"Invalid error: {actualError}, expected: {expectedError}";
        }

        public string CheckEmailWasSentMessageDisplayed(string email)
        {
            bool isSuccessDisplayed = UiWaitHelper.Wait(() => resetPasswordPage.IsSuccessDisplayed(), WaitConstants.CheckElementExistInSec);

            return isSuccessDisplayed
                ? string.Empty
                : $"Success messange not displayed if using valid email ('{email}')";
        }

        public string CheckBackToLoginButtonRedirectsToLoginPage()
        {
            resetPasswordPage.ClickBackToLoginButton();

            spinner.WaitTopProgressBarToDisappear();

            bool isLoginPageOpened = loginSteps.IsLoginPageOpened(isRedesign: true);

            return isLoginPageOpened
                ? string.Empty
                : $"Back to login button clicked: Login page not opened";
        }

        public string CheckResetPasswordButtonRedirectsToResetPasswordPage()
        {
            resetPasswordPage.ClickResetPasswordButton();

            spinner.WaitTopProgressBarToDisappear();

            return CheckResetPasswordPage();
        }

        public string CheckResetPasswordPage()=> CheckPage(TextConstants.ResetPasswordPageName,
                "Reset password",
                resetPasswordPage.GetExpectedPageElements()
            );

        public string CheckEmailSentPage() => CheckPage(TextConstants.ResetPasswordEmailSentPageName,
                "Email sent (success)",
                resetPasswordPage.GetExpectedEmailSentPageElements()
            );

        public string CheckYourPasswordExpiredPage() => CheckPage(TextConstants.ResetPasswordYourPasswordExpiredPageName,
                "Your password has expired",
                resetPasswordPage.GetExpectedYourPasswordExpiredPageElements()
            );

        public string CheckCreateNewPasswordPage() => CheckPage(TextConstants.ResetPasswordCreateNewPasswordPageName,
                "Create a new password",
                resetPasswordPage.GetExpectedCreateNewPasswordPageElements()
            );

        public string CheckPage(string expectedPageName, string pageDescription, Dictionary<string, string> expectedPageElements)
        {
            List<string> errors = new List<string>();

            string actualName = resetPasswordPage.GetPageName();

            string err = pageSteps.CheckPageName(actualName, expectedPageName);

            ErrorHelper.AddToErrorList(errors, err);

            err = pageSteps.CheckPageElementsDisplayed(expectedPageElements);

            ErrorHelper.AddToErrorList(errors, err);

            err = pageSteps.CheckCopyrigtOnPageRedesign(); // check copyright

            ErrorHelper.AddToErrorList(errors, err);

            err = ErrorHelper.ConvertErrorsToString(errors, $"'{pageDescription}' page check failed: ");

            LogHelper.LogResult(log, $"'{pageDescription}' page checked", err);

            return err;
        }
    }
}
