using Taf.UI.Core.Element;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Taf.UI.PageObjects.Authoring.CustomDevice
{
    public class DevicesPage : BasePage
    {
        private readonly string createDeviceButton = "//main[contains(@class,'devices-page')]/button";

        public void ClickCreateDeviceButton() => new Element(createDeviceButton).ClickIfExists();

        //public void SelectManufacturer(string optionId) => new Select(manufacturerSelect).SelectByValue(optionId);

        //public bool WaitModalDisappeared() => WaitModalDisappeared(modalXpath);

        //public bool WaitModalAppeared() => WaitModalAppeared(modalXpath);
    }
}
