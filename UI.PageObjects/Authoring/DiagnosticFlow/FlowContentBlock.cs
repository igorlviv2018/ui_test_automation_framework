using NLog;
using OpenQA.Selenium;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using System.Threading;

namespace Taf.UI.PageObjects
{
    public class FlowContentBlock : BasePage
    {
        private readonly string topLevelBlock = "//div[contains(@class,'diagnostic-flow-editor')]/div[contains(@class,'flow-blocks-holder')]/div[@class='collection-item']/div/div/div[contains(@class,'flow-block')]";

        private readonly string addBlockButton = "/../../div[contains(@class,'flow-blocks-connector')]//button";

        private readonly string addBlockButtonInBranch = "/div[contains(@class,'flow-blocks-holder')]//button";

        private readonly string blockDropdownItem = "/../button[@class='dropdown-item']//span[@class='icon-{0}']";

        private readonly string flowEditor = "//div[contains(@class,'diagnostic-flow-editor')]";

        private readonly string blockTitleInput = "/div[contains(@class,'block-header')]//input";

        private readonly string blockTitle = "/div[contains(@class,'block-header')]//span[contains(@class,'title')]/span[contains(@class,'text')]";

        private readonly string blockIcon = "/div[contains(@class,'block-header')]//div[contains(@class,'icon-wrap')]";

        private readonly string blockDescriptionEditIcon = "//div[contains(@class,'text-editor')]//span[contains(@class,'icon-edit')]";

        private readonly string blockDescriptionEditor = "/div[contains(@class,'block-header')]//div[contains(@class,'ql-editor')]";

        private readonly string branchTitleInput = "/div[contains(@class,'branch-top')]//input";

        private readonly string branchTitle = "/div[contains(@class,'branch-top')]//span[contains(@class,'title')]/span[contains(@class,'text')]";

        private readonly string blockType = "/div[@class='block-body']/div";

        private readonly string blockMenuButton = "/div[contains(@class,'block-header')]/div[contains(@class,'block-description')]/div[contains(@class,'block-menu')]/div[contains(@class,'dropdown')]/button";

        private readonly string blockMenuButtonInBranch = "/div[contains(@class,'branch-top')]/div[contains(@class,'block-menu')]/div[contains(@class,'dropdown')]/button";

        private readonly string dropdownMenuItem = "/../ul//button[@role='menuitem']/span[contains(@class,'{0}')]";

        private readonly string addImageButtonInBranch = "/div[contains(@class,'branch-image-wrap')]/div[contains(@class,'aspect')]";

        private readonly string radioButtonLabelInDecisionDropdown = "//div[@role='radiogroup']//label";

        private readonly string innerContentBlock = "//div[contains(@class,'content-tree')]/div[contains(@class,'content-tree-node')]/div[contains(@class,'content-block')]/div[contains(@class,'content-block')]";

        private readonly string buttonInProcessBlock = "//div[contains(@role,'group')]/button[@title='{0}']";

        private readonly ILogger log = LogManager.GetLogger("FlowContentBlock");

        public string AddBlockButton(bool isBranch = false) => isBranch ? addBlockButtonInBranch : addBlockButton;

        public string BlockDropdownMenuButton(bool isBranch) => isBranch ? blockMenuButtonInBranch : blockMenuButton;

        public string BlockTitle(bool isBranch) => isBranch ? branchTitle : blockTitle;

        public string TopLevelBlockAtPosition(int position) => IndexedXpath(topLevelBlock, position);

        public string InnerBlockInProcessAtPosition(string processXpath, int position) => $"({processXpath}{innerContentBlock})[{position}]";

        public bool InnerBlockInProcessAtPositionExists(string processXpath, int position) =>
            new Element(InnerBlockInProcessAtPosition(processXpath, position)).Exists(WaitConstants.ImplicitWaitInSec);

        public void ClickAddBlockButton(string blockXpath, bool isBranch = false)
        {
            Element plusButton = new Element(blockXpath + AddBlockButton(isBranch));

            //plusButton.ScrollToView();

            plusButton.ClickIfExists();

            bool isExpanded = IsAddBlockDropdownExpanded(blockXpath, isBranch);

            if (!isExpanded) //workaround as sometimes click is not handled
            {
                plusButton.ClickIfExists();

                log.Info("Second click on plus button");
            }
        }

        public void ClickAddBlockButtonInStartBlock() => new Element(flowEditor + addBlockButtonInBranch).ClickIfExists();

        public void SelectAddBlockMenuItem(string blockXpath, string menuItemName, bool isBranch) =>
            new Element(blockXpath + AddBlockButton(isBranch) + string.Format(blockDropdownItem, menuItemName)).ClickIfExists();

        public void SelectAddBlockMenuItemInStartBlock(string menuItemName) => SelectAddBlockMenuItem("/", menuItemName, true);

        public bool IsAddBlockDropdownExpanded(string blockXpath, bool isBranch) =>
            UiWaitHelper.Wait(() => new Element(blockXpath + AddBlockButton(isBranch)).GetAttribute("aria-expanded") == "true", WaitConstants.TwoSeconds);

        public void ClickTitle(string blockXpath)
        {
            Element titleInput = new Element(blockXpath + blockTitleInput);

            try
            {
                titleInput.Click();
            }
            catch (ElementClickInterceptedException)
            {
                Thread.Sleep(100); //workaround as sometimes exception is raised

                titleInput.Click();
            }
        }

        public void SetTitle(string blockXpath, string title) => new Element(blockXpath + blockTitleInput).SetText(title);

        public void ClickBlockIcon(string blockXpath)
        {
            Element icon = new Element(blockXpath + blockIcon);

            icon.ScrollToView();

            icon.ClickIfExists();
        }

        public void ClickBlockMenu(string blockXpath, bool isBranch) => new Element(blockXpath + BlockDropdownMenuButton(isBranch)).ClickIfExists();

        public void SetBranchTitle(string blockXpath, string title) => new Element(blockXpath + branchTitleInput).SetText(title);

        //new description code
        public void ClickDescriptionEditIcon(string blockXpath) => new Element(blockXpath + blockDescriptionEditIcon).ClickIfExists();

        public bool IsDescriptionEditable(string blockXpath) => new Element(blockXpath + blockDescriptionEditor).GetAttribute("contenteditable") == "true";

        public void SetDescriptionText(string blockXpath, string description) => new Element(blockXpath + blockDescriptionEditor).SetText(description);

        public bool WaitDescriptionEditable(string blockXpath) =>
            UiWaitHelper.Wait(() => IsDescriptionEditable(blockXpath), WaitConstants.ImplicitWaitInSec);

        public string GetBlockType(string blockXpath) => new Element(blockXpath + blockType).GetAttribute("class");

        public string GetTitle(string blockXpath, bool isBranch) => new Element(blockXpath + BlockTitle(isBranch)).Text;

        public void ClickAddImageButtonInBranch(string branchXpath) => new Element(branchXpath + addImageButtonInBranch).ClickIfExists();

        public void ClickDropdownMenuItem(string blockXpath, AuthoringFlowBlockMenuItem menuItem, bool isBranch=false) =>
            new Element(blockXpath + BlockDropdownMenuButton(isBranch) + string.Format(dropdownMenuItem, CommonHelper.GetFlowBlockMenuItem(menuItem))).ClickIfExists();

        public bool IsBlockDropdownExpanded(string blockXpath, bool isBranch) =>
            UiWaitHelper.Wait(()=> new Element(blockXpath + BlockDropdownMenuButton(isBranch)).GetAttribute("aria-expanded") == "true", WaitConstants.TwoSeconds);

        public void ClickRadioButton(string branchXpath, AuthoringFlowBlockMenuItem menuItem)
        {
            int buttonPosition = menuItem == AuthoringFlowBlockMenuItem.Buttons ? 1 : 2;

            new Element(IndexedXpath(branchXpath + radioButtonLabelInDecisionDropdown, buttonPosition)).ClickIfExists();
        }

        public void ClickButtonInProcess(string processXpath, string buttonName) => new Element(processXpath + string.Format(buttonInProcessBlock, buttonName)).ClickIfExists();

        public bool IsButtonInProcessVisible(string processXpath, string buttonName) => new Element(processXpath + string.Format(buttonInProcessBlock, buttonName)).IsDisplayed();
    }
}
