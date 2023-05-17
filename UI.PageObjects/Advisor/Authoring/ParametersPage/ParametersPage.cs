using Taf.UI.Core.Element;

namespace Taf.UI.PageObjects.TafTest.Authoring
{
    public class ParametersPage
    {
        private readonly string addParameterButton = "//div[contains(@class,'card-footer')]/button";

        public void ClickAddParameter() => new Element(addParameterButton).ClickIfExists();
    }
}
