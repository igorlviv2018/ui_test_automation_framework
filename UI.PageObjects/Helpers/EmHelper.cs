using Taf.UI.Core.Enums;
using Taf.UI.Core.Models;
using System.Collections.Generic;

namespace Taf.UI.PageObjects.Helpers
{
    public class TafHelper
    {
        private readonly AccordionBlock accordionBlock;

        private readonly ContentBlockBase contentBlock;

        public TafHelper(App app, bool isRedesign=false)
        {
            accordionBlock = new AccordionBlock(app, isRedesign);

            contentBlock = new ContentBlockBase(app, isRedesign);
        }

        public void AddXpathToPathElements(ArticlePath articlePath)
        {
            List<TafArticleElement> path = articlePath.Path;

            string xPath;

            string lastCollapseXPath = "";

            int counter = 1;

            for (int i = 1; i < path.Count; i++)
            {
                if (string.IsNullOrEmpty(lastCollapseXPath))
                {
                    xPath = contentBlock.TopLevelElementAtPositionXpath(path[i].ElementPosition);//counter);
                }
                else
                {
                    xPath = accordionBlock.ElementInCollapseAtPosition(lastCollapseXPath, counter);
                }

                bool isNextElementCollapse = (i < path.Count - 1) && path[i + 1].ElementType == ArticleContentElementType.Collapse;

                if (path[i].ElementType == ArticleContentElementType.Accordion && isNextElementCollapse)
                {
                    int collapsePos = path[i + 1].ElementPosition;

                    xPath = accordionBlock.CollapseAtPosition(xPath, collapsePos);

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
    }
}
