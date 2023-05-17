using NLog;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.PageObjects;
using System.Collections.Generic;
using Taf.UI.PageObjects.TafAd.Authoring;
using Taf.UI.Core.Constants;
using System;
using Taf.UI.Core.Models.TafAd;

namespace Taf.UI.Steps.TafAdSteps
{
    public class JourneyParametersSteps : TableBaseSteps
    {
        public JourneyParametersSteps(ILogger logger) : base(App.Taf, logger)
        {

        }

        private readonly ParametersTable parametersTable = new ParametersTable();

        private readonly ParameterEditor parameterEditor = new ParameterEditor();

        private readonly ParametersPage parametersPage = new ParametersPage();

        private readonly ParameterScale parameterScale = new ParameterScale();

        private readonly Spinner spinner = new Spinner(App.TafAd);

        private readonly ToastAlertSteps toastAlertSteps = new ToastAlertSteps();

        private readonly ConfirmModalSteps confirmModalSteps = new ConfirmModalSteps();

        public string CreateParameterProperties(string title, JourneyParameterType type, string description)
        {
            int parameterCountBeforeCreating = parametersTable.GetRowCount();

            parametersPage.ClickAddParameter();

            parameterEditor.SetTitle(title);

            parameterEditor.SetDescription(description);

            if (type == JourneyParameterType.Interval)
            {
                parameterEditor.ExpandDropdown();

                parameterEditor.ClickDropdownMenuItem(type.ToString());
            }

            parameterEditor.ClickConfirmButton();

            List<string> errors = new List<string>();

            string err = toastAlertSteps.CheckAlertPopup(AlertStatus.Success);

            errors.Add(err);

            bool isParamCountIncreasedByOne = UiWaitHelper.Wait(() => parametersTable.GetRowCount() - parameterCountBeforeCreating == 1, WaitConstants.TwoSeconds);

            //int parameterCountAfterCreating = parametersTable.GetRowCount();

            if (!isParamCountIncreasedByOne)
            {
                errors.Add($"Parameter was not created (count of params did not increase by 1)");
            }

            return ErrorHelper.ConvertErrorsToString(errors);
        }

        public string EditParameterProperties(string title, string newTitle, string newDescription)
        {
            List<string> titles = parametersTable.GetTitleColumn();

            int parameterRowPosition = FindItemRowPosition(title, titles) + 1;

            if (parameterRowPosition < 1)
            {
                return $"Parameter '{title}' not found!";
            }

            parametersTable.ClickEditButton(parameterRowPosition);

            parameterEditor.SetTitle(newTitle, parameterRowPosition);

            parameterEditor.SetDescription(newDescription, parameterRowPosition);

            parameterEditor.ClickConfirmButton(parameterRowPosition);

            string err = toastAlertSteps.CheckAlertPopup(AlertStatus.Success);

            List<string> errors = new List<string>();

            errors.Add(err);

            return ErrorHelper.ConvertErrorsToString(errors, "Parameter editing failed: ");
        }

        public string DeleteParameter(string title)
        {
            List<string> titles = parametersTable.GetTitleColumn();

            int parameterRowPosition = FindItemRowPosition(title, titles) + 1;

            if (parameterRowPosition < 1)
            {
                return $"Parameter '{title}' not found!";
            }

            parametersTable.ClickDeleteButton(parameterRowPosition);

            string err = confirmModalSteps.CheckConfirmModal(MessageConstants.DeleteParameterModalTitle, MessageConstants.DeleteParameterModalMessage);

            if (!string.IsNullOrEmpty(err))
            {
                return $"Modal 'Delete parameter' check failed: {err}";
            }

            confirmModalSteps.ClickButtonInConfirmModal("Delete");

            bool isModalDisappeared = confirmModalSteps.IsModalDisappeared();

            if (!isModalDisappeared)
            {
                return "Modal 'Delete parameter' did not disappear";
            }

            // check if parameter is not displayed in parameter table
            titles = parametersTable.GetTitleColumn();

            parameterRowPosition = FindItemRowPosition(title, titles) + 1;

            if (parameterRowPosition > 0)
            {
                return $"Parameter '{title}' found in table after deleting!";
            }

            err = toastAlertSteps.CheckAlertPopup(AlertStatus.Success);

            return ErrorHelper.AddPrefixToError(err, "Parameter deleting failed: ");
        }

        public string CheckParameterProperties(string title, JourneyParameterType expectedType, string expectedDescription)
        {
            List<string> titles = parametersTable.GetTitleColumn();

            int parameterRowPosition = FindItemRowPosition(title, titles) + 1;

            List<string> errors = new List<string>();

            if (parameterRowPosition < 1)
            {
                return $"Parameter '{title}' not created!";
            }

            string actualTitle = parametersTable.GetTitle(parameterRowPosition);

            if (title != actualTitle)
            {
                errors.Add($"Invalid title: {actualTitle}, expected: {title}");
            }

            string actualType = parametersTable.GetType(parameterRowPosition);

            if (actualType != expectedType.ToString())
            {
                errors.Add($"Invalid type: {actualType}, expected: {expectedType}");
            }

            string actualDescription = parametersTable.GetDescription(parameterRowPosition);

            if (actualDescription != expectedDescription)
            {
                errors.Add($"Invalid description: {actualDescription}, expected: {expectedDescription}");
            }

            return ErrorHelper.ConvertErrorsToString(errors, "Parameter check failed: ");
        }

        public void ExpandParameter(string title)
        {
            List<string> titles = parametersTable.GetTitleColumn();

            int parameterRowPosition = FindItemRowPosition(title, titles) + 1;

            if (parameterRowPosition > 0 && !parametersTable.IsParameterExpanded(parameterRowPosition))
            {
                parametersTable.ClickExpandToggle(parameterRowPosition);

                spinner.WaitSpinnerToDisappear(SpinnerType.JourneyEditorParameterExpanding);
            }
        }

        public void CollapseParameter(string title)
        {
            List<string> titles = parametersTable.GetTitleColumn();

            int parameterRowPosition = FindItemRowPosition(title, titles) + 1;

            if (parameterRowPosition > 0 && parametersTable.IsParameterExpanded(parameterRowPosition))
            {
                parametersTable.ClickExpandToggle(parameterRowPosition);

                UiWaitHelper.Wait(() => !parametersTable.IsParameterExpanded(parameterRowPosition), WaitConstants.CheckElementExistInSec);
            }
        }

        public JourneyStepType GetParameterType(string title)
        {
            List<string> titles = parametersTable.GetTitleColumn();

            int parameterRowPosition = FindItemRowPosition(title, titles) + 1;

            JourneyStepType journeyStepType = JourneyStepType.None;

            if (parameterRowPosition > 0)
            {
                Enum.TryParse(parametersTable.GetType(parameterRowPosition), out journeyStepType);
            }

            return journeyStepType;
        }

        //public JourneyStepType GetParameterDescription(string title)
        //{
        //    List<string> titles = parametersTable.GetTitleColumn();

        //    int parameterRowPosition = FindItemRowPosition(title, titles) + 1;

        //    JourneyStepType journeyStepType = JourneyStepType.None;

        //    if (parameterRowPosition > 0)
        //    {
        //        Enum.TryParse(parametersTable.GetType(parameterRowPosition), out journeyStepType);
        //    }

        //    return journeyStepType;
        //}

        public Parameter GetParameterData(string title)
        {
            JourneyStepType parameterType = GetParameterType(title);

            Parameter parameter = new Parameter();

            if (parameterType == JourneyStepType.None)
            {
                return parameter;
            }
            else if (parameterType == JourneyStepType.Infinity || parameterType == JourneyStepType.Interval)
            {
                int itemsCount = parameterScale.GetRatedItemCount();

                List<RatedDevice> ratedDevices = new List<RatedDevice>();

                for (int i = 0; i < itemsCount; i++)
                {
                    RatedDevice device = new RatedDevice();

                    device.Manufacturer = parameterScale.GetItemBrand(i + 1);

                    device.Model = parameterScale.GetItemModel(i + 1);

                    int rating;

                    int.TryParse(parameterScale.GetItemRating(i + 1), out rating) ;

                    if (parameterType == JourneyStepType.Infinity)
                    {
                        device.Rating = rating;
                    }
                    else if (parameterType == JourneyStepType.Interval)
                    {
                        device.Interval = rating;
                    }

                    ratedDevices.Add(device);
                }

                if (parameterType == JourneyStepType.Interval)
                {
                    parameter.IntervalLabels = parameterScale.GetIntervalLabels();
                    
                    parameter.IntervalLabels.Reverse();
                }

                parameter.RatedDevices = ratedDevices;

                parameter.Title = title;
            }

            return parameter;
        }
    }
}