using Taf.UI.Core.Element;
using System.Collections.Generic;

namespace Taf.UI.PageObjects
{
    public class PublishSummaryModal : BasePage
    {
        private const string modalBody = "//div[contains(@class,'modal-body')]";

        private readonly string title =  $"{modalBody}//fieldset//div[contains(@class,'item-title')]";

        private readonly string description = $"{modalBody}//fieldset//div[contains(@class,'item-description')]";

        private readonly string button = $"{modalBody}//button";

        private readonly string fieldSet = $"{modalBody}//fieldset";

        private readonly string warningIcon = $"//span[contains(@class,'text-warning')]";

        private readonly string errorIcon = "//span[contains(@class,'text-danger')]";

        private string OmnichannelBlock() => IndexedXpath(fieldSet, 5);

        private string DiagnosticFlowPropertiesBlock() => IndexedXpath(fieldSet, 7);

        private string LinkToLocationInOmnichannelBlock() => OmnichannelBlock() + "//a";

        private string LinkToContentInFlowPropertiesBlock() => IndexedXpath(fieldSet, 7) + "//a";

        public void ClickButton(int buttonPosition) => new Element(IndexedXpath(button, buttonPosition)).ClickIfExists();

        public string ButtonName(int buttonPosition) => new Element(IndexedXpath(button, buttonPosition)).Text;

        public List<string> GetButtonNames() => GetTextOfElements(button);

        public string GetTitle() => new Element(title).Text;

        public string GetDescription() => new Element(description).Text;

        public bool IsNoChannelsWarningDisplayed() => new Element(OmnichannelBlock() + errorIcon).IsDisplayed();

        public bool IsIncompleteFlowErrorDisplayed() => new Element(DiagnosticFlowPropertiesBlock() + errorIcon).IsDisplayed();

        public void ClickGoToLocation() => new Element(LinkToLocationInOmnichannelBlock()).ClickIfExists();

        public void ClickGoToContent() => new Element(LinkToContentInFlowPropertiesBlock()).ClickIfExists();

        public bool WaitModalDisappeared() => WaitModalDisappeared(modalBody);

        public bool WaitModalAppeared() => WaitModalAppeared(modalBody);
    }
}
