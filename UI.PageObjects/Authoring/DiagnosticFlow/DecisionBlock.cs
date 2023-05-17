using Taf.UI.Core.Constants;
using Taf.UI.Core.Element;

namespace Taf.UI.PageObjects
{
    public class DecisionBlock : BasePage
    {
        private readonly string branchExpandButton = "/div[@class='branch-top']/div[contains(@class,'collapse-branch')]/button";

        private readonly string decisionBranch = "/div[@class='block-body']/div/div[contains(@class,'branches')]/div/div/div/div[contains(@class,'flow-block-branch')]";

        private readonly string blockInBranch = "/div[contains(@class,'branch-blocks')]/div[@class='collection-item']/div/div/div[contains(@class,'flow-block')]";

        private readonly string addNewBranchDropdownButton = "//div[contains(@class,'new-branch-wrap')]//button[contains(@class,'dropdown-toggle')]";

        private readonly string createNewBranchMenuItem = "//div[contains(@class,'new-branch-wrap')]//button[contains(@role,'menuitem')]";

        public string BranchAtPosition(string decisionXpath, int position) => $"({decisionXpath}{decisionBranch})[{position}]";

        public string BlockInBranchAtPosition(string branchXpath, int position) => $"({branchXpath}{blockInBranch})[{position}]";

        public bool IsBranchPresent(string branchXpath) => new Element(branchXpath).Exists(WaitConstants.OneSecond);

        public bool IsBranchCollapsed(string branchXpath) => new Element(branchXpath + branchExpandButton).GetAttribute("class").Contains("is-collapsed");

        public void ClickBranchExpandButton(string branchXpath) => new Element(branchXpath + branchExpandButton).ClickIfExists();

        public void ClickAddNewBranchDropdownButton(string decisionXpath) => new Element(decisionXpath + addNewBranchDropdownButton).ClickIfExists();

        public void ClickCreateNewBranchMenuItem(string branchXpath) => new Element(branchXpath + createNewBranchMenuItem).ClickIfExists();

        public bool IsAddNewBranchDropdownExpanded(string branchXpath) =>
            new Element(branchXpath + addNewBranchDropdownButton).GetAttribute("aria-expanded") == "true";
    }
}
