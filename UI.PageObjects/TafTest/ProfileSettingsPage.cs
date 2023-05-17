using Taf.UI.Core.Element;

namespace Taf.UI.PageObjects
{
    public class ProfileSettingsPage : BasePage
    {
        private const string commonXpath = "//li[contains(@class,'profile-menu')]/ul";

        private string pageName = "//main/h1";

        private string userInitials = "//div[@class='row']//em[contains(@class,'user-initials')]";

        private readonly string userInitialsRedesign = "//div[@class='profile-initials']/span";

        private string userFirstNameInput = "//input[@id='firstNameInput']";

        private string userFirstNameInputRedesign = "(//form[@class='my-5'][1]//input)[1]";

        private string userLastNameInput = "//input[@id='lastNameInput']";

        private string userLastNameInputRedesign = "(//form[@class='my-5'][1]//input)[2]";

        private string userEmail = "//input[@id='emailAddressInput']";

        private string userEmailRedesign = "(//form[@class='my-5'][1]//input)[3]";

        private string uiLangSelector = "//select[@id='uiLangSelect']/option[contains(text(),'{0}')]";

        private string uiLangById = "//select[@id='uiLangSelect']/option[contains(@value,'{0}')]";

        private string contentLangSelector = "//select[@id='contentLangSelect']/option[contains(text(),'{0}')]";

        private string updateProfileSpinner = "//section[contains(@class,'my-3')]//button[contains(@class,'btn-primary')]/span[@role='status']";

        private string updateProfileBtn = "//section[contains(@class,'my-3')]//button[contains(@class,'btn-primary')]";

        private readonly string updateProfileBtnRedesign = "//form[@class='my-5'][1]//button";

        private string currentPassword = "//input[@id='currentPassInput']";

        private string currentPasswordRedesign = "(//form[@class='my-5'][3]//input)[1]";

        private string newPassword = "//input[@id='newPassInput']";

        private string newPasswordRedesign = "(//form[@class='my-5'][3]//input)[2]";

        private string newPasswordConfirm = "//input[@id='confirmPassInput']";

        private string newPasswordConfirmRedesign = "(//form[@class='my-5'][3]//input)[3]";

        private string updatePasswordBtn = "//form[contains(@class,'pt-2')]//button[contains(@class,'btn-primary')]";

        private readonly string updatePasswordBtnRedesign = "//form[@class='my-5'][3]/button";

        private string updatePasswordSpinner = "//form[contains(@class,'pt-2')]//button[contains(@class,'btn-primary')]/span[@role='status']";

        private string notificationsCount = "//a[@role='menuitem']//span[contains(@class,'badge')]";

        private string errorMessage = "//div[@role='alert']";

        private string errorMessageRedesign = "//form[@class='my-5'][3]//div[contains(@class,'break-words')]/p";

        private readonly string passwordFormOpener = "//div[contains(@class,'items-center')]/button";

        public void SelectUiLang(string uiLang)
        {
            Element item = new Element(string.Format(uiLangSelector, uiLang));

            item.ClickIfExists();
        }

        public void SelectUiLangById(string uiLangId)
        {
            Element item = new Element(string.Format(uiLangById, uiLangId));

            item.ClickIfExists();
        }

        //redisign only
        public bool IsButtonEnabled(Element button) => button.GetAttribute("disabled") == null;

        public bool ConfirmProfileUpdate(bool isRedesign= false)
        {
            Element updateProfileButton = new Element(isRedesign ? updateProfileBtnRedesign : updateProfileBtn);

            bool isButtonClicked = false;

            if (IsButtonEnabled(updateProfileButton))
            {
                updateProfileButton.Click();

                isButtonClicked = true;
            }

            if (!isRedesign)
            {
                Element spinner = new Element(updateProfileSpinner);

                spinner.WaitTillDisappeared("User profile update button spinner");
            }

            return isButtonClicked;
        }

        public void ConfirmPasswordUpdate(bool isRedesign= false)
        {
            Element updatePasswordButton = new Element(isRedesign ? updatePasswordBtnRedesign : updatePasswordBtn);

            updatePasswordButton.Click();

            if (!isRedesign)
            {
                Element spinner = new Element(updatePasswordSpinner);

                spinner.WaitTillDisappeared("User password update button spinner");
            }
        }

        public void SetFirstName(string firstName, bool isRedesign = false) =>
            new Element(isRedesign ? userFirstNameInputRedesign : userFirstNameInput).SetText(firstName);

        public void SetLastName(string lastName, bool isRedesign = false) =>
            new Element(isRedesign ? userLastNameInputRedesign : userLastNameInput).SetText(lastName);

        public void SetEmail(string email, bool isRedesign = false) => new Element(isRedesign ? userEmailRedesign : userEmail).SetText(email);

        public void SetCurrentPassword(string currentPass, bool isRedesign = false) =>
            new Element(isRedesign ? currentPasswordRedesign : currentPassword).SetText(currentPass, clearWithCtrlADel: isRedesign);

        public void SetNewPassword(string newPass, bool isRedesign = false) =>
            new Element(isRedesign ? newPasswordRedesign : newPassword).SetText(newPass, clearWithCtrlADel: isRedesign);

        public void SetNewPasswordConfirm(string newPassConfirm, bool isRedesign = false) =>
            new Element(isRedesign ? newPasswordConfirmRedesign : newPasswordConfirm).SetText(newPassConfirm, clearWithCtrlADel: isRedesign);

        public string GetFirstName(bool isRedesign= false) => new Element(isRedesign ? userFirstNameInputRedesign : userFirstNameInput).InputText;

        public string GetLastName(bool isRedesign= false) => new Element(isRedesign ? userLastNameInputRedesign : userLastNameInput).InputText;

        public string GetEmail(bool isRedesign= false) => new Element(isRedesign ? userEmailRedesign : userEmail).InputText;

        public string GetErrorMessage(bool isRedesign= false)
        {
            Element error = new Element(isRedesign ? errorMessageRedesign : errorMessage);

            return error.Exists(2) ? error.Text : string.Empty;
        }

        public string GetUserInitials(bool isRedesign= false) => new Element(isRedesign ? userInitialsRedesign: userInitials).Text;

        public void WaitPageLoad(bool isRedesign=false)
        {
            WaitPageLoad("Profile settings", pageName, isRedesign ? userInitialsRedesign : userInitials);
        }

        // redesign only
        public void OpenPasswordForm()
        {
            Element button = new Element(passwordFormOpener);

            if (button.Text.Contains("Change"))
            {
                button.ClickIfExists();
            }
        }
    }
}
