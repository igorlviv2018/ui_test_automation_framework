using NLog;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using System.Collections.Generic;
using Taf.UI.PageObjects.Authoring;
using Taf.UI.Core.Models.TafAd;

namespace Taf.UI.Steps.TafAdSteps
{
    public class JourneyContentSteps : TableBaseSteps
    {
        private readonly JourneyContent journeyContentTab;

        public JourneyContentSteps(ILogger logger) : base(App.Taf, logger)
        {
            journeyContentTab = new JourneyContent();
        }

        public List<string> GetUsedInfinityParameterTitles()
        {
            int stepCount = journeyContentTab.GetStepCount();

            List<string> parameterTitles = new List<string>();

            for (int i = 0; i < stepCount; i++)
            {
                string rawStepType = journeyContentTab.GetStepTypeRaw(i + 1);

                if (CommonHelper.GetJourneyStepType(rawStepType) == JourneyStepType.Infinity)
                {
                    int checkboxCount = journeyContentTab.GetParameterCount(i + 1);

                    for (int j = 0; j < checkboxCount; j++)
                    {
                        bool isChecked = journeyContentTab.IsCheckboxChecked(i + 1, j + 1);

                        if (isChecked)
                        {
                            string title = journeyContentTab.GetParameterTitle(i + 1, j + 1);

                            if (!parameterTitles.Contains(title))
                            {
                                parameterTitles.Add(title);
                            }
                        }
                    }
                }
            }

            return parameterTitles;
        }

        public TestJourney GetJourneyContentFromEditor(string journeyTitle)
        {
            int stepCount = journeyContentTab.GetStepCount();

            List<string> usedParameterTitles = new List<string>(); // infinity & interval

            List<JourneyStep> steps = new List<JourneyStep>();

            for (int i = 0; i < stepCount; i++)
            {
                string rawStepType = journeyContentTab.GetStepTypeRaw(i + 1);

                JourneyStep step = new JourneyStep();

                JourneyStepType stepType = CommonHelper.GetJourneyStepType(rawStepType);

                if (stepType == JourneyStepType.Infinity|| stepType == JourneyStepType.Brands)
                {
                    int checkboxCount = journeyContentTab.GetParameterCount(i + 1);

                    for (int j = 0; j < checkboxCount; j++)
                    {
                        bool isChecked = journeyContentTab.IsCheckboxChecked(i + 1, j + 1);

                        if (isChecked)
                        {
                            string title = journeyContentTab.GetParameterTitle(i + 1, j + 1);

                            step.ParameterTitles.Add(title);

                            if (stepType == JourneyStepType.Infinity && !usedParameterTitles.Contains(title))
                            {
                                usedParameterTitles.Add(title);
                            }
                        }
                    }
                }
                else if (stepType == JourneyStepType.Interval)
                {
                    step.IntervalParameterTitle = GetSelectedIntervalParanmeter(i + 1);

                    step.ParameterTitles.Add(step.IntervalParameterTitle);

                    if (!usedParameterTitles.Contains(step.IntervalParameterTitle))
                    {
                        usedParameterTitles.Add(step.IntervalParameterTitle);
                    }
                }

                if (journeyContentTab.IsIdontMindCheckboxChecked(i + 1))
                {
                    step.HasIdontMind = true;
                }

                step.Title = journeyContentTab.GetStepTitle(i + 1);

                step.Description = journeyContentTab.GetStepDescription(i + 1);

                step.StepType = stepType;

                steps.Add(step);
            }

            TestJourney testJourney = new TestJourney()
            {
                Title = journeyTitle,
                Steps = steps,
                UsedParameterTitles = usedParameterTitles
            };

            return testJourney;
        }

        public string GetSelectedIntervalParanmeter(int stepPosition)
        {
            int radioButtonCount = journeyContentTab.GetRadioButtonCount(stepPosition);

            string label = string.Empty;

            for (int i = 0; i < radioButtonCount; i++)
            {
                if (journeyContentTab.IsRadioButtonSelected(stepPosition, i + 1))
                {
                    label = journeyContentTab.GetRadioButtonLabel(stepPosition, i + 1);

                    break;
                }
            }

            return label;
        }
    }
}