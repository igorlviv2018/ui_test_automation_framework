using Taf.UI.Core.Element;
using System.Collections.Generic;

namespace Taf.UI.PageObjects
{
    public class ResetPasswordPage : BasePage
    {
        private readonly string spLogo = "//main//img[contains(@alt,'logo')]";

        private readonly string pageName = "//h2";

        private readonly string emailInput = "//div[contains(@class,'reset-password')]//input[@type='email']";

        private readonly string getResetLinkButton = "//div[contains(@class,'reset-password')]//button";

        private readonly string backToLoginButton = "//div[contains(@class,'reset-password')]//button[contains(@class,'lg')]";

        private readonly string justRememberedLink = "//div[contains(@class,'reset-password')]//a[contains(@href,'login')]";

        private readonly string errorText = "//form/div[contains(@class,'items-center')]//p";

        private readonly string successMessage = "//div[contains(@class,'page')]/div[contains(@class,'items-center')]//p";

        private readonly string resetPasswordButton = "//div[contains(@class,'expired-password-page')]/button";

        private readonly string passwordInput = "(//input)[1]";

        private readonly string repeatPasswordInput = "(//input)[2]";

        private readonly string createPasswordButton = "//div[contains(@class,'new-password-page')]/form/button";

        public Dictionary<string, string> GetExpectedPageElements() => new Dictionary<string, string>()
        {
            { "SP Logo", spLogo},
            { "Page caption", pageName},
            { "Email input", emailInput},
            { "Get reset link button", getResetLinkButton},
            { "Just remembered? link", justRememberedLink}
        };

        public Dictionary<string, string> GetExpectedEmailSentPageElements() => new Dictionary<string, string>()
        {
            { "SP Logo", spLogo},
            { "Page caption", pageName},
            { "Email sent success message", successMessage},
            { "Back to Login button", backToLoginButton}
        };

        public Dictionary<string, string> GetExpectedYourPasswordExpiredPageElements() => new Dictionary<string, string>()
        {
            { "SP Logo", spLogo},
            { "Page caption", pageName},
            { "Reset password button", resetPasswordButton}
        };

        public Dictionary<string, string> GetExpectedCreateNewPasswordPageElements() => new Dictionary<string, string>()
        {
            { "SP Logo", spLogo},
            { "Page caption", pageName},
            { "Password input", passwordInput},
            { "Repeat password input", repeatPasswordInput},
            { "Create button", createPasswordButton}
        };

        public string GetPageName() => new Element(pageName).Text;

        public void SetEmail(string text) => new Element(emailInput).SetText(text);

        public void ClickGetResetLinkButton() => new Element(getResetLinkButton).ClickIfExists();

        public void ClickJustRememberedLink() => new Element(justRememberedLink).ClickIfExists();

        public string GetErrorText() => new Element(errorText).Text;

        public bool IsErrorDisplayed() => new Element(errorText).IsDisplayed();

        public bool IsSuccessDisplayed() => new Element(successMessage).IsDisplayed();

        public void ClickBackToLoginButton() => new Element(backToLoginButton).ClickIfExists();

        public void ClickResetPasswordButton() => new Element(resetPasswordButton).ClickIfExists();
    }
}