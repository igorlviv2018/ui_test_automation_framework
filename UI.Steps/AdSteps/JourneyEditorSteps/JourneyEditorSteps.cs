using NLog;
using Taf.UI.Core.Enums;
using Taf.UI.Steps.TafAdSteps;
using Taf.UI.Core.Models.TafAd;
using System.Collections.Generic;
using Taf.UI.Core.Helpers;
using System.Linq;

namespace Taf.UI.Steps.TafAdSteps
{
    public class JourneyEditorSteps //: AuthoringBaseSteps
    {
        private readonly EditorHeaderSteps editorHeaderSteps;

        private readonly JourneyTableSteps journeyTableSteps;

        private readonly JourneyParametersSteps journeyParametersSteps;

        private readonly JourneyContentSteps journeyContentSteps;

        private readonly TafAdTestDataHelper TafAdHelper;

        public JourneyEditorSteps(ILogger logger)// : base(app, logger)
        {
            journeyTableSteps = new JourneyTableSteps(logger);

            editorHeaderSteps = new EditorHeaderSteps(App.TafAd);

            journeyContentSteps = new JourneyContentSteps(logger);

            journeyParametersSteps = new JourneyParametersSteps(logger);

            TafAdHelper = new TafAdTestDataHelper();
        }

        public TestJourney GetJourneyData(string journeyTitle)
        {
            string err = journeyTableSteps.OpenJourney(journeyTitle);

            TestJourney testJourney = new TestJourney();

            if (string.IsNullOrEmpty(err))
            {
                editorHeaderSteps.OpenEditorTab("Journey");

                testJourney = journeyContentSteps.GetJourneyContentFromEditor(journeyTitle);

                editorHeaderSteps.OpenEditorTab("Parameters");

                List<Parameter> usedParameters = GetUsedParameters(testJourney.UsedParameterTitles);

                testJourney = AddParametersToSteps(testJourney, usedParameters);

                editorHeaderSteps.CloseEditor();
            }

            return testJourney;
        }

        public List<TestJourney> GetJourneyData(List<string> journeyTitles) =>
            journeyTitles.Select(t => GetJourneyData(t)).Where(j => !j.IsJourneyEmpty()).ToList();

        public List<Parameter> GetUsedParameters(List<string> usedParameterTitles)
        {
            List<Parameter> parameters = new List<Parameter>();

            foreach (var usedParameterTitle in usedParameterTitles)
            {
                journeyParametersSteps.ExpandParameter(usedParameterTitle);

                Parameter parameter = journeyParametersSteps.GetParameterData(usedParameterTitle);

                journeyParametersSteps.CollapseParameter(usedParameterTitle);

                if (!parameter.IsParameterEmpty())
                {
                    parameters.Add(parameter);
                }
            }

            return parameters;
        }

        public TestJourney AddParametersToSteps(TestJourney testJourney, List<Parameter> usedParameters)
        {
            foreach (var step in testJourney.Steps)
            {
                step.Parameters = TafAdHelper.GetParametersByTitles(step.ParameterTitles, usedParameters);
            }

            return testJourney;
        }
    }
}


