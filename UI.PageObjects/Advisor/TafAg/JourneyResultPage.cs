using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;
using System.Collections.Generic;

namespace Taf.UI.PageObjects.TafTest.Taf
{
    public class JourneyResultPage : BasePage
    {
        private const string recommendedItem = "//div[contains(@class,'recommendation-block')]//div[@class='row']/div[contains(@class,'device')]";

        private readonly string noMatchingText = "//div[contains(@class,'recommendations')]/p[contains(@class,'no-matching')]";

        private readonly string manufacturer = recommendedItem + "/p";

        private readonly string model = recommendedItem + "/h3";

        //private readonly string bestMatchHeader = "//div[contains(@class,'advisor_recommend')]//h1";
        private readonly string backToPreviousStepButton = "//div[contains(@class,'controls-wrap')]/button";

        private readonly Dictionary<string, string> locators;

        private readonly Dictionary<string, string> locatorsCommon =

            new Dictionary<string, string>()
            {

            };

        private readonly Dictionary<string, string> locatorsTaf =

            new Dictionary<string, string>()
            {
                { "back to previous step button", "//button/span[contains(@class,'ss-left')]"}
            };

        private readonly Dictionary<string, string> locatorsTafTest =

            new Dictionary<string, string>()
            {
                { "back to previous step button", "//div[contains(@class,'controls-wrap')]/button"}
            };

        public JourneyResultPage(App app)
        {
            locators = SetLocatorsAdvisor(app, locatorsCommon, locatorsTaf, locatorsTafTest);
        }

        public int GetRecommendedItemsCount() => new Element(recommendedItem).Count;

        public string GetManufacturer(int blockPosition) => new Element(IndexedXpath(manufacturer, blockPosition)).Text;

        public string GetModel(int blockPosition) => new Element(IndexedXpath(model, blockPosition)).Text;

        public string GetNoMatchingText() => new Element(noMatchingText).Text;

        public void ClickBackToPreviousStepButton() => new Element(backToPreviousStepButton).ClickIfExists();

        public bool IsBackToPreviousStepButtonDisplayed() => new Element(backToPreviousStepButton).IsDisplayed();
    }
}
