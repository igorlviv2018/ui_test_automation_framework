using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Models.TafAd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Taf.UI.Core.Helpers
{
    public class TafAdTestDataHelper
    {
        //-----------Journeys------------

        //001: Custom article with nested collapses (text, image, video, step-by-step)
        public List<JourneyStep> TestDataJourney001()
        {
            JourneyStep step1 = new JourneyStep() { Title = "Step 1 - infinity parameters", Description = "desc", StepType = JourneyStepType.Infinity };

            Parameter game = new Parameter() { Title = "game", Description = "gaming" };

            game.RatedDevices = new List<RatedDevice>() { 
                new RatedDevice() {Manufacturer ="Samsung", Model = "Galaxy Z Fold3 5G", Rating = 86 },//86
                new RatedDevice() {Manufacturer ="Apple", Model = "iPhone 13 Pro", Rating = 57 },
                new RatedDevice() {Manufacturer ="Samsung", Model = "Galaxy S21", Rating = 31 }
            };

            step1.Parameters.Add(game);

            Parameter busi = new Parameter() { Title = "busi", Description = "description" };

            busi.RatedDevices = new List<RatedDevice>() {
                new RatedDevice() {Manufacturer ="Samsung", Model = "Galaxy S22 Ultra", Rating = 66 },
                new RatedDevice() {Manufacturer ="Apple", Model = "iPhone 13 Pro", Rating = 95 },
                new RatedDevice() {Manufacturer ="Samsung", Model = "Galaxy S21", Rating = 43 }
            };

            step1.Parameters.Add(busi);

            step1.ParametersToSelectTitles.Add("vhg");

            step1.HasIdontMind = true;

            List<JourneyStep> journeySteps = new List<JourneyStep>();

            journeySteps.Add(step1);

            JourneyStep step2 = new JourneyStep() { Title = "Step 2 - infinity parameters", Description = "", StepType = JourneyStepType.Infinity };
            step2.Parameters.Add(game);
            step2.Parameters.Add(busi);

            step2.ParametersToSelectTitles.Add("vhg");

            step2.ParametersToSelectTitles.Add("busi");

            journeySteps.Add(step2);

            //----------interval parameter--------------
            Parameter interv = new Parameter() { Title = "interval", Description = "interval parameter" };

            interv.RatedDevices = new List<RatedDevice>() {
                new RatedDevice() {Manufacturer ="Samsung", Model = "Galaxy Z Fold3 5G", Interval = 3 },
                new RatedDevice() {Manufacturer ="Samsung", Model = "Galaxy A52s 5G", Interval = 2 },
                new RatedDevice() {Manufacturer ="Apple", Model = "iPhone 13 Pro", Interval = 1 }
            };

            interv.IntervalLabels = new List<string>() { "$", "$$", "Label" };

            JourneyStep step3 = new JourneyStep() { Title = "Step 3 - interval parameter", Description = "select interval", StepType = JourneyStepType.Interval};

            step3.Parameters.Add(interv);

            journeySteps.Add(step3);

            return journeySteps;
        }

        public List<string> GetTestJourneyTitles() => new List<string>()
            {   
                "aqa_test_001",
                "aqa_test_002",
                "aqa_test_003",
                "aqa_test_004",
                "aqa_test_005"
            };

        public int GetTestCombinationsCount(List<JourneyStep> journeySteps, JourneyCheckDepth checkDepth=JourneyCheckDepth.Minimum)
        {
            int parametersCount = GetParametersCount(journeySteps);

            int count = 0;

            if (checkDepth == JourneyCheckDepth.Minimum)
            {
                count = parametersCount < 5 ? parametersCount : parametersCount - 1;
            }
            else if (checkDepth == JourneyCheckDepth.Medium)
            {
                count = 2*parametersCount;
            }
            else if (checkDepth == JourneyCheckDepth.Maximum)
            {
                count = parametersCount * parametersCount;
            }

            return count;
        }

        public int GetParametersCount(List<JourneyStep> journeySteps)
        {
            int count = 0;

            foreach (var step in journeySteps)
            {
                if (step.StepType == JourneyStepType.Infinity)
                {
                    count += step.Parameters.Count;
                }
                else if (step.StepType == JourneyStepType.Interval)
                {
                    count += step.Parameters[0].IntervalLabels.Count;
                }

                else if (step.StepType == JourneyStepType.Brands)
                {
                    count += step.ParameterTitles.Count;
                }
            }

            return count;
        }

        public List<TestJourney> GetTestJourneysData(TestEnvironment environment) =>
            FileHelper.ReadJsonFile<List<TestJourney>>(Path.Combine(CommonHelper.GetTestDataFolderPath(),
                string.Format(CommonConstants.TestJourneysFile, environment.ToString().ToLower())));

        public string WriteToTestJourneysFile(List<TestJourney> testJourneys, TestEnvironment environment) =>
            FileHelper.WriteToJsonFile(testJourneys, Path.Combine(CommonHelper.GetTestDataFolderPath(), 
               string.Format(CommonConstants.TestJourneysFile, environment.ToString().ToLower())));

        public TestJourney GetTestJourneyData(string journeyTitle, List<TestJourney> testJourneys) =>
            testJourneys.Where(j => j.Title == journeyTitle).FirstOrDefault();

        public Parameter GetParameterByTitle(string parameterTitle, List<Parameter> parameters)
        {
            Parameter result = null;

            int parameterIndex = parameters.FindIndex(p => p.Title == parameterTitle);

            if (parameterIndex > -1)
            {
                result = parameters[parameterIndex];
            }

            return result;
        }

        public List<Parameter> GetParametersByTitle(List<string> parameterTitles, List<Parameter> parameters) =>
            parameterTitles.Select(t => GetParameterByTitle(t, parameters)).Where(p => p != null).ToList();

        public void AddToSelectedParameters(List<string> parameterTitles, JourneyStep journeyStep, List<Parameter> selectedParameters)
        {
            foreach (var parameterTitle in parameterTitles)
            {
                Parameter parameter = GetParameterByTitle(parameterTitle, journeyStep.Parameters);

                if (!IsParameterOnList(parameterTitle, selectedParameters) && parameter != null)
                {
                    selectedParameters.Add(parameter);
                }
            }
        }

        public void AddToSelectedParameters(List<Parameter> parametersToAdd, List<Parameter> selectedParameters)
        {
            foreach (var parameter in parametersToAdd)
            {
                if (!IsParameterOnList(parameter.Title, selectedParameters))
                {
                    selectedParameters.Add(parameter);
                }
            }
        }

        public void AddAllInfinityParamsToSelectedParameters(List<JourneyStep> steps, List<Parameter> selectedParameters)
        {
            foreach (var step in steps)
            {
                if (step.StepType == JourneyStepType.Infinity)
                {
                    AddToSelectedParameters(step.Parameters, selectedParameters);
                }
            }
        }

        public List<Parameter> GetAllUniqueInfinityParameters(List<JourneyStep> steps)
        {
            List<Parameter> allInfinity = new List<Parameter>();

            AddAllInfinityParamsToSelectedParameters(steps, allInfinity);

            return allInfinity;
        }

        public bool IsParameterOnList(string parameterTitle, List<Parameter> selectedParameters) => selectedParameters.FindIndex(p => p.Title == parameterTitle) > -1;

        public bool IsParameterOnList(Parameter parameterToCheck, List<Parameter> parameters) =>
            parameters.Where(p => p.Title == parameterToCheck.Title).ToList().Count > 0;

        public List<string> GetExpectedParameterTitles(JourneyStep journeyStep)
        {
            List<string> parameterNames = new List<string>();

            if (journeyStep.StepType == JourneyStepType.Infinity)
            { 
                parameterNames = journeyStep.Parameters.Select(p => p.Title).ToList();
            }
            else if (journeyStep.StepType == JourneyStepType.Interval)
            {
                parameterNames = journeyStep.Parameters[0].IntervalLabels;
            }
            else if (journeyStep.StepType == JourneyStepType.Brands)
            {
                parameterNames = journeyStep.ParameterTitles;
            }

            if (journeyStep.HasIdontMind)
            {
                parameterNames.Insert(0, CommonConstants.IdontMindText);
            }

            return parameterNames;
        }

        public double CalculateRatingRelativeToParameters(RatedDevice ratedDevice, Parameter parameter, List<Parameter> parameters)
        {
            int deviceRating = GetDeviceRating(ratedDevice, parameter);

            double relativeRating = 100 * ((double)deviceRating / GetMaxRating(parameter)) / GetUniqueRatingCount(ratedDevice, parameters);

            return relativeRating;
        }

        public int GetDeviceRating(RatedDevice ratedDevice, Parameter parameter)
        {
            RatedDevice device = parameter
                .RatedDevices
                .FirstOrDefault(d => (d.Manufacturer == ratedDevice.Manufacturer) && (d.Model == ratedDevice.Model));

            return device == null ? 0 : device.Rating;
        }

        public int GetMaxRating(Parameter parameter) => 
            parameter.RatedDevices.Count > 0 ? parameter.RatedDevices.Select(d => d.Rating).Max() : 0;

        public int GetUniqueRatingCount(RatedDevice ratedDevice, List<Parameter> parameters) =>
            parameters.Select(p => GetDeviceRating(ratedDevice, p)).Distinct().ToList().Count;

        //public List<RatedDevice> GetItemsListSortedByRatingDesc(List<Parameter> selectedParameters)
        //{
        //    List<RatedDevice> resultDevices = new List<RatedDevice>();

        //    foreach (var parameter in selectedParameters)
        //    {
        //        foreach (var device in parameter.RatedDevices)
        //        {
        //            if (!IsDeviceOnList(device, resultDevices))
        //            {
        //                double rating = CalculateRating(device, selectedParameters);

        //                device.TotalRating = rating;

        //                resultDevices.Add(device);
        //            }
        //        }
        //    }

        //    resultDevices = resultDevices.OrderByDescending(d => d.TotalRating).ToList();

        //    return resultDevices;
        //}

        public List<RatedDevice> GetItemsListSortedByRatingDesc(List<Parameter> selectedParameters, List<Parameter> intervalParameters, List<string> selectedBrands)
        {
            List<RatedDevice> resultDevices = new List<RatedDevice>();

            foreach (var parameter in selectedParameters)
            {
                foreach (var device in parameter.RatedDevices)
                {
                    if (!IsDeviceOnList(device, resultDevices))
                    {
                        double rating = CalculateRating(device, selectedParameters);

                        device.TotalRating = rating;

                        resultDevices.Add(device);
                    }
                }
            }

            int positionInList;

            foreach (var parameter in intervalParameters)
            {
                foreach (var device in parameter.RatedDevices)
                {
                    positionInList = GetDevicePositionInList(device, resultDevices);

                    if (positionInList > -1)
                    {
                        resultDevices[positionInList].IntervalRating += device.IntervalRating;
                        
                        resultDevices[positionInList].TotalRating += device.IntervalRating;


                    }
                    else
                    {
                        device.TotalRating = device.IntervalRating;

                        resultDevices.Add(device);
                    }
                }
            }

            resultDevices = resultDevices.OrderByDescending(d => d.TotalRating)
                .ThenBy(d => d.Manufacturer)
                .ThenBy(d => d.Model)
                .ToList();

            if (selectedBrands.Count > 0)
            {
                resultDevices = resultDevices.Where(d => selectedBrands.Contains(d.Manufacturer)).ToList();
            }

            return resultDevices;
        }

        public List<RatedDevice> CalculateRelativeRatings(List<Parameter> selectedParameters, List<Parameter> allParameters)
        {
            List<RatedDevice> resultDevices = GetItemsListSortedByRatingDesc(selectedParameters, new List<Parameter>(), new List<string>());

            double relativeRating;

            foreach (var resultDevice in resultDevices)
            {
                resultDevice.RelativeRatings.Clear();

                foreach (var parameter in allParameters)
                {
                    if (IsParameterOnList(parameter, selectedParameters))
                    {
                        // calculate relative rating against each parameter
                        relativeRating = CalculateRatingRelativeToParameters(resultDevice, parameter, selectedParameters);

                        relativeRating = Math.Round(relativeRating, 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        relativeRating = 0;
                    }

                    resultDevice.RelativeRatings.Add(relativeRating);
                }
            }

            return resultDevices;
        }

        public bool IsDeviceOnList(RatedDevice deviceToCheck, List<RatedDevice> ratedDevices) =>
            ratedDevices
            .Where(d => d.Manufacturer == deviceToCheck.Manufacturer && d.Model == deviceToCheck.Model).ToList().Count > 0;

        public double CalculateRating(RatedDevice ratedDevice, List<Parameter> selectedParameters)
        {
            double weight = 0;

            foreach (var parameter in selectedParameters)
            {
                int maxRating = GetMaxRating(parameter);

                if (maxRating > 0)
                {
                    weight += (double)GetDeviceRating(ratedDevice, parameter) / maxRating;
                }
            }

            int parameterCount = selectedParameters.Count;

            double deviceRating = parameterCount > 0 ? weight / parameterCount : 0;

            return deviceRating;
        }

        public double CalculateIntervalRating(RatedDevice ratedDevice, List<int> selectedIntervals)
        {
            double deviceRating = 0;

            if (ratedDevice.Interval > 0)
            {
                if (selectedIntervals.Contains(ratedDevice.Interval))
                {
                    deviceRating += 1;
                }

                deviceRating += 0.1 * GetSelectedNeighbourIntervalsCount(ratedDevice, selectedIntervals);
            }

            return deviceRating;
        }

        public int GetSelectedNeighbourIntervalsCount(RatedDevice ratedDevice, List<int> selectedIntervals)
        {
            int count = 0;

            if (selectedIntervals.Contains(ratedDevice.Interval + 1))
            {
                count++;
            }

            if (selectedIntervals.Contains(ratedDevice.Interval - 1))
            {
                count++;
            }

            return count;
        }

        public void AddIntervalRatings(Parameter intervalParameter, List<int> selectedIntervals)
        {
            foreach (var device in intervalParameter.RatedDevices)
            {
                device.IntervalRating = CalculateIntervalRating(device, selectedIntervals);
            }
        }

        //public List<RatedDevice> GetItemsListWithIntervalRatings(List<Parameter> intervalParameters)
        //{
        //    List<RatedDevice> result = new List<RatedDevice>();

        //    int positionInList;

        //    foreach (var parameter in intervalParameters)
        //    {
        //        foreach (var device in parameter.RatedDevices)
        //        {
        //            positionInList = GetDevicePositionInList(device, result);

        //            if (positionInList > -1)
        //            {
        //                result[positionInList].IntervalRating += device.IntervalRating;
        //            }
        //            else
        //            {
        //                result.Add(device);
        //            }
        //        }
        //    }

        //    return result;
        //}

        public int GetDevicePositionInList(RatedDevice ratedDevice, List<RatedDevice> ratedDevices) =>
            ratedDevices.FindIndex(d => d.Manufacturer == ratedDevice.Manufacturer && d.Model == ratedDevice.Model);

        public string GetDeviceListAsString(List<RatedDevice> devices)
        {
            List<string> items = devices.Select(d => GetDeviceAsString(d)).ToList();

            return string.Join("; ", items);
        }

        public string GetDeviceAsString(RatedDevice device) => $"{device.Manufacturer} {device.Model}, R={device.TotalRating.ToString("0.##")}";

        public string CompareDevices(RatedDevice deviceA, RatedDevice deviceB)
        {
            string err;

            List<string> errors = new List<string>();

            if (deviceA.Manufacturer != deviceB.Manufacturer || deviceA.Model != deviceB.Model)
            {
                err = $"Device manufacturer and/or model mismatch: {deviceA.Manufacturer} {deviceA.Model}, {deviceB.Manufacturer} {deviceB.Model}";

                errors.Add(err);
            }

            //compare relative ratings
            if (deviceA.RelativeRatings.Count != deviceB.RelativeRatings.Count)
            {
                err = $"Device relative ratings count mismatch: {deviceA.RelativeRatings.Count}, {deviceB.RelativeRatings.Count}";

                errors.Add(err);
            }
            else
            {
                for (int i = 0; i < deviceA.RelativeRatings.Count; i++)
                {
                    if (Math.Abs(deviceA.RelativeRatings[i] - deviceB.RelativeRatings[i]) > 0.01)
                    {
                        err = $"Relative ratings at position {i} mismatch: {deviceA.RelativeRatings[i]} {deviceB.RelativeRatings[i]}";

                        errors.Add(err);
                    }
                }
            }

            return ErrorHelper.ConvertErrorsToString(errors, "Device comparison failed: ");
        }

        public List<bool> GenerateRandomBoolList(int numOfBools)
        {
            List<bool> randomList = new List<bool>();

            Random rand = new Random();

            for (int i = 0; i < numOfBools; i++)
            {
                randomList.Add(rand.NextDouble() > 0.5);
            }

            return randomList;
        }

        public List<bool> GenerateAllFalsesBoolList(int numOfBools)
        {
            List<bool> allFalsesList = new List<bool>();

            for (int i = 0; i < numOfBools; i++)
            {
                allFalsesList.Add(false);
            }

            return allFalsesList;
        }

        public List<bool> GenerateNotBoundaryCombination(int numOfBools)
        {
            List<bool> randomList = new List<bool>() { true };

            if (numOfBools == 1)
            {
                return randomList;
            }

            randomList = GenerateRandomBoolList(numOfBools);

            while (IsConbinationAllFalses(randomList) || IsConbinationAllTrues(randomList))
            {
                randomList = GenerateRandomBoolList(numOfBools);
            }

            return randomList;
        }

        public List<List<bool>> GenerateStartPageParameterCombinations(int parameterCount)
        {
            List<List<bool>> combinations = new List<List<bool>>();

            List<bool> allTruesCombination = Enumerable.Repeat(true, parameterCount).ToList();

            combinations.Add(allTruesCombination);

            List<bool> combination;

            if (parameterCount >= 2)
            {
                for (int i = 0; i < parameterCount; i++)
                {
                    combination = GenerateNotBoundaryCombination(parameterCount);

                    combinations.Add(combination);
                }

                if (parameterCount == 2)
                {
                    combinations.RemoveAt(2);
                }
            }

            return combinations;
        }

        public void SetJourneyParameterCombinationInSteps(List<JourneyStep> steps, bool generateAllFalsesCombination=false)
        {
            foreach (var step in steps)
            {
                //int stepParametersCount = step.StepType == JourneyStepType.Infinity ? step.Parameters.Count : step.Parameters[0].IntervalLabels.Count;
                int stepParametersCount = GetStepParametersCount(step);

                if (step.StepType == JourneyStepType.Infinity || step.StepType == JourneyStepType.Interval || step.StepType == JourneyStepType.Brands)
                {
                    step.SelectedParameters = generateAllFalsesCombination
                        ? GenerateAllFalsesBoolList(stepParametersCount)
                        : GenerateRandomBoolList(stepParametersCount);
                }
            }
        }

        public int GetStepParametersCount(JourneyStep step)
        {
            int stepParametersCount = 0;

            if (step.StepType == JourneyStepType.Infinity)
            {
                stepParametersCount = step.Parameters.Count;
            }
            else if (step.StepType == JourneyStepType.Interval)
            {
                stepParametersCount = step.Parameters[0].IntervalLabels.Count;
            }
            else if (step.StepType == JourneyStepType.Brands)
            {
                stepParametersCount = step.ParameterTitles.Count;
            }

            return stepParametersCount;
        }

        public string GetJourneyParameterCombinationAsText(List<JourneyStep> steps)
        {
            List<string> selectedParametersInSteps = new List<string>();

            foreach (var step in steps)
            {
                string description = $"'{step.Title}' step: {string.Join(", ", step.ParametersToSelectTitles)}";

                selectedParametersInSteps.Add(description);
            }

            return string.Join("; ", selectedParametersInSteps);
        }

        public bool IsConbinationAllFalses(List<bool> combination) => !combination.Contains(true);

        public bool IsConbinationAllTrues(List<bool> combination) => !combination.Contains(false);

        public bool IsNoneInfinityParameterSelected(List<JourneyStep> steps)
        {
            bool isNoneSelected = true;

            foreach (var step in steps)
            {
                if (step.StepType == JourneyStepType.Infinity && !IsConbinationAllFalses(step.SelectedParameters))
                {
                    isNoneSelected = false;

                    break;
                }
            }

            return isNoneSelected;
        }

        public List<Parameter> GetParametersByTitles(List<string> titles, List<Parameter> parameters)
        {
            List<Parameter> result = new List<Parameter>();

            foreach (var title in titles)
            {
                int index = parameters.FindIndex(p => p.Title == title);

                if (index > -1)
                {
                    result.Add(parameters[index]);
                }
            }

            return result;
        }

        public List<Parameter> GetSelectedParameters(List<Parameter> parameters, List<bool> combination)
        {
            List<Parameter> result = new List<Parameter>();

            for (int i = 0; i < combination.Count; i++)
            {
                if (combination[i])
                {
                    result.Add(parameters[i]);
                }
            }

            return result;
        }

        public List<int> GetIntervals(List<bool> combination)
        {
            List<int> intervals = new List<int>();

            for (int i = 0; i < combination.Count; i ++)
            {
                if (combination[i])
                {
                    intervals.Add(i + 1);
                }
            }

            return intervals;
        }
    }
}
