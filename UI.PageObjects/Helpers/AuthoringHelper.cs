using Taf.UI.Core.Enums;
using Taf.UI.Core.Models;
using System.Collections.Generic;

namespace Taf.UI.PageObjects.Helpers
{
    public class TafHelper
    {
        private readonly AuthoringContentBlock contentBlock = new AuthoringContentBlock();

        private readonly FlowContentBlock flowContentBlock = new FlowContentBlock();

        private readonly DecisionBlock decisionBlock = new DecisionBlock();

        public void AddXpathToPathElements(ArticlePath articlePath)
        {
            List<TafArticleElement> path = articlePath.Path;

            string xPath;

            string lastCollapseXPath = string.Empty;

            int counter = 1;

            for (int i = 1; i < path.Count; i++)
            {
                if (string.IsNullOrEmpty(lastCollapseXPath))
                {
                    xPath = contentBlock.TopLevelBlockAtPosition(path[i].ElementPosition);
                }
                else
                {
                    xPath = contentBlock.BlockInCollapseAtPosition(lastCollapseXPath, counter);
                }

                bool isNextElementCollapse = (i < path.Count - 1) && path[i + 1].ElementType == ArticleContentElementType.Collapse;

                if (path[i].ElementType == ArticleContentElementType.Accordion && isNextElementCollapse)
                {
                    int collapsePos = path[i + 1].ElementPosition;

                    path[i].XPath = xPath; // accordion - debug

                    xPath = contentBlock.CollapseAtPosition(xPath, collapsePos);

                    lastCollapseXPath = xPath;

                    path[i + 1].XPath = xPath;

                    counter = 0;
                }
                else if (path[i].ElementType == ArticleContentElementType.Collapse)
                {
                    continue;
                }
                else
                {
                    path[i].XPath = xPath;
                }

                counter++;
            }
        }

        public void AddXpathToFlowElements(List<TafArticleElement> traverseSequence)
        {
            string xPath;

            bool isTopLevelElement = true;

            for (int i = 0; i < traverseSequence.Count; i++)
            {
                TafArticleElement currentElement = traverseSequence[i];

                if (isTopLevelElement)
                {
                    xPath = flowContentBlock.TopLevelBlockAtPosition(i + 1);
                }
                else if (currentElement.ElementType == ArticleContentElementType.Branch)
                {
                    xPath = decisionBlock.BranchAtPosition(currentElement.Parent.XPath, currentElement.ElementPosition);
                }
                else // block in branch
                {
                    string branchXpath = currentElement.PathToElement.FindLast(x => x.ElementType == ArticleContentElementType.Branch).XPath;

                    int blockPosition = currentElement.PathToElement.Count - currentElement.PathToElement.FindLastIndex(x => x.ElementType == ArticleContentElementType.Branch);

                    xPath = decisionBlock.BlockInBranchAtPosition(branchXpath, blockPosition);
                }

                if (currentElement.ElementType == ArticleContentElementType.Decision)
                {
                    isTopLevelElement = false;
                }

                currentElement.XPath = xPath;
            }
        }

        public string ContentBlockToDropElementToXpath(TafArticleElement parent)
        {
            string xPath;

            if (parent.ElementType == ArticleContentElementType.Root)
            {
                xPath = contentBlock.EmptyContentHolder();
            }
            else if (parent.ElementType == ArticleContentElementType.Collapse)
            {
                xPath = contentBlock.EmptyCollapseContentHolder(parent.XPath);
            }
            else
            {
                xPath = parent.XPath;
            }

            return xPath;
        }
    }
}
