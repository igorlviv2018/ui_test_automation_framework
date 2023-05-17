using Taf.UI.Core.Element;

namespace Taf.UI.PageObjects
{
    public class ProcessBlock : BasePage
    {
        private readonly string innerContentBlock = "//div[contains(@class,'content-tree')]/div[contains(@class,'content-tree-node')]/div[contains(@class,'content-block')]/div[contains(@class,'content-block')]";

        public string InnerBlockAtPosition(string processXpath, int position) => $"({processXpath}{innerContentBlock})[{position}]";
    }
}
