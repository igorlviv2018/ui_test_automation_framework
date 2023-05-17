using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;
using System.Collections.Generic;

namespace Taf.UI.PageObjects
{
    public class ButtonsBlock : ContentBlockBase
    {
        public ButtonsBlock(App app) : base(app)
        { 
        
        }

        private string ButtonByPositionXpath(string blockXpath, int buttonPosition) =>
            IndexedXpath(blockXpath + ButtonXpath(), buttonPosition);

        public List<string> GetButtonLabels(string blockXpath) => GetTextOfElements(blockXpath + ButtonXpath());

        public void ClickButton(string blockXpath, int buttonPosition)
        {
            Element button = new Element(ButtonByPositionXpath(blockXpath, buttonPosition));

            button.ScrollToView();

            button.ClickIfDisplayed();
        }
    }
}
