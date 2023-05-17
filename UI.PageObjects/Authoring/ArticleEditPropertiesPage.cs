using Taf.UI.Core.Element;
using Taf.WebDriver.Wrapper;
using System.Collections.Generic;

namespace Taf.UI.PageObjects
{
    public class ArticleEditPropertiesPage : BasePage
    {
        // to complete!!!

        private readonly string titleInput = "//section/div[contains(@class,'required')]//input[@id='titleInput']";

        private readonly string titleLabelWithAsterisk = "//section/div[contains(@class,'required')]/label[@for='titleInput']";

        private readonly string descriptionInput = "//section/div[contains(@class,'required')]//textarea[@id='descriptionInput']";

        private readonly string descriptionLabelWithAsterisk = "//section/div[contains(@class,'required')]/label[@for='descriptionInput']";

        private readonly string workNoteInput = "//section//textarea[@id='noteInput']";

        private readonly string workNoteLabel = "//section//label[@for='noteInput']";

        private readonly string ownerLabelWithAsterisk = "//div[@class='row']//div[contains(@class,'required')]//label[@for='ownerInput']";

        private readonly string currentOwnerName = "//div[@class='search-wrap']//span[@class='text-truncate']";

        private readonly string ownerClearBtn = "//div[@class='search-wrap']//span[contains(@class,'deselect-btn')]";

        private readonly string ownerListOpenBtn = "//div[@class='search-wrap']//span[@class='caret']";

        private readonly string ownerListOpenedClosed = "//div[contains(@class,'search-select')]";

        private const string ownerListItem = "//div[contains(@class,'search-select')]//a[@class='dropdown-item']";

        private const string ownerListItemByName = "//div[contains(@class,'search-select')]//a[@class='dropdown-item' and contains(text(),'{0}')]";

        private readonly string ownerListItemNumbered = $"({ownerListItem})" + "[{0}]";

        private readonly string scheduleButton = "//div[@role='button' and @aria-controls='schedule']";

        private readonly string publishDateButton = "(//div[contains(@class,'datepicker-dd')]/button)[1]";

        private readonly string currentPublishDate = "(//div[contains(@class,'datepicker-dd')]/button)[1]/span";

        private readonly string expirationDateButton = "(//div[contains(@class,'datepicker-dd')]/button)[2]";

        private readonly string currentExpirationDate = "(//div[contains(@class,'datepicker-dd')]/button)[2]/span";

        public void SetTitle(string text) => new Element(titleInput).SetText(text);

        public void SetDescription(string text) => new Element(descriptionInput).SetText(text);

        public void SetWorkNote(string text) => new Element(workNoteInput).SetText(text);

        public string GetTitle() => new Element(titleInput).Text;

        public string GetUrl() => Browser.Current.GetWindowUrl();

        public string GetDescription() => new Element(descriptionInput).Text;

        public string GetWorkNote() => new Element(workNoteInput).Text;

        public string GetCurrentOwnerName() => new Element(currentOwnerName).Text;

        public void ClearOwnerInput() => new Element(ownerClearBtn).ClickIfExists();

        public bool IsOwnerListOpened() => new Element(ownerListOpenedClosed).GetAttribute("class").Contains("is-opened");

        public List<string> GetOwnerListItems() => GetTextOfElements(ownerListItem);

        public void ClickOwnerListItem(int itemNumber) => new Element(string.Format(ownerListItemNumbered, itemNumber)).Click();

        public void ClickOwnerListItem(string ownerName) => new Element(string.Format(ownerListItemByName, ownerName)).Click();

        public void OpenOwnerList() => new Element(ownerListOpenBtn).Click();

        public void CloseOwnerList() => new Element(ownerListOpenBtn).Click();

        public void ClickScheduleButton() => new Element(scheduleButton).Click();

        public bool IsScheduleOpened() => new Element(scheduleButton).GetAttribute("aria-expanded").Contains("true");

        public void ClickPublishDateButton() => new Element(publishDateButton).ClickIfExists();

        public string GetPublishDate() => new Element(currentPublishDate).Text;

        public string GetExpirationDate() => new Element(currentExpirationDate).Text;

        public bool IsPublishDatePickerOpened() => new Element(publishDateButton).GetAttribute("aria-expanded").Contains("true");

        public void ClickExpirationDateButton() => new Element(expirationDateButton).ClickIfExists();

        public bool IsExpirationDatePickerOpened() => new Element(expirationDateButton).GetAttribute("aria-expanded").Contains("true");
    }
}
