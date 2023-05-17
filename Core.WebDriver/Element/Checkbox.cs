using OpenQA.Selenium;

namespace Taf.UI.Core.Element
{
    public class Checkbox : Element
    {
        public Checkbox(string selector) : base(selector)
        {

        }

        public bool IsChecked
        {
            get 
            { 
                return IsSelected; 
            }

            set
            {
                if (IsSelected != value)
                {
                    ClickWithActions();
                }
            }
        }

        public void Check()
        {
            IsChecked = true;
        }

        public void Uncheck()
        {
            IsChecked = false;
        }
    }
}
