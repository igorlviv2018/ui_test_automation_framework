using Taf.UI.Core.Constants;
using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Models;

namespace Taf.UI.PageObjects
{
    public class ToastAlert : BasePage
    {
        private const string alertTitle = "//div[@id='mini-toastr']//div[contains(@class, 'mini-toastr-notification__title')]";

        private const string alertMessage = "//div[@id='mini-toastr']//div[contains(@class, 'mini-toastr-notification__message')]";

        private const string alert = "//div[@id='mini-toastr']/div[contains(@class, 'mini-toastr__notification')]";

        private const string alertTitleRedesign = "//div[@role='alert']/div/h3";

        private const string alertMessageRedesign = "//div[@role='alert']/div/p";

        private const string alertRedesign = "//div[contains(@class,'toast--')]";

        private readonly string firstAlert = $"({alert})[last()]"; // alert that appeared first, i.e. earliest (if multiple alerts are present)

        private readonly string firstAlertTitle = $"({alertTitle})[last()]";

        private readonly string firstAlertMessage = $"({alertMessage})[last()]";

        private readonly string firstAlertRedesign = $"({alertRedesign})[last()]"; // alert that appeared first, i.e. earliest (if multiple alerts are present)

        private readonly string firstAlertTitleRedesign = $"({alertTitleRedesign})[last()]";

        private readonly string firstAlertMessageRedesign = $"({alertMessageRedesign})[last()]";

        public AlertInfo GetAlertInfo(bool isRedesign = false)
        {
            Element recentAlert = new Element(isRedesign ? firstAlertRedesign : firstAlert);

            Element title = new Element(isRedesign ? firstAlertTitleRedesign : firstAlertTitle);

            Element message = new Element(isRedesign ? firstAlertMessageRedesign : firstAlertMessage);

            AlertInfo alertInfo = new AlertInfo();

            if (recentAlert.Exists(WaitConstants.CheckElementExistInSec))
            {
                alertInfo.Status = recentAlert.GetAttribute("class").Contains("success") ? AlertStatus.Success : AlertStatus.Failed;

                alertInfo.Title = title.Text;
                
                alertInfo.Message = message.Text;

                alertInfo.IsDisplayed = true;
            }

            return alertInfo;
        }

        public void ClickAlert() => new Element(firstAlertTitle).ClickIfDisplayed();

        public bool IsAlertDisplayed(bool isRedesign = false) => new Element(isRedesign ? firstAlertRedesign : firstAlert).IsDisplayedSafe();

        public bool IsAlertRendered(bool isRedesign = false) => !new Element(isRedesign ? firstAlertRedesign : firstAlert)
            .GetAttribute("class").Contains("bounce-enter");
    }
}
