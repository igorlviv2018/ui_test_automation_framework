using Microsoft.Extensions.Configuration;
using NLog;
using Taf.UI.Core.Configuration;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Exceptions;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models;
using Taf.UI.PageObjects;
using System.Collections.Generic;

namespace Taf.UI.Steps
{
    public class LoginSteps : BaseSteps
    {
        public LoginSteps(ILogger logger) : base(App.Taf, logger)
        {
            TestConfig = new TestConfiguration().ConfigRoot;

            browserSteps = new BrowserSteps(logger);
        }

        private readonly IConfigurationRoot TestConfig;

        private readonly LoginPage loginPage = new LoginPage();

        private readonly NavigationBar navigationBar = new NavigationBar();

        private readonly BrowserStorageHelper localStorage = new BrowserStorageHelper();

        private bool rememberMe;

        private readonly Spinner spinner = new Spinner(App.Taf);

        private readonly BrowserSteps browserSteps;

        public string Login(string email, string password, bool  rememberMe=false, bool isRedesign=false)
        {
            if (IsLoginPageOpened(isRedesign))
            {
                this.rememberMe = rememberMe;

                loginPage.FillEmail(email);

                loginPage.FillPassword(password);

                loginPage.StaySignedIn(rememberMe, isRedesign);

                loginPage.ClickSignIn(isRedesign);

                if (isRedesign)
                {
                    spinner.WaitTopProgressBarToDisappearRedesign();
                }
                else
                {
                    spinner.WaitSpinnerToDisappear(SpinnerType.SignInButton);
                }

                return string.Empty;
            }

            return "Login failed: login page is not displayed";
        }

        public string Login(string email, bool rememberMe = false)
        {
            string userPass = SecretsHelper.GetUserPasswordByEmail(TestConfig, email) ?? string.Empty;

            string err = TryLogin(email, userPass, roles:null, rememberMe);

            return err;
        }

        public string Login(User user, bool rememberMe = false, bool isRedesign=false) => TryLogin(user.Email, user.Password, user.Roles, rememberMe, isRedesign);

        //public string Login_new(User user, bool rememberMe = false)
        //{
        //    if (!IsAlreadyLoggedIn())
        //    {
        //        browserSteps.OpenAppDeepLink(App.Taf);
        //    }

        //    return TryLogin(user.Email, user.Password, user.Roles, rememberMe);

        //}

        public string TryLogin(string email, string password, List<UserRole> roles=null, bool rememberMe = false, bool isRedesign=false)
        {
            List<string> errors = new List<string>();

            string userRoles = roles != null ? $" (Roles='{ string.Join(",", roles)})'" : string.Empty;

            if (IsAlreadyLoggedIn(isRedesign))
            {
                LogHelper.LogInfo(log, $"Login skipped: already logged in as '{email}'{userRoles}");
            }
            else
            {
                browserSteps.OpenAppDeepLink(App.Taf, isRedesign);

                string err = Login(email, password, rememberMe, isRedesign);

                errors.Add(err);

                bool isSpLogoPresent = WaitSpLogoPresent(isRedesign);

                err = isSpLogoPresent ? string.Empty : $"Login as '{email}' failed: {loginPage.GetErrorMessage(isRedesign)}";

                errors.Add(err);

                LogHelper.LogResult(log, $"Login as '{email}'{userRoles} successful", ErrorHelper.ConvertErrorsToString(errors));
            }

            return ErrorHelper.ConvertErrorsToString(errors);
        }

        public void LoginWithOtherEmailRetry(string email, string password, string alternativeEmail, bool isRedesign = false)
        {
            string err = TryLogin(email, password, isRedesign:isRedesign);

            if (!string.IsNullOrEmpty(err))
            {
                err = TryLogin(alternativeEmail, password, isRedesign:isRedesign);

                if (!string.IsNullOrEmpty(err))
                {
                    throw new PageNotLoadedException(err);
                }
            }
        }

        public void LoginWithOtherPasswordRetry(string email, string password, string alternativePass, bool isRedesign = false)
        {
            string err = TryLogin(email, password, isRedesign: isRedesign);

            if (!string.IsNullOrEmpty(err))
            {
                err = TryLogin(email, alternativePass, isRedesign:isRedesign);

                if (!string.IsNullOrEmpty(err))
                {
                    throw new PageNotLoadedException(err);
                }
            }
        }

        public void SaveLocalStorage()
        {
            if (rememberMe)
            {
                localStorage.SaveItem("vuex", BrowserStorage.Local);
            }
        }

        public void RestoreLocalStorage()
        {
            if (rememberMe)
            {
                localStorage.RestoreItem("vuex", BrowserStorage.Local);
            }
        }

        public void ClearLocalStorage()
        {
            localStorage.ClearLocalStorage();
        }

        public string CheckInvalidEmailOrPassError(string expectedErr, bool isRedesign=false)
        {
            string actualErr = loginPage.GetErrorMessage(isRedesign);

            string errMessage = expectedErr != actualErr
                ? $"'Login' page: Invalid error message: '{actualErr}' but expected '{expectedErr}'"
                : string.Empty;

            return errMessage;
        }

        public bool IsSpLogoPresent(bool isRedesign=false) => navigationBar.IsLogoPresent(isRedesign);

        public bool WaitSpLogoPresent(bool isRedesign=false) => UiWaitHelper.Wait(()=> navigationBar.IsLogoPresent(isRedesign), WaitConstants.FifteenSeconds);

        public bool IsAlreadyLoggedIn(bool isRedesign=false) => navigationBar.IsLogoPresent(isRedesign);

        public bool IsLoginPageOpened(bool isRedesign=false) => loginPage.IsAt(isRedesign);

        public void WaitPageLoad(bool isRedesign = false) => loginPage.WaitPageLoad(isRedesign);

        public void GoToRestorePasswordPage(bool isRedesign=false) => loginPage.ClickForgotYourPassLink(isRedesign);
    }
}
