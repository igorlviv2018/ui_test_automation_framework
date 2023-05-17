using Taf.UI.Core.Element;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Taf.UI.PageObjects.Authoring.CustomDevice
{
    public class AddDeviceModal : BasePage
    {
        private const string modalXpath = "//div[@class='modal-content']";

        private readonly string deviceManufacturerSelect = $"({modalXpath}//select)[1]";

        private readonly string deviceTypeSelect = $"({modalXpath}//select)[2]";

        private readonly string deviceModelInput = $"({modalXpath}//fieldset/div/input";

        private readonly string selectOption = $"/option[not(@disabled)]";

        //public bool IsModalDisplayed() => new Element(modalXpath).IsDisplayed();

        public Dictionary<string, string> GetSelectOptions(string optionXpath) =>
            new Element(optionXpath).FindElements().ToDictionary(e => e.GetAttribute("value"), e => e.Text);

        public Dictionary<string, string> GetAvailableManufacturers() =>
            GetSelectOptions(deviceManufacturerSelect + selectOption);

        public Dictionary<string, string> GetAvailableDeviceTypes() =>
            GetSelectOptions(deviceTypeSelect + selectOption);

        public void SelectManufacturer(string optionId) => new Select(deviceManufacturerSelect).SelectByValue(optionId);

        public void SelectDeviceType(string optionId) => new Select(deviceTypeSelect).SelectByValue(optionId);

        public void SetModel(string text) => new Element(deviceModelInput).SetText(text);

        public bool WaitModalDisappeared() => WaitModalDisappeared(modalXpath);

        public bool WaitModalAppeared() => WaitModalAppeared(modalXpath);
    }
}
