using Taf.UI.Core.Constants;
using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Taf.UI.PageObjects
{
    public class DxFlowDecisionBlock : ContentBlockBase
    {
        private readonly DropdownMenu dropdownInDecision;

        private readonly bool isRedesign;

        public DxFlowDecisionBlock(App app, bool isRedesign=false) : base(app, isRedesign)
        {
            dropdownInDecision = new DropdownMenu(app, isRedesign);

            locators = SetLocators(app, locatorsCommon, locatorsEmbed, locatorsTaf);

            this.isRedesign = isRedesign;
        }

        private readonly Dictionary<string, string> locators;

        private readonly Dictionary<string, string> locatorsCommon =

            new Dictionary<string, string>()
            {
                { "button", "//button[@type='button']"}
            };

        private readonly Dictionary<string, string> locatorsTaf =

            new Dictionary<string, string>()
            {
                { "image in decision button", "//div[contains(@class,'image-button')]/img"},
                { "button label", "//span[@class='label']"}, //button with image
                { "button name", "//span[@class='label']"} //button without image
            };

        private readonly Dictionary<string, string> locatorsEmbed =

            new Dictionary<string, string>()
            {
                { "image in decision button", "//div[contains(@class,'image-wrap')]/img"},
                { "button label", "//div[@class='label']"},
                { "button name", "/span"} //button without image
            };

        //private const string imageXpath = "//div[contains(@class,'image-wrap')]/img";

        //private const string buttonXpath = "//button[@type='button']";

        //private const string buttonLabelXpath = "//div[@class='label']";

        private string ButtonsXpath(int pathItemNum) => DxFlowPathItemAtPositionXpath(pathItemNum) + GetXpath("button", locators);

        private string ButtonByPositionXpath(int pathItemNum, int buttonPosition) => IndexedXpath(ButtonsXpath(pathItemNum), buttonPosition);

        private string ButtonImageByPositionXpath(int pathItemNum, int buttonPosition) =>
            ButtonByPositionXpath(pathItemNum, buttonPosition) + GetXpath("image in decision button", locators);

        private string ButtonLabelByPositionXpath(int pathItemNum, int buttonPosition) =>
            ButtonByPositionXpath(pathItemNum, buttonPosition) + GetXpath("button label", locators);

        private string ButtonNameByPositionXpath(int pathItemNum, int buttonPosition) =>
            ButtonByPositionXpath(pathItemNum, buttonPosition) + GetXpath("button name", locators);

        public string ClickButton(int pathItemNum, int buttonPos)
        {
            string err = string.Empty;

            Element button = new Element(ButtonByPositionXpath(pathItemNum, buttonPos));

            if (button.IsDisplayed(WaitConstants.CheckElementExistInSec))
            {
                button.ScrollToView();

                button.Click();
            }
            else
            {
                err = $"Decision at position - {pathItemNum}: expected button at position - {buttonPos} is not displayed";
            }

            return err;
        }

        public List<TafBranchData> GetButtons(int pathItemNum)
        {
            int buttonCount = new Element(ButtonsXpath(pathItemNum)).Count;

            List<TafBranchData> answersData = new List<TafBranchData>();

            for (int i = 0; i < buttonCount; i++)
            {
                Element buttonImage = new Element(ButtonImageByPositionXpath(pathItemNum, i + 1));

                bool isImageDisplayed = false;

                string answer;

                if (buttonImage.Exists())
                {
                    isImageDisplayed = buttonImage.IsImageVisible(); // IsImageDisplayed(buttonImage);

                    Element buttonLabel = new Element(ButtonLabelByPositionXpath(pathItemNum, i + 1));

                    answer = buttonLabel.Text;
                }
                else
                {
                    answer = new Element(ButtonNameByPositionXpath(pathItemNum, i + 1)).Text;
                }

                answersData.Add(new TafBranchData()
                {
                    Answer = answer,
                    HasImage = isImageDisplayed,
                    IsImageDisplayed = isImageDisplayed
                });
            }

            return answersData;
        }

        public List<string> GetPressedButtonNames(int pathItemNum) =>
            new Element(ButtonsXpath(pathItemNum)).FindElements()
                .Where(x => x.GetAttribute(isRedesign ? "pressed" : "aria-pressed") == "true")
                .Select(x => x.Text).ToList();

        public int GetPressedButtonPosition(int pathItemNum) =>
            new Element(ButtonsXpath(pathItemNum)).FindElements()
                .FindIndex(x => x.GetAttribute(isRedesign ? "pressed" : "aria-pressed") == "true");

        public void SelectDropdownMenuItem(int pathItemNum, int menuItemPos)
        {
            dropdownInDecision.BaseXpath = DxFlowPathItemAtPositionXpath(pathItemNum);

            dropdownInDecision.SelectMenuItem(menuItemPos);
        }

        public void OpenDropdownMenu(int pathItemNum)
        {
            dropdownInDecision.BaseXpath = DxFlowPathItemAtPositionXpath(pathItemNum);

            dropdownInDecision.OpenMenu();
        }

        //del?
        public void CloseDropdownMenu(int pathItemNum)
        {
            dropdownInDecision.BaseXpath = DxFlowPathItemAtPositionXpath(pathItemNum);

            dropdownInDecision.CloseMenu();
        }

        public List<TafBranchData> GetDropdownMenuItems(int pathItemNum)
        {
            dropdownInDecision.BaseXpath = DxFlowPathItemAtPositionXpath(pathItemNum);

            return dropdownInDecision.GetMenuItems().Select(i => i.ToBranchData()).ToList();
        }

        public TafBranchData GetSelectedDropdownItem(int pathItemNum)
        {
            dropdownInDecision.BaseXpath = DxFlowPathItemAtPositionXpath(pathItemNum);

            DropdownMenuItem selectedItem = dropdownInDecision.GetSelectedDropdownMenuItem();

            return selectedItem?.ToBranchData();
        }

        public DxFlowDecisionBlockData GetActualDecisionBlockData(int pathItemNum, bool isButtonView)
        {
            DxFlowDecisionBlockData data = new DxFlowDecisionBlockData();

            if (isButtonView)
            {
                List<TafBranchData> buttonsData = GetButtons(pathItemNum);

                data.AnswersData = buttonsData;

                int pressedButtonPosition = GetPressedButtonPosition(pathItemNum);

                data.SelectedAnswerData = pressedButtonPosition > -1 ? buttonsData[pressedButtonPosition] : null;
            }
            else //dropdown view
            {
                data.SelectedAnswerData = GetSelectedDropdownItem(pathItemNum);
            }

            return data;
        }
    }
}
