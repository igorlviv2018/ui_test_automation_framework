using NLog;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models;
using Taf.UI.PageObjects;
using Taf.UI.PageObjects.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Taf.UI.Steps.Authoring
{
    public class DiagnosticFlowCreateSteps : BaseSteps
    {
        public DiagnosticFlowCreateSteps(ILogger logger) : base(App.Taf, logger)
        {
            blockConfigurationSteps = new FlowBlockConfigurationSteps(log);
        }

        private readonly FlowContentBlock flowContentBlock = new FlowContentBlock();

        private readonly DecisionBlock decisionBlock = new DecisionBlock();

        private readonly DataHelper dataHelper = new DataHelper();

        private readonly FlowBlockConfigurationSteps blockConfigurationSteps;

        private readonly TafAuthHelper authoringHelper = new TafAuthHelper();

        public string CreateFlowContent(List<TafEmArticleElement> elementSequence)
        {
            string err = string.Empty;

            bool hasInternalConnector = elementSequence.Any(x => x.ElementType == ArticleContentElementType.InternalConnector);

            if (hasInternalConnector)
            {
                err = CreateFlowWithIntConnectorContent(elementSequence);
            }
            else
            {
                err = CreateFlowWithoutIntConnectorContent(elementSequence);
            }

            return err;
        }

        public string CreateFlowWithoutIntConnectorContent(List<TafEmArticleElement> elementSequence)
        {
            string err = string.Empty;

            foreach (var element in elementSequence)
            {
                err = AddFlowContentBlock(element);

                if (!string.IsNullOrEmpty(err))
                {
                    break;
                }

                err = blockConfigurationSteps.ConfigureFlowContentBlock(element);

                if (!string.IsNullOrEmpty(err))
                {
                    break;
                }
            }

            return err;
        }

        public string CreateFlowWithIntConnectorContent(List<TafEmArticleElement> elementSequence)
        {
            string err = string.Empty;

            foreach (var element in elementSequence)
            {
                err = AddFlowContentBlock(element);

                if (!string.IsNullOrEmpty(err))
                {
                    break;
                }
            }

            foreach (var element in elementSequence)
            {
                err = blockConfigurationSteps.ConfigureFlowContentBlock(element);

                if (!string.IsNullOrEmpty(err))
                {
                    break;
                }
            }

            return err;
        }

        public string AddFlowContentBlock(TafEmArticleElement element)
        {
            AddFlowElement(element, element.Parent);

            string err = CheckContentElementPlaced(element);

            LogHelper.LogResult(log, $"Added content block: {element}", err);

            return err;
        }

        /// <summary>
        /// Add flow element (flow element is either block or decision branch)
        /// </summary>
        /// <param name="elementToAdd"></param>
        /// <param name="parentElement"></param>
        public void AddFlowElement(TafEmArticleElement elementToAdd, TafEmArticleElement parentElement)
        {
            if (elementToAdd.ElementType == ArticleContentElementType.Branch)
            {
                AddDecisionBranch(elementToAdd);
            }
            else //flow block
            {
                AddFlowBlock(elementToAdd, parentElement);
            }
        }

        public void AddFlowBlock(TafEmArticleElement elementToAdd, TafEmArticleElement parentElement)
        {
            string menuItemName = CommonHelper.GetFlowBlockTypeInAddBlockDropdown(elementToAdd.ElementType);

            if (parentElement.ElementType == ArticleContentElementType.Root)
            {
                flowContentBlock.ClickAddBlockButtonInStartBlock();

                flowContentBlock.SelectAddBlockMenuItemInStartBlock(menuItemName);
            }
            else
            {
                bool isParentElementBranch = parentElement.ElementType == ArticleContentElementType.Branch;

                if (isParentElementBranch && decisionBlock.IsBranchCollapsed(parentElement.XPath))
                {
                    decisionBlock.ClickBranchExpandButton(parentElement.XPath);
                }

                flowContentBlock.ClickAddBlockButton(parentElement.XPath, isParentElementBranch);

                flowContentBlock.SelectAddBlockMenuItem(parentElement.XPath, menuItemName, isParentElementBranch);
            }

            if (elementToAdd.ElementType != ArticleContentElementType.InternalConnector)
            {
                Thread.Sleep(100);

                SetContentBlockTitleAndDescription(elementToAdd, elementToAdd.Title, elementToAdd.Description);
            }
        }

        public void AddDecisionBranch(TafEmArticleElement branchToAdd)
        {
            if (branchToAdd.ElementPosition > 2)
            {
                decisionBlock.ClickAddNewBranchDropdownButton(branchToAdd.Parent.XPath);

                decisionBlock.ClickCreateNewBranchMenuItem(branchToAdd.Parent.XPath);
            }

            TafEmBranchData branchData = dataHelper.GetElementData<TafEmBranchData>(branchToAdd);

            SetContentBlockTitleAndDescription(branchToAdd, branchData.Answer);
        }

        public void SetContentBlockTitleAndDescription(TafEmArticleElement element, string title = "", string description = "")
        {
            if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(description))
            {
                return;
            }

            bool isBranch = element.ElementType == ArticleContentElementType.Branch;

            if (isBranch)
            {
                flowContentBlock.SetBranchTitle(element.XPath, title);

                flowContentBlock.ClickBlockIcon(element.Parent.XPath);
            }
            else
            {
                if (!string.IsNullOrEmpty(title))
                {
                    flowContentBlock.ClickTitle(element.XPath);

                    flowContentBlock.SetTitle(element.XPath, title);
                }

                if (!string.IsNullOrEmpty(description))
                {
                    SetDescription(description, element.XPath);
                }

                flowContentBlock.ClickBlockIcon(element.XPath);
            }
        }

        public void SetDescription(string description, string elementXpath)
        {
            //new
            flowContentBlock.ClickDescriptionEditIcon(elementXpath);

            flowContentBlock.WaitDescriptionEditable(elementXpath);

            flowContentBlock.SetDescriptionText(elementXpath, description);

            flowContentBlock.ClickBlockIcon(elementXpath);
        }

        public string CheckContentElementPlaced(TafEmArticleElement element)
        {
            string err = string.Empty;

            string elementXpath = element.XPath;

            ArticleContentElementType elementType = element.ElementType;

            bool isBranch = elementType == ArticleContentElementType.Branch;

            string expectedTitle = isBranch ? dataHelper.GetElementData<TafEmBranchData>(element).Answer : element.Title;

            if (isBranch && !decisionBlock.IsBranchPresent(elementXpath))
            {
                return $"[Critical] Failed to add {elementType} element (title='{expectedTitle}')";
            }

            bool isElementTypeCorrect = isBranch
                || UiWaitHelper.Wait(() => GetFlowBlockType(elementXpath) == elementType, WaitConstants.OneSecond);

            string actualTitle = flowContentBlock.GetTitle(elementXpath, isBranch);

            if (!isElementTypeCorrect) //(actualTitle != elementTitle)
            {
                err = $"[Critical] Failed to place {elementType} element (title='{expectedTitle}') to the correct location";
            }

            if (element.ElementType != ArticleContentElementType.ExternalConnector && (actualTitle != expectedTitle))
            {
                err = $"[Critical] Failed to place {elementType} element (title='{expectedTitle}') to the correct location";
            }

            return err;
        }

        public ArticleContentElementType GetFlowBlockType(string elementXpath) =>
            CommonHelper.GetFlowBlockType(flowContentBlock.GetBlockType(elementXpath));

        public List<TafEmArticleElement> GetFlowBlockSequence(List<TafEmArticleElement> flowElements)
        {
            foreach (var _ in flowElements[0].FindArticlePaths(flowElements))
            {

            }

            authoringHelper.AddXpathToFlowElements(flowElements[0].TreeTraversalSequence);

            return flowElements[0].TreeTraversalSequence;
        }
    }
}

