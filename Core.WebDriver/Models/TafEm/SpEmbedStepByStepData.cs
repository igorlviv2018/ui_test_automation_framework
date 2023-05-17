using System.Collections.Generic;

namespace Taf.UI.Core.Models
{
    public class TafEmStepByStepData
    {
        public List<TafEmStepData> Steps { get; set; } = new List<TafEmStepData>();

        public TafEmStepByStepData AddStepData(string stepTitle, string imageFilePath, params string[] linkParams)
        {
            TafEmStepData step = new TafEmStepData()
            {
                Title = stepTitle,
                ImageFilePath = imageFilePath
            };

            int paramNum = 0;

            string linkName = string.Empty;

            string uri;

            foreach (var linkParam in linkParams)
            {
                if (paramNum % 2 == 0)
                {
                    linkName = linkParam;
                }
                else
                {
                    uri = linkParam;

                    step.AddStepTextLink(linkName, linkParam);
                }

                paramNum++;
            }

            Steps.Add(step);

            return this;
        }
    }
}
