using Taf.UI.Core.Element;

namespace Taf.UI.PageObjects.Authoring
{
    public class JourneyContent : BasePage
    {
        private readonly string advisorStep = "//div[contains(@class,'flow-block-node')]";

        private readonly string stepTitle = "//div[@class='block-title-wrap']//span[@class='text-truncate']";

        private readonly string stepDescription = "//div[contains(@class,'block-description')]//span[@class='text-truncate']";

        private readonly string stepIcon = "//div[@class='icon-wrap']/span[contains(@class,'icon')]";

        private readonly string parameterCheckbox = "//div[@role='group']/div/input[@type='checkbox' and @class='custom-control-input']";

        private readonly string iDontMindCheckbox = "//div[contains(@class,'select-all')]/input[@type='checkbox' and @class='custom-control-input']";

        private readonly string parameterLabel = "/../label/span";

        private readonly string iDontMindLabel = "/../label//span[@class='text-truncate']";

        private readonly string parameterRadioButton = "//input[@type='radio' and @class='custom-control-input']"; //interval parameter

        private readonly string parameterRadioButtonLabel = "/../label/span";

        private string StepAtPosition(int position) => IndexedXpath(advisorStep, position);

        private string CheckboxAtPosition(int stepPosition, int checkboxPosition) => IndexedXpath(StepAtPosition(stepPosition) + parameterCheckbox, checkboxPosition);

        private string RadioButtonAtPosition(int stepPosition, int radioButtonPosition) => IndexedXpath(StepAtPosition(stepPosition) + parameterRadioButton, radioButtonPosition);

        public int GetStepCount() => new Element(advisorStep).Count;

        public string GetStepTitle(int stepPosition) => new Element(StepAtPosition(stepPosition) + stepTitle).Text;

        public string GetStepDescription(int stepPosition) => new Element(StepAtPosition(stepPosition) + stepDescription).Text;

        public string GetStepTypeRaw(int stepPosition) => new Element(StepAtPosition(stepPosition) + stepIcon).GetAttribute("class");

        public int GetParameterCount(int stepPosition) => new Element(StepAtPosition(stepPosition) + parameterCheckbox).Count;

        public bool IsCheckboxChecked(int stepPosition, int checkboxPosition) => new Checkbox(CheckboxAtPosition(stepPosition, checkboxPosition)).IsChecked;

        public bool IsIdontMindCheckboxChecked(int stepPosition) => new Checkbox(StepAtPosition(stepPosition) + iDontMindCheckbox).IsChecked;

        public string GetParameterTitle(int stepPosition, int checkboxPosition)
        {
            Element label = new Element(CheckboxAtPosition(stepPosition, checkboxPosition) + parameterLabel);

            Element iDontMind = new Element(CheckboxAtPosition(stepPosition, checkboxPosition) + iDontMindLabel);

            string title = label.Exists() ? label. Text : iDontMind.Text;

            return title;
        }

        public int GetRadioButtonCount(int stepPosition) => new Element(StepAtPosition(stepPosition) + parameterRadioButton).Count;

        public bool IsRadioButtonSelected(int stepPosition, int radioButtonPosition) => new Element(RadioButtonAtPosition(stepPosition, radioButtonPosition)).IsSelected;

        public string GetRadioButtonLabel(int stepPosition, int radioButtonPosition) => new Element(RadioButtonAtPosition(stepPosition, radioButtonPosition) + parameterRadioButtonLabel).Text;
    }
}
