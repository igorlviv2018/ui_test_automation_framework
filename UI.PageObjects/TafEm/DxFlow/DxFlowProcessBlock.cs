using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models;
using System.Collections.Generic;
using System.Threading;

namespace Taf.UI.PageObjects
{
    public class DxFlowProcessBlock : ContentBlockBase
    {
        public DxFlowProcessBlock(App app, bool isRedesign=false) : base(app, isRedesign)
        { 
        
        }

        private string ButtonXpath(int pathItemNum, int blockNum) =>
            DxFlowProcessContentBlockTypeXpath(pathItemNum, blockNum) + ButtonXpath();

        private string ButtonByPositionXpath(int pathItemNum, int blockNum, int buttonPosition) =>
            "(" + ButtonXpath(pathItemNum, blockNum) + $")[{buttonPosition}]";

        public List<ArticleContentElementType> GetContentBlockTypes(int pathItemNum)
        {
            List<ArticleContentElementType> blockTypes = new List<ArticleContentElementType>();

            for (int i = 0; i < GetContentBlocksCount(pathItemNum); i++)
            {
                blockTypes.Add(GetContentBlockType(pathItemNum, i + 1));
            }

            return blockTypes;
        }

        public ArticleContentElementType GetContentBlockType(int pathItemNum, int blockNum)
        {
            string type = new Element(DxFlowProcessContentBlockTypeXpath(pathItemNum, blockNum)).GetAttribute("class");

            return CommonHelper.GetContentElementType(type);
        }

        public int GetContentBlocksCount(int pathItemNum) =>
            new Element(ContentBlockInProcessXpath(pathItemNum)).Count;

        public void ClickButton(int pathItemNum, int blockNum, int buttonPosition)
        {
            Thread.Sleep(400); // debug - sometimes click is not handled ...

            Element button = new Element(ButtonByPositionXpath(pathItemNum, blockNum, buttonPosition));

            button.ScrollToView();

            button.ClickIfExists();
        }

        public void ClickButton(DxFlowProcessButtonPosition buttonPosition) =>
            ClickButton(buttonPosition.ProcessPosition, buttonPosition.ButtonsBlockPosition, buttonPosition.ButtonPosition);

        public List<string> GetButtons(int pathItemNum, int blockNum) =>
            GetTextOfElements(ButtonXpath(pathItemNum, blockNum));
    }
}
