using Taf.UI.Core.Constants;
using Taf.UI.Core.Element;
using Taf.UI.Core.Helpers;

namespace Taf.UI.PageObjects.TafTest.Taf
{
    public class NavigationElements : BasePage
    {
        private readonly string progressBarStep = "//div[contains(@class,'progress-point')]";

        private const string button = "//div[contains(@class,'controls-wrap')]/button";

        private readonly string buttonByName = button + "[contains(text(),'{0}')]";

        private readonly string startOverButton = "//span[contains(@class,'ss-replay')]";

        public int GetStepCount() => new Element(progressBarStep).Count;

        public bool IsProgressBarStepActive(int stepNumber) =>
            new Element(IndexedXpath(progressBarStep, stepNumber)).GetAttribute("class").Contains("active");

        public void ClickButton(string buttonName) => new Element(string.Format(buttonByName, buttonName)).ClickIfExists();

        public void ClickStartOverButton() => new Element(startOverButton).ClickIfExists();

        public bool IsButtonPresent(string buttonName) => new Element(string.Format(buttonByName, buttonName)).Exists();

        public bool WaitButtonToAppear() => new Element(button).Exists(WaitConstants.HalfMinuteInSec);

        public bool WaitProgressBarStepToBecomeActive(int stepNumber) =>
            UiWaitHelper.Wait(() => IsProgressBarStepActive(stepNumber), WaitConstants.CheckElementExistInSec);
    }
}
