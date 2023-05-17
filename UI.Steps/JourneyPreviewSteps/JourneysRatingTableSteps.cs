using NLog;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models.TafAd;
using Taf.UI.PageObjects.TafAd.Taf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Taf.UI.Steps.JourneyPreviewSteps
{
    public class JourneyRatingTableSteps
    {
        private readonly JourneyStartPage journeyStartPage;

        private readonly TafAdTestDataHelper TafAdHelper;

        private readonly RatingsTable ratingsTable;

        private readonly ILogger log;

        public JourneyRatingTableSteps(ILogger logger)
        {
            journeyStartPage = new JourneyStartPage();

            TafAdHelper = new TafAdTestDataHelper();

            ratingsTable = new RatingsTable();

            log = logger;
        }

        public List<RatedDevice> ReadRatingsTable(int parameterCount)
        {
            List<RatedDevice> devices = new List<RatedDevice>();

            for (int i = 0; i < ratingsTable.GetRowCount(); i++)
            {
                RatedDevice device = new RatedDevice();

                device.Manufacturer = ratingsTable.GetManufacturer(i + 1);

                device.Model = ratingsTable.GetModel(i + 1);

                string rate;

                for (int j = 0; j < parameterCount; j++)
                {
                    rate = ratingsTable.GetParameterRating(i + 1, j + 1);

                    bool isParseSuccessful = double.TryParse(rate, NumberStyles.Any, CultureInfo.InvariantCulture, out double relativeRating);

                    if (!isParseSuccessful)
                    {
                        relativeRating = 0;
                    }

                    device.RelativeRatings.Add(relativeRating);
                }

                devices.Add(device);
            }

            return devices;
        }

        public string CheckRatingsTable(List<Parameter> selectedParameters, List<Parameter> allParameters)
        {
            List<RatedDevice> actualRows = ReadRatingsTable(allParameters.Count);

            List<RatedDevice> expectedRows = TafAdHelper.CalculateRelativeRatings(selectedParameters, allParameters);

            string selectedParametersAsString = string.Join(",", selectedParameters.Select(p => p.Title).ToList());

            string err = string.Empty;

            if (actualRows.Count != expectedRows.Count)
            {
                err = $"Invalid actual devices count: {actualRows.Count}, expected: {expectedRows.Count} (parameters selected: {selectedParametersAsString})";
            }
            else
            {
                for (int i = 0; i < actualRows.Count; i++)
                {
                    err = TafAdHelper.CompareDevices(actualRows[i], expectedRows[i]);

                    if (!string.IsNullOrEmpty(err))
                    {
                        err = $"Row {i} (Actual device: {actualRows[i].Manufacturer} {actualRows[i].Model}): {err} (actual - expected)";

                        break;
                    }
                }
            }

            LogHelper.LogResult(log, "Rating table check completed", err);

            return err;
        }

        public void SelectParameters(List<bool> parameterStates)
        {
            for (int i = 0; i < parameterStates.Count; i++)
            {
                journeyStartPage.SetCheckboxState(i + 1, parameterStates[i]);
            }
        }

        public string CheckParameterCombinations(List<Parameter> allParameters)
        {
            List<string> parameterTitles = journeyStartPage.GetParameterTitles();

            List<List<bool>> combinations = TafAdHelper.GenerateStartPageParameterCombinations(allParameters.Count);

            string err = string.Empty;

            foreach(var combination in combinations)
            {
                SelectParameters(combination);
                
                WaitTableToRefresh(); //wait table to refresh

                List<Parameter> sortedParameters = TafAdHelper.GetParametersByTitles(parameterTitles, allParameters);

                List<Parameter> selectedParameters = TafAdHelper.GetSelectedParameters(sortedParameters, combination);

                err = CheckRatingsTable(selectedParameters, sortedParameters);

                if (!string.IsNullOrEmpty(err))
                {
                    break;
                }
            }

            return err;
        }

        public void WaitTableToRefresh()
        {
            UiWaitHelper.Wait(() => ratingsTable.IsTableBusy(), WaitConstants.OneSecond);

            bool isRefreshed = UiWaitHelper.Wait(() => ratingsTable.IsTableRefreshed(), WaitConstants.HalfMinuteInSec);

            if (!isRefreshed)
            {
                throw new Exception($"Table is not refreshed in {WaitConstants.HalfMinuteInSec} s");
            }
        }

        public void WaitTableIsDisplayed()
        {
            bool isDisplayed = ratingsTable.IsTableDisplayed();

            bool isAttributeNotNull = UiWaitHelper.Wait(() => !ratingsTable.IsAttributeAriaBusyNull(), WaitConstants.CheckElementExistInSec);

            if (!isDisplayed)
            {
                throw new Exception($"Table is not displayed in {WaitConstants.HalfMinuteInSec} s");
            }
        }
    }
}
