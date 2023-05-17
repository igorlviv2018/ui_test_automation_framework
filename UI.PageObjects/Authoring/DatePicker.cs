using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;

namespace Taf.UI.PageObjects
{
    public class DatePicker
    {
        private readonly string pendingOrEvergreenBtn = "(//ul[@role='menu' and contains(@class,'show')]//button)[1]";

        private readonly string immediatelyBtn = "(//ul[@role='menu' and contains(@class,'show')]//button)[2]";

        private readonly string currentMonthYearBtn = "//ul[@role='menu' and contains(@class,'show')]//span[contains(@class,'day__month')]";

        private readonly string currentYearBtn = "//ul[@role='menu' and contains(@class,'show')]//span[contains(@class,'month__year')]";

        private readonly string dayCell = "//ul[@role='menu' and contains(@class,'show')]//span[contains(@class,'cell day') and text()='{0}']";

        private readonly string monthCell = "//ul[@role='menu' and contains(@class,'show')]//span[contains(@class,'cell month') and text()='{0}']";

        private readonly string yearIncreaseBtn = "//ul[@role='menu' and contains(@class,'show')]//span[contains(@class,'month__year')]/../span[@class='next']";

        private readonly string yearDecreaseBtn = "//ul[@role='menu' and contains(@class,'show')]//span[contains(@class,'month__year')]/../span[@class='prev']";

        public void ClickPickerButton(DatePickerButton button)
        {
            string xPath = immediatelyBtn;

            if (button == DatePickerButton.Pending || button == DatePickerButton.Evergreen)
            {
                xPath = pendingOrEvergreenBtn;
            }

            new Element(xPath).Click();
        }

        public string GetMonthYear()
        {
            Element monthYear = new Element(currentMonthYearBtn);

            return monthYear.Exists()
                ? monthYear.Text
                : string.Empty;
        }

        public void ClickMonthYearButton()
        {
            new Element(currentMonthYearBtn).Click();
        }

        public bool IsMonthYearButtonDisplayed()
        {
            return new Element(currentMonthYearBtn).IsDisplayed();
        }

        public string GetYear()
        {
            Element year = new Element(currentYearBtn);

            return year.Exists()
                ? year.Text
                : string.Empty;
        }

        public bool IsYearButtonDisplayed()
        {
            return new Element(currentYearBtn).IsDisplayed();
        }

        public void ClickDayCell(int day)
        {
            new Element(string.Format(dayCell, day)).Click();
        }

        public void ClickMonthCell(string month)
        {
            new Element(string.Format(monthCell, month)).Click();
        }

        public bool IsMonthCellEnabled(string month)
        {
            Element monthElement = new Element(string.Format(monthCell, month));

            return monthElement.IsDisplayed() && !monthElement.GetAttribute("class").Contains("disabled");
        }

        public bool IsDayCellEnabled(int day)
        {
            Element dayElement = new Element(string.Format(dayCell, day));

            return dayElement.IsDisplayed() && !dayElement.GetAttribute("class").Contains("disabled");
        }

        public bool IncreaseYear()
        {
            Element increaseBtn = new Element(yearIncreaseBtn);

            bool increaseSucceeed = false;

            if (increaseBtn.IsDisplayed())
            {
                increaseBtn.Click();

                increaseSucceeed = true;
            }

            return increaseSucceeed;
        }

        public bool DecreaseYear()
        {
            Element decreaseBtn = new Element(yearDecreaseBtn);

            bool decreaseSucceeed = false;

            if (decreaseBtn.IsDisplayed())
            {
                decreaseBtn.Click();

                decreaseSucceeed = true;
            }

            return decreaseSucceeed;
        }
    }
}
