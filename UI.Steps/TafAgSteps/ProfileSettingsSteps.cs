using NLog;
using Taf.Api.Tests.JsonModels.ProfileModels;
using Taf.Api.Tests.JsonModels.UserModels;
using Taf.Api.Tests.Models;
using Taf.Api.Tests.RestSharp;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.PageObjects;
using System.Collections.Generic;

namespace Taf.UI.Steps
{
    public class ProfileSettingsSteps// : BaseSteps
    {
        private readonly ProfileSettingsPage profileSettingsPage;

        private readonly ApiHelper apiHelper;

        private readonly HttpHelper httpHelper;

        private readonly BrowserStorageHelper browserStorage;

        private readonly ToastAlertSteps toastAlertSteps;

        private readonly ProfileMenuSteps profileMenuSteps;

        private readonly ILogger log;

        public ProfileSettingsSteps(ILogger logger)
        {
            log = logger;

            profileSettingsPage = new ProfileSettingsPage();

            apiHelper = new ApiHelper();

            httpHelper = new HttpHelper();

            browserStorage = new BrowserStorageHelper();

            toastAlertSteps = new ToastAlertSteps();

            profileMenuSteps = new ProfileMenuSteps(log);
        }

        public string UpdateProfile(bool isRedesign=false) => UpdateProfile(CommonConstants.ProfileUpdateNewFirstName,
                CommonConstants.ProfileUpdateNewLastName,
                CommonConstants.ProfileUpdateNewEmail, isRedesign);

        public string UpdateProfile(string newFirstName, string newLastName, string newEmail, bool isRedesign=false)
        {
            profileSettingsPage.SetFirstName(newFirstName, isRedesign);

            profileSettingsPage.SetLastName(newLastName, isRedesign);

            profileSettingsPage.SetEmail(newEmail, isRedesign);

            LogHelper.LogInfo(log, $"New '{newFirstName}' first name, new '{newLastName}', new '{newEmail}' email set");

            bool isUpdateButtonClicked = profileSettingsPage.ConfirmProfileUpdate(isRedesign);

            LogHelper.LogInfo(log, "Update profile button clicked");

            return isUpdateButtonClicked ? CheckUpdateProfileAlert(isRedesign) : string.Empty;
        }

        public string CheckUpdateProfileAlert(bool isRedesign = false)
        {
            toastAlertSteps.WaitAlertIsRendered(isRedesign);

            string err = toastAlertSteps.CheckAlertPopup(AlertStatus.Success, MessageConstants.ProfileUpdateSuccessMessage, isRedesign);

            toastAlertSteps.WaitAlertToDisappear(isRedesign);

            LogHelper.LogResult(log, "Update profile alert checked", err);

            return err;
        }

        public string CheckProfileSettingsPageInitials(string expectedInitials, bool isRedesign=false)
        {
            string actualInitials = profileSettingsPage.GetUserInitials(isRedesign);

            string err = profileMenuSteps.CheckProfileInitials(actualInitials, expectedInitials);

            return ErrorHelper.AddPrefixToError(err, "Profile settings page: ");
        }

        /// <summary>
        /// Check initials on 'Profile menu' button and on the Profile settings page
        /// </summary>
        /// <param name="expectedUserData"></param>
        /// <returns></returns>
        public string CheckProfileInitials(UserData expectedUserData, bool isRedesign=false)
        {
            List<string> errors = new List<string>();

            string expectedInitials = DataHelper.GetUserInitials(expectedUserData.FirstName, expectedUserData.LastName);

            string err = CheckProfileSettingsPageInitials(expectedInitials, isRedesign);

            ErrorHelper.AddToErrorList(errors, err);

            err = profileMenuSteps.CheckProfileMenuInitials(expectedInitials, isRedesign);

            ErrorHelper.AddToErrorList(errors, err);

            return ErrorHelper.ConvertErrorsToString(errors, "User initials check failed: ");
        }

        public string CheckProfileBeforeUpdate(bool isRedesign=false)
        {
            UserData actualUserData = GetActualUserData(isRedesign);

            UserData expectedUserData = GetExpectedUserDataBeforeProfileUpdate();

            string err = CheckProfile(actualUserData, expectedUserData, isRedesign);

            err = ErrorHelper.AddPrefixToError(err, "Profile check before update: ");

            LogHelper.LogResult(log, "Profile before update checked", err);

            return err;
        }

        /// <summary>
        /// Get data from API and compare with test data (from constants)
        /// </summary>
        /// <returns></returns>
        public string CheckProfileUpdated(bool isRedesign=false)
        {
            UserData actualUserData = GetActualUserDataViaApi(isRedesign);

            UserData expectedUserData = GetExpectedUserDataAfterProfileUpdate();

            string err = CheckProfile(actualUserData, expectedUserData, isRedesign);

            err = ErrorHelper.AddPrefixToError(err, "Profile check after update: ");

            LogHelper.LogResult(log, "Profile Update checked", err);

            return err;
        }

        public string CheckProfile(UserData actualUserData, UserData expectedUserData, bool isRedesign=false)
        {
            List<string> errors = new List<string>();

            string err = CheckProfileInitials(expectedUserData, isRedesign);

            ErrorHelper.AddToErrorList(errors, err);

            if (actualUserData.FirstName != expectedUserData.FirstName)
            {
                err = $"Actual first name: '{actualUserData.FirstName}' but expected: '{expectedUserData.FirstName}'";

                ErrorHelper.AddToErrorList(errors, err);
            }

            if (actualUserData.LastName != expectedUserData.LastName)
            {
                err = $"Actual last name: '{actualUserData.LastName}' but expected: '{expectedUserData.LastName}'";

                ErrorHelper.AddToErrorList(errors, err);
            }

            if (actualUserData.Email != expectedUserData.Email)
            {
                err = $"Actual email: '{actualUserData.Email}' but expected: '{expectedUserData.Email}'";

                ErrorHelper.AddToErrorList(errors, err);
            }

            return ErrorHelper.ConvertErrorsToString(errors);
        }

        public string RestoreProfile(bool isRedesign=false)
        {
            string authToken = browserStorage.GetAuthToken(isRedesign:isRedesign);

            string err = apiHelper.RestoreProfile(authToken);

            LogHelper.LogResult(log, "Profile restored (to original test data)", err);

            return err;
        }

        public UserData GetActualUserData(bool isRedesign = false) => new UserData()
            {
                FirstName = profileSettingsPage.GetFirstName(isRedesign),
                LastName = profileSettingsPage.GetLastName(isRedesign),
                Email = profileSettingsPage.GetEmail(isRedesign)
            };

        public UserData GetActualUserDataViaApi(bool isRedesign=false)
        {
            Profile profile = apiHelper.GetProfile(browserStorage.GetAuthToken(isRedesign:isRedesign));

            return new UserData()
            {
                FirstName = profile.User.FirstName,
                LastName = profile.User.LastName,
                Email = profile.User.Email
            };
        }

        public UserData GetExpectedUserDataBeforeProfileUpdate() => new UserData()
        {
            FirstName = CommonConstants.ProfileUpdateOriginalFirstName,
            LastName = CommonConstants.ProfileUpdateOriginalLastName,
            Email = CommonConstants.ProfileUpdateOriginalEmail
        };

        public UserData GetExpectedUserDataAfterProfileUpdate() => new UserData()
            {
                FirstName = CommonConstants.ProfileUpdateNewFirstName,
                LastName = CommonConstants.ProfileUpdateNewLastName,
                Email = CommonConstants.ProfileUpdateNewEmail
            };

        public void UpdatePassword(string currentPassword, string newPassword, string newPasswordConfirm, bool isRedesign= false)
        {
            if (isRedesign)
            {
                profileSettingsPage.OpenPasswordForm();
            }

            profileSettingsPage.SetCurrentPassword(currentPassword, isRedesign);

            profileSettingsPage.SetNewPassword(newPassword, isRedesign);

            profileSettingsPage.SetNewPasswordConfirm(newPasswordConfirm, isRedesign);

            profileSettingsPage.ConfirmPasswordUpdate(isRedesign);
        }

        public void SetLang(string id)
        {
            profileSettingsPage.SelectUiLangById(id);

            //profileSettingsPage.SetNewPassword(newPassword);

            //profileSettingsPage.SetNewPasswordConfirm(newPasswordConfirm);

            //profileSettingsPage.ConfirmPasswordUpdate();
        }

        public string CheckEmailUpdated(string newEmail, string password)
        {
            LoginResponseData loginResponse = httpHelper.GetLoginResponseData(newEmail, password);// apiHelper.Login(email, newPass);

            string err = string.Empty;

            if (loginResponse.Errors.Count > 0)
            {
                err = $"Email update - failed to login with new email ('{newEmail}'): " + ErrorHelper.ConvertErrorsToString(loginResponse.Errors);
            }

            LogHelper.LogResult(log, "Email update checked", err);

            return err;
        }

        /// <summary>
        /// Check if password update is successful (try to login via API using the new password)
        /// </summary>
        /// <returns></returns>
        public string CheckPasswordUpdated(string email, string newPass)
        {
            LoginResponseData loginResponse = httpHelper.GetLoginResponseData(email, newPass);// apiHelper.Login(email, newPass);

            string err = string.Empty;

            if (loginResponse.Errors.Count > 0)
            {
                err = "Password update failed: " + ErrorHelper.ConvertErrorsToString(loginResponse.Errors);
            }

            LogHelper.LogResult(log, "Password update checked", err);

            return err;
        }

        public string RestorePassword(string currentPass, string newPass, bool isRedesign= false)
        {
            string authToken = browserStorage.GetAuthToken(isRedesign:isRedesign);

            string err = apiHelper.RestorePassword(currentPass, newPass, authToken);

            LogHelper.LogResult(log, "Password restored", err);

            return err;
        }

        public string CheckErrorMessage(string expectedMessage, bool isRedesign=false)
        {
            UiWaitHelper.Wait(() => profileSettingsPage.GetErrorMessage(isRedesign) == expectedMessage, WaitConstants.TwoSeconds);

            string actualMessage = profileSettingsPage.GetErrorMessage(isRedesign);

            if (!isRedesign)
            {
                actualMessage = actualMessage.Length > 1 ? actualMessage[1..] : actualMessage;

                actualMessage = actualMessage.TrimStart('\r', '\n');
            }

            return actualMessage != expectedMessage
                ? $"Invalid error message: '{actualMessage}' but expected - '{expectedMessage}'"
                : string.Empty;
        }
    }
}
