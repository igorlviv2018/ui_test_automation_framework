using NLog;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models;
using Taf.UI.PageObjects;
using Taf.UI.PageObjects.Authoring.CustomDevice;
using Taf.UI.PageObjects.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Taf.UI.Steps.AuthoringSteps.CustomDevice
{
    public class CustomDeviceCreateSteps : BaseSteps
    {
        public CustomDeviceCreateSteps(ILogger logger) : base(App.Taf, logger)
        {
            
        }

        private readonly AddDeviceModal addDeviceModal = new AddDeviceModal();

        private readonly DevicesPage devicesPage = new DevicesPage();

        private readonly TafAuthHelper authoringHelper = new TafAuthHelper();

        public string FillAddDeviceModal(DeviceType deviceType, string model)
        {
            string err = SelectRandomManufacturer();

            if (string.IsNullOrEmpty(err))
            {
                err = SelectDeviceType(deviceType);
            }

            if (string.IsNullOrEmpty(err))
            {
                addDeviceModal.SetModel(model);
            }

            return err;
        }

        public string SelectRandomManufacturer()
        {
            Dictionary<string, string> availableManufacturers = addDeviceModal.GetAvailableManufacturers();

            string err = string.Empty;

            if (availableManufacturers.Count > 0)
            {
                string randomOptionId = DataHelper.GetRandomElement(availableManufacturers.Select(d => d.Key).ToList());

                addDeviceModal.SelectManufacturer(randomOptionId);

                LogHelper.LogInfo(log, $"Selected '{availableManufacturers[randomOptionId]}' manufacturer");
            }
            else
            {
                err = "Failed to select a manufacturer";

                LogHelper.LogError(log, err);
            }

            return err;
        }

        public string SelectDeviceType(DeviceType deviceType)
        {
            Dictionary<string, string> availableDeviceTypes = addDeviceModal.GetAvailableDeviceTypes();

            string err = string.Empty;

            string optionId = string.Empty;

            //bool isApple = CommonHelper.IsAppleDevice(availableDeviceTypes);

            foreach (var key in availableDeviceTypes.Keys)
            {
                if (CommonHelper.GetDeviceType(availableDeviceTypes[key]) == deviceType)
                {
                    optionId = key;

                    addDeviceModal.SelectDeviceType(optionId);

                    LogHelper.LogInfo(log, $"Selected '{availableDeviceTypes[key]}' as device type");

                    break;
                }
            }
            
            if(string.IsNullOrEmpty(optionId))
            {
                err = $"Failed to select device type ({deviceType} not present in select drop-down)";

                LogHelper.LogError(log, err);
            }

            return err;
        }

        public void OpenAddDeviceModal()
        {
            devicesPage.ClickCreateDeviceButton();

            bool isModalAppeared = addDeviceModal.WaitModalAppeared();

            if (!isModalAppeared)
            {
                LogHelper.LogError(log, "Add device modal did not appear");
            }
        }
    }
}

