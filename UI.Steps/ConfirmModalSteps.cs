using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.PageObjects;
using Taf.UI.PageObjects.Authoring;

namespace Taf.UI.Steps
{
    public class ConfirmModalSteps
    {
        private readonly ConfirmModal confirmModal = new ConfirmModal();

        private readonly Spinner spinner = new Spinner(App.Taf);

        public void ClickButtonInConfirmModal(string buttonName)
        {
            confirmModal.ClickButton(buttonName);

            //if (!confirmModal.WaitModalDisappeared()) //workaruond as sometimes button click is not handled
            //{
            //    confirmModal.ClickButton(operation.ToString());
            //    LogHelper.LogInfo(log, $"'{operation}' button clicked second time (as first click was not handled)");
            //}

            spinner.WaitTopProgressBarToDisappear(WaitConstants.TwoSeconds);
        }

        public string CheckConfirmModal(string expectedTitle, string expectedMessage)
        {
            if (!confirmModal.WaitModalAppeared())
            {
                return "Operation confirm modal did not appear";
            }

            string actualTitle = confirmModal.GetTitle();

            string actualMessage = confirmModal.GetMessage();

            string err = string.Empty;

            if (actualTitle != expectedTitle)
            {
                err = $"Invalid title: {actualTitle} (expected: {expectedTitle});";
            }

            if (actualMessage != expectedMessage)
            {
                err = $"{err}Invalid message: {actualMessage} (expected: {expectedMessage});";
            }

            //LogHelper.LogResult(log, $"Modal for '{operation}' operation checked", err);

            return ErrorHelper.AddPrefixToError(err, $"Confirm modal: ");
        }

        public bool IsModalDisappeared() => confirmModal.WaitModalDisappeared();
    }
}
