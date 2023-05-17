using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.PageObjects;
using Taf.UI.PageObjects.Authoring;
using System;
using System.Collections.Generic;

namespace Taf.UI.Steps
{
    public class EditorHeaderSteps
    {
        private readonly EditorHeader editorHeader;

        private readonly Spinner spinner;

        private readonly ToastAlertSteps toastAlertSteps = new ToastAlertSteps();

        public EditorHeaderSteps(App app)
        {
            editorHeader = new EditorHeader(app);

            spinner = new Spinner(App.Taf);
        }

        public string EditorSave()
        {
            editorHeader.ClickSaveButton();

            List<string> errors = new List<string>();

            string err = toastAlertSteps.CheckAlertPopup(AlertStatus.Success);

            errors.Add(ErrorHelper.AddPrefixToError(err, "Save operation failed: "));

            spinner.WaitSpinnerToDisappear(SpinnerType.TopProgressBar, 3);

            bool isSaveButtonActive = editorHeader.WaitSaveButtonIsActive();

            if (!isSaveButtonActive)
            {
                errors.Add("'Save' button did not become active");
            }

            return ErrorHelper.ConvertErrorsToString(errors);
        }

        public void SelectPublish()
        {
            editorHeader.WaitSaveButtonIsActive();

            if (!editorHeader.IsDropdownMenuExpanded())
            {
                editorHeader.ExpandDropdown();
            }

            editorHeader.ClickDropdownMenuItem("Publish");
        }

        public void SelectPreview() => editorHeader.ClickPreviewButton();

        public void OpenEditorTab(string tabName)
        {
            if (!editorHeader.IsTabActive(tabName))
            {
                editorHeader.ClickArticleTab(tabName);

                spinner.WaitSpinnerToDisappear(SpinnerType.TopProgressBar);

                editorHeader.WaitTabIsActive(tabName);
            }
        }

        public void CloseEditor()
        {
            editorHeader.ClickCloseButton();

            spinner.WaitSpinnerToDisappear(SpinnerType.TopProgressBar);
        }

        public void GoBackToEditorFromPreview()
        {
            editorHeader.ClickBackToEditorButton();

            spinner.WaitSpinnerToDisappear(SpinnerType.TopProgressBar);
        }
    }
}
