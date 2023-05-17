using Taf.UI.Core.Element;
using Taf.UI.Core.Helpers;

namespace Taf.UI.PageObjects
{
    public class AuthoringContentBlock : BasePage
    {
        // to complete!!
        private const string nestedContentBlock = "/div[contains(@class,'inner-content')]";

        private const string contentBlock = "/div[contains(@class,'content-tree')]/div[contains(@class,'content-tree-node')]/div[contains(@class,'content-block')]/div";

        private readonly string topLevelcontentHolder = "//div[contains(@class,'card')]/div[contains(@class,'content-holder')]";

        private readonly string plusButton = "/div[contains(@class,'text-center')]/button";

        private readonly string titleXpath = "/../h4";

        private readonly string expandCollapseButton = "/h4/button";

        public string TopLevelBlockAtPosition(int position) => IndexedXpath($"{topLevelcontentHolder}{contentBlock}", position);

        public string CollapseAtPosition(string parentXpath, int position) => $"({parentXpath}{nestedContentBlock}{contentBlock})[{position}]";

        public string StepInStepByStepAtPosition(string parentXpath, int position) => $"({parentXpath}{nestedContentBlock}{contentBlock})[{position}]";

        public string EmptyCollapseContentHolder(string collapseXpath) => $"{collapseXpath}{nestedContentBlock}";

        public string PlusButton(string parentXpath) => $"{parentXpath}{plusButton}";

        public string EmptyContentHolder() => topLevelcontentHolder;

        public string BlockInCollapseAtPosition(string parentXpath, int blockPosition) =>
            IndexedXpath($"{parentXpath}{nestedContentBlock}{contentBlock}", blockPosition);

        public void OpenCollapse(string collapseXpath)
        {
            Element collapse = new Element(collapseXpath);

            Element expandButton = new Element(collapseXpath + expandCollapseButton);

            if (collapse.GetAttribute("class").Contains("collapsed"))
            {
                expandButton.ClickIfExists();

                bool isExpandSuccessful = UiWaitHelper.WaitInMs(() => !collapse.GetAttribute("class").Contains("collapsed"), 500);
            }
        }

        public void ClickPlusButton(string blockXpath)
        {
            Element plusButton = new Element(PlusButton(blockXpath));

            plusButton.ClickIfExists();
        }

        public void ClickHeader(string blockXpath)
        {
            Element title = new Element($"{blockXpath}{titleXpath}");

            title.ClickIfExists();
        }

        public void ClickCollapseHeader(string blockXpath)
        {
            Element title = new Element($"{blockXpath}/h4");

            title.ClickIfExists();
        }

        public string GetTitle(string blockXpath, bool isCollapse=false)
        {
            string xPath = isCollapse ? $"{blockXpath}/h4" : $"{blockXpath}{titleXpath}";
            
            return new Element(xPath).Text;
        }

        public string GetContentBlockType(string blockXpath) =>
            new Element(blockXpath).GetAttribute("class");
    }
}
