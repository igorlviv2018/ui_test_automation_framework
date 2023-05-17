using NLog;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using System.Collections.Generic;

namespace Taf.UI.PageObjects
{
    public class ContentBlockBase: BasePage
    {
        private readonly ILogger log;

        private readonly bool isRedesign;

        public ContentBlockBase(App app, bool isRedesign=false)
        {
            locators = SetLocators(app, locatorsCommon, locatorsEmbed, locatorsTaf,
                agentsRedesign: locatorsTafRedesign, isRedesign: isRedesign);

            log = LogManager.GetLogger($"{app}UI");

            this.isRedesign = isRedesign;
        }

        private readonly Dictionary<string, string> locators;

        private readonly Dictionary<string, string> locatorsCommon =

            new Dictionary<string, string>()
            {
                { "button in buttons block", "//button[contains(@class,'btn')]"},
                { "external flow title", "/div/h2"},
                { "flow path item", "//div[@class='diagnostic-flow-preview']//div[contains(@class,'path-item')]"},
                { "flow block title", "//h3"}
            };

        private readonly Dictionary<string, string> locatorsTaf =

            new Dictionary<string, string>()
            {
                { "article title", "//div[@class='card-header']/div/h3"},
                { "top level element", "//div[@class='article-main']/div/div[@class='my-3']"},
                { "content block", "//div[contains(@class,'content-blocks-preview')]/div[contains(@class,'my-')]"},
                { "flow block description", "//p[contains(@class,'description')]"}
            };

        private readonly Dictionary<string, string> locatorsEmbed =

            new Dictionary<string, string>()
            {
                { "article title", "//div[@class='sp-article-widget']//h2[@class='article-title']"},
                { "top level element", "//div[@class='sp-article-widget']/div/div[@class='custom-article']/div[contains(@class,'size-')]"},
                { "content block", "//div[@class='custom-article']/div[contains(@class,'size-')]"},
                { "flow block description", "//p[contains(@class,'description')]"}
            };

        private readonly Dictionary<string, string> locatorsTafRedesign =

            new Dictionary<string, string>()
            {
                { "article title", "//div[@class='card-header']/div/h3"},
                { "top level element", "//div[@class='article-main']/div/div[@class='my-3']"},
                { "content block", "//div[contains(@class,'article-preview')]/div[contains(@class,'my-')]"},//new
                { "flow block description", "//p[contains(@class,'italic')]"}//new
            };

        private string ContentElementType(string xPath) => $"{xPath}/div";

        public string TopLevelElementAtPositionXpath(int position) => $"({GetXpath("top level element", locators)})[{position}]";

        public string ButtonXpath() => GetXpath("button in buttons block", locators);

        public string DxFlowPathItemXpath() => GetXpath("flow path item", locators);

        public string DxFlowPathItemAtPositionXpath(int position) => $"({DxFlowPathItemXpath()})[{position}]";

        public string DxFlowExternalFlowTitleAtPositionXpath(int position) =>
            DxFlowPathItemAtPositionXpath(position) + GetXpath("external flow title", locators);

        public string DxFlowPathItemTitleAtPositionXpath(int position) =>
            DxFlowPathItemAtPositionXpath(position) + GetXpath("flow block title", locators);

        public string DxFlowPathItemDescriptionAtPositionXpath(int position) =>
            DxFlowPathItemAtPositionXpath(position) + GetXpath("flow block description", locators);

        public string DxFlowProcessContentBlockXpath(int pathItemNum, int blockNum) =>
            IndexedXpath(ContentBlockInProcessXpath(pathItemNum), blockNum);

        public string ContentBlockInProcessXpath(int pathItemNum) =>
            DxFlowPathItemAtPositionXpath(pathItemNum) + GetXpath("content block", locators);

        public string DxFlowProcessContentBlockTypeXpath(int pathItemNum, int blockNum) =>
            DxFlowProcessContentBlockXpath(pathItemNum, blockNum) + "/div";

        public string GetArticleTitle() => new Element(GetXpath("article title", locators)).Text;

        public string GetDxFlowPathItemTitle(int itemPosition) =>
            new Element(DxFlowPathItemTitleAtPositionXpath(itemPosition)).Text;

        public string GetDxFlowPathItemDescription(int itemPosition) =>
            new Element(DxFlowPathItemDescriptionAtPositionXpath(itemPosition)).Text;

        public string GetDxFlowExternalFlowTitle(int itemPosition) =>
            new Element(DxFlowExternalFlowTitleAtPositionXpath(itemPosition)).Text;

        public PathItemType GetDxFlowPathItemType(int itemPosition)
        {
            Element pathItem = new Element(DxFlowPathItemAtPositionXpath(itemPosition));

            PathItemType type = PathItemType.Undefined;

            if (pathItem.Exists())
            {
                type = CommonHelper.GetPathItemType(pathItem.GetAttribute("class"));
            }

            return type;
        }

        public ArticleContentElementType GetContentElementType(string contentElementXpath)
        {
            Element contentElement = new Element(ContentElementType(contentElementXpath));

            ArticleContentElementType type = ArticleContentElementType.Undefined;

            if (contentElement.Exists())
            {
                type = CommonHelper.GetContentElementType(contentElement.GetAttribute("class"), isRedesign);
            }

            return type;
        }

        public bool WaitArticleLoad() => UiWaitHelper.Wait(() => GetArticleTopLevelElementCount() > 0, WaitConstants.CheckElementExistInSec);

        public int GetArticleTopLevelElementCount() => new Element(GetXpath("top level element", locators)).Count;

        public int GetDxFlowPathItemCount() => new Element(DxFlowPathItemXpath()).Count;

        public void DxFlowPathItemScrollToView(int pathItemNum) =>
            new Element(DxFlowPathItemAtPositionXpath(pathItemNum)).ScrollToView();

        public void DxFlowContentBlockScrollToView(int pathItemNum, int contentBlockNum) =>
            new Element(DxFlowProcessContentBlockXpath(pathItemNum, contentBlockNum)).ScrollToView();

        public bool WaitNumberOfPathItems(int numOfElementsToWait, bool exactMatch = true)
        {
            Element pathItem = new Element(DxFlowPathItemXpath());

            bool isWaitSuccessful = UiWaitHelper.Wait(
                () => exactMatch
                      ? pathItem.Count == numOfElementsToWait
                      : pathItem.Count >= numOfElementsToWait,
                WaitConstants.CheckElementExistInSec, 100);

            if (isWaitSuccessful)
            {
                log.Info($"Expected path item count displayed: {pathItem.Count}");

                Element lastPathItem = new Element(DxFlowPathItemAtPositionXpath(numOfElementsToWait));

                bool isPathItemRendered() => !lastPathItem.GetAttribute("class").Contains("path-enter-active");

                bool isLastPathItemRendered = UiWaitHelper.Wait(() => isPathItemRendered(), WaitConstants.CheckElementExistInSec, 100);

                log.Info($"Last path item is rendered: {isLastPathItemRendered}");
            }

            return isWaitSuccessful;
        }
    }
}
