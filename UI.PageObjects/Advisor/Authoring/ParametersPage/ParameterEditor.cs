using Taf.UI.Core.Element;
using Taf.UI.PageObjects.CommonPages.Authoring;

namespace Taf.UI.PageObjects.TafTest.Authoring
{
    public class ParameterEditor : BaseTable
    {
        private const string parameterEditor = "//div[contains(@class,'card-footer')]/div";

        private const string parameterEditorInEditMode = "//div[@class='p-3']";

        private readonly string titleInput = "/div//input[@id='titleInput']";

        private readonly string descriptionInput = "//div//input[@id='titleDescription']";

        private readonly string crossButton = "//div[contains(@class,'confirm-buttons')]/button[contains(@class,'btn-cross')]";

        private readonly string confirmButton = "//div[contains(@class,'confirm-buttons')]/button[contains(@class,'btn-confirm')]";

        private readonly string parameterTypeDropdown = parameterEditor + "//div[contains(@class,'dropdown')]/button";

        private readonly string parameterTypeMenuItem = parameterEditor + "//ul[contains(@class,'dropdown-menu')]//a[contains(text(),'{0}')]";

        public void SetTitle(string text) => new Element(parameterEditor + titleInput).SetText(text);

        public void SetDescription(string text) => new Element(parameterEditor + descriptionInput).SetText(text);

        public void ClickCrossButton() => new Element(parameterEditor + crossButton).ClickIfExists();

        public void ClickConfirmButton() => new Element(parameterEditor + confirmButton).ClickIfExists();

        //extract to base class?
        public bool IsDropdownMenuExpanded()
        {
            Element dropdown = new Element(parameterTypeDropdown);

            return dropdown.Exists() && dropdown.GetAttribute("aria-expanded") == "true";
        }

        public void ExpandDropdown()
        {
            if (!IsDropdownMenuExpanded())
            {
                new Element(parameterTypeDropdown).Click();
            }
        }

        public void ClickDropdownMenuItem(string itemName) => new Element(string.Format(parameterTypeMenuItem, itemName)).ClickIfExists();

        // Edit mode
        public string GetXpathForEditMode(string elementXpath, int rowPosition) => $"{RowAtPositionXpath(rowPosition + 1)}{parameterEditorInEditMode}{elementXpath}";

        public void SetTitle(string text, int rowPosition) => new Element(GetXpathForEditMode(titleInput, rowPosition)).SetText(text);

        public void SetDescription(string text, int rowPosition) => new Element(GetXpathForEditMode(descriptionInput, rowPosition)).SetText(text);

        public void ClickCrossButton(int rowPosition) => new Element(GetXpathForEditMode(crossButton, rowPosition)).ClickIfExists();

        public void ClickConfirmButton(int rowPosition) => new Element(GetXpathForEditMode(confirmButton, rowPosition)).ClickIfExists();
    }
}
