using NLog;
using Taf.Api.Tests.JsonModels.ProfileModels;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.PageObjects;

namespace Taf.UI.Steps
{
    public class ProfileMenuSteps// : BaseSteps
    {
        private readonly ProfileMenu profileMenu;

        private readonly LoginPage loginPage;

        private readonly ProfileSettingsPage profileSettingsPage;

        private readonly ApiHelper apiHelper;

        private readonly BrowserStorageHelper browserStorage;

        private readonly ILogger log;

        public ProfileMenuSteps(ILogger logger)
        {
            profileMenu = new ProfileMenu();

            loginPage = new LoginPage();

            profileSettingsPage = new ProfileSettingsPage();

            apiHelper = new ApiHelper();

            browserStorage = new BrowserStorageHelper();

            log = logger;
        }

        public void OpenProfileMenu()
        {
            profileMenu.OpenProfileMenu();

            LogHelper.LogInfo(log, "Profile menu opened");
        }

        public void OpenProfileSettings(bool isRedesign=false)
        {
            profileMenu.OpenProfileMenu(isRedesign);

            profileMenu.SelectMenuItem("Profile settings", isRedesign);

            profileSettingsPage.WaitPageLoad(isRedesign); //to update for redesign (when spinners are added to redesign)

            LogHelper.LogInfo(log, "Profile settings page opened");
        }

        public void SignOut(bool isRedesign=false)
        {
            profileMenu.OpenProfileMenu(isRedesign);

            profileMenu.SelectMenuItem(isRedesign ? "Log out" : "Sign out", isRedesign);
        }

        public void TrySignOut(bool isRedesign = false)
        {
            if (profileMenu.IsProfileMenuButtonDisplayed(isRedesign))
            {
                LogHelper.LogInfo(log, "Trying to sign out");

                SignOut(isRedesign);

                UiWaitHelper.Wait(() => loginPage.IsAt(isRedesign), WaitConstants.HalfMinuteInSec); // wait for login page
            }
            else
            {
                LogHelper.LogInfo(log, "Profile menu button is not displayed");
            }
        }

        public string CheckProfileInitials(string actualInitials, string expectedInitials)
        {
            string err = actualInitials == expectedInitials
                ? string.Empty
                : $"Invalid user initials: {actualInitials}, expexted: {expectedInitials}";

            return err;
        }

        public string CheckProfileMenuInitials(string expectedInitials, bool isRedesign=false)
        {
            string actualInitials = profileMenu.GetUserInitials(isRedesign);

            string err = CheckProfileInitials(actualInitials, expectedInitials);

            return ErrorHelper.AddPrefixToError(err, "Profile menu: ");
        }

        public string CheckUserNameAndEmail(bool isRedesign = false)
        {
            profileMenu.OpenProfileMenu(isRedesign);

            string actualUserName = profileMenu.GetUserName(isRedesign);

            string actualUserEmail = profileMenu.GetUserEmail(isRedesign);

            Profile userProfile = apiHelper.GetProfile(browserStorage.GetAuthToken(BrowserStorage.Session, isRedesign));

            string expectedUserName = userProfile.User.FirstName + " " + userProfile.User.LastName;

            string expectedUserEmail = userProfile.User.Email;

            string err = actualUserName != expectedUserName
                ? $"Invalid user name: actual - '{actualUserName}' but expected - '{expectedUserName}'; "
                : string.Empty;

            return actualUserEmail != expectedUserEmail
                ? $"{err}Invalid user email: actual - '{actualUserEmail}' but expected - '{expectedUserEmail}'"
                : err;
        }
    }
}
