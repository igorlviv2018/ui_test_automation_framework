using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.PageObjects;
using Taf.UI.PageObjects.Authoring;
using System.Collections.Generic;

namespace Taf.UI.Steps
{
    public class PublishingSummarySteps
    {
        private readonly EditorHeader editorHeader;

        private readonly EditorHeaderSteps editorHeaderSteps;

        private readonly ToastAlertSteps toastAlertSteps = new ToastAlertSteps();

        private readonly PublishSummaryModal modal = new PublishSummaryModal();

        public PublishingSummarySteps(App app)
        {
            editorHeader = new EditorHeader(app);
            
            editorHeaderSteps = new EditorHeaderSteps(app);
        }

        public bool OpenPublishSummaryModal()
        {
            editorHeaderSteps.SelectPublish();

            return modal.WaitModalAppeared();
        }

        public string CheckNoChannelsWarning() => !modal.IsNoChannelsWarningDisplayed()
            ? "No warning (no channels selected) in 'Omnichannel publish' block is displayed"
            : string.Empty;

        public string CheckGoToLocationLinkInChannelPublishProperties()
        {
            string err = string.Empty;

            modal.ClickGoToLocation();

            if (!modal.WaitModalDisappeared())
            {
                err = "Publish summary modal did not disappear after 'Go to Location' (Omnichannel publish block) clicked";
            }
            else
            {
                editorHeader.WaitTabIsActive("Location"); // wait Location tab loads

                if (!OpenPublishSummaryModal())
                {
                    err = "Publish summary modal did not appear after 'Publish' clicked";
                }
            }

            return err;
        }

        public string CheckGoToContentsLinkInFlowProperties()
        {
            string err = string.Empty;

            modal.ClickGoToContent();

            if (!modal.WaitModalDisappeared())
            {
                err = "Publish summary modal did not disappear after 'Go to Content' (Diagnostic flow properties block) clicked";
            }
            else
            {
                editorHeader.WaitTabIsActive("Content"); // wait Location tab loads

                if (!OpenPublishSummaryModal())
                {
                    err = "Publish summary modal did not appear after 'Publish' clicked";
                }
            }

            return err;
        }

        public string CheckIncompleteFlowError() => !modal.IsIncompleteFlowErrorDisplayed()
                ? "No error (An incomplete flow...) in 'Diagnostic flow properties' block is displayed"
                : string.Empty;

        public string CheckExpectedButtonsPresent(params string[] expectedButtonNames)
        {
            List<string> actualButtonNames = modal.GetButtonNames();

            List<string> expectedButtons = new List<string>();

            expectedButtons.AddRange(expectedButtonNames);

            string err = expectedButtonNames.Length != actualButtonNames.Count
                ? $"Invalid buttons: {string.Join(", ", actualButtonNames)} (expected buttons: {string.Join(", ", expectedButtonNames)})"
                : DataHelper.CompareListsIgnoreOrder(actualButtonNames, expectedButtons);

            return ErrorHelper.AddPrefixToError(err, "Expected buttons check failed: ");
        }

        public string CheckCancelButton()
        {
            ClickButton("Cancel");

            return !modal.WaitModalDisappeared()
                ? "Cancel button did not close the modal (or button is missing)"
                : string.Empty;
        }

        public string Publish()
        {
            string err = string.Empty;

            if (!IsButtonPresent("Start publishing"))
            {
                return "Start publishing button not present";
            }

            ClickButton("Start publishing");

            bool isSaveButtonActive = editorHeader.WaitSaveButtonIsActive(WaitConstants.TwoMinutesInSec);

            if (!isSaveButtonActive)
            {
                err = $"Save button did not become active after waiting for {WaitConstants.TwoMinutesInSec} s;";
            }

            err += toastAlertSteps.CheckAlertPopup(AlertStatus.Success);

            return err;
        }

        public void ClickButton(string buttonName)
        {
            List<string> buttonNames = modal.GetButtonNames();

            for (int i = 0; i < buttonNames.Count; i++)
            {
                if (buttonNames[i] == buttonName)
                {
                    modal.ClickButton(i + 1);
                }
            }
        }

        public bool IsButtonPresent(string buttonName)
        {
            List<string> buttonNames = modal.GetButtonNames();

            bool isPresent = false;

            for (int i = 0; i < buttonNames.Count; i++)
            {
                if (buttonNames[i] == buttonName)
                {
                    isPresent = true;
                }
            }

            return isPresent;
        }
    }
}
