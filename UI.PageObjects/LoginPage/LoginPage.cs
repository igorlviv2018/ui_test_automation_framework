using Taf.UI.Core.Constants;
using Taf.UI.Core.Element;

namespace Taf.UI.PageObjects
{
    public class LoginPage : BasePage
    {
        private readonly string email = "//input[@type='email']";

        private readonly string password = "//input[@type='password']";

        private readonly string staySignInCheckbox = "//input[@type='checkbox']";

        private readonly string staySignInCheckboxRedesign = "//i[@data-type='check']";

        private readonly string signInButton = "//button[@type='submit']";

        private readonly string signInButtonRedesign = "//div[@class='login-page']//button[1]";

        private readonly string forgotYourPass = "//a[@class='forgot-pass-link']";

        private readonly string forgotYourPassRedesign = "//a[contains(@class,'forgot-password')]";

        private readonly string welcomeText = "//div[contains(@class,'welcome-section')]/h1"; //"Welcome!" text on top of Login box

        private readonly string errorMessage = "//div[contains(@class,'alert-danger')]";

        private readonly string errorMessageRedesign = "//div[contains(@class, 'words')]/p";

        public void FillEmail(string loginEmail) => new Element(email).SetText(loginEmail);

        public void FillPassword(string pass) => new Element(password).SetText(pass);

        public void ClickSignIn(bool isRedesign=false) => new Element(isRedesign ? signInButtonRedesign : signInButton).ClickIfExists();

        public void ClickForgotYourPassLink(bool isRedesign=false) => new Element(isRedesign ? forgotYourPassRedesign : forgotYourPass).ClickIfExists();

        public void StaySignedIn(bool check, bool isRedesign=false) => new Checkbox(isRedesign? staySignInCheckboxRedesign : staySignInCheckbox).IsChecked = check;

        public string GetErrorMessage(bool isRedesign=false)
        {
            string errMessage = new Element(isRedesign ? errorMessageRedesign : errorMessage).Text;

            if (!isRedesign)
            { 
                errMessage = errMessage.Length > 3 ? errMessage[3..] : errMessage;
            }

            return errMessage;
        }

        public bool IsAt(bool isRedesign=false) => isRedesign ? ElementsDisplayed(signInButtonRedesign) : ElementsDisplayed(welcomeText, signInButton);

        public void WaitPageLoad(bool isRedesign = false)
        {
            if (isRedesign)
            {
                WaitPageLoad("Login page", signInButtonRedesign);
            }
            else
            {
                WaitPageLoad("Login page", welcomeText, signInButton);
            }
        }
    }
}
