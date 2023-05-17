using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models;
using Taf.UI.PageObjects;
using System.Collections.Generic;

namespace Taf.UI.Steps
{
    public class ToastAlertSteps
    {
        private readonly ToastAlert toastAlert = new ToastAlert();

        public string CheckAlertPopup(AlertStatus expectedAlertStatus, string expectedMessage = "", bool isRedesign=false)
        {
            AlertInfo alertInfo = toastAlert.GetAlertInfo(isRedesign);

            List<string> errors = new List<string>();

            if (!alertInfo.IsDisplayed)
            {
                string err = !string.IsNullOrEmpty(expectedMessage) ? $"with '{expectedMessage}' message " : string.Empty;

                errors.Add($"Alert (with '{expectedAlertStatus}' status) {err}did not appear");
            }
            else
            {
                //toastAlert.ClickAlert();
            }

            if (alertInfo.IsDisplayed && !string.IsNullOrEmpty(expectedMessage) && alertInfo.Message != expectedMessage)
            {
                errors.Add($"Invalid alert message: {alertInfo.Message}, expected: {expectedMessage}");
            }

            if (alertInfo.IsDisplayed && alertInfo.Status != expectedAlertStatus)
            {
                errors.Add($"Invalid alert status: {alertInfo.Status}, expected: {expectedAlertStatus} (actual alert message: {alertInfo.Message})");
            }

            return ErrorHelper.ConvertErrorsToString(errors);
        }

        public void WaitAlertToDisappear(bool isRedesign = false) => UiWaitHelper.Wait(() => !toastAlert.IsAlertDisplayed(isRedesign), WaitConstants.FifteenSeconds);

        public void WaitAlertToAppear(bool isRedesign = false) => UiWaitHelper.Wait(() => toastAlert.IsAlertDisplayed(isRedesign), WaitConstants.FifteenSeconds);

        public void WaitAlertIsRendered(bool isRedesign = false) =>
            UiWaitHelper.Wait(() => toastAlert.IsAlertDisplayed(isRedesign) && toastAlert.IsAlertRendered(isRedesign), WaitConstants.FifteenSeconds);
    }
}
