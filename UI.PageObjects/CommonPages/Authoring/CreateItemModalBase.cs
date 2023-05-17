using Taf.UI.Core.Element;

namespace Taf.UI.PageObjects.CommonPages.Authoring
{
    public class CreateItemModalBase
    {
        private const string modalBody = "//div[contains(@class,'modal-body')]";

        private readonly string titleInput = $"{modalBody}//div[contains(@class,'required')]//input[@id='titleInput']";

        private readonly string descriptionInput = $"{modalBody}//div[contains(@class,'required')]//textarea[@id='descriptionInput']";

        private readonly string workNoteInput = $"{modalBody}//textarea[@id='noteInput']";

        private const string createButton = modalBody + "//section/button";

        private readonly string createButtonSpinner = $"{createButton}/span[contains(@class,'spinner-border')]";

        private readonly string spinnerInModal = $"{modalBody}//span[contains(@class,'spinner-border')]"; // spinner in the center of page disabling the content of page when run

        public void SetTitle(string text) => new Element(titleInput).SetText(text);

        public void SetDescription(string text) => new Element(descriptionInput).SetText(text);

        public void SetWorkNote(string text) => new Element(workNoteInput).SetText(text);

        public void ClickCreateButton() => new Element(createButton).ClickIfExists();

        public bool IsSpinnerInCreateButtonDisappeared() => Spinner.WaitSpinnerToDisappear("Create button spinner", createButtonSpinner, false);

        public bool IsSpinnerInModalDisappeared() => Spinner.WaitSpinnerToDisappear("Modal window spinner", spinnerInModal, false);
    }
}
