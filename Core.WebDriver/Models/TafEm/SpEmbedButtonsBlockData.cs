using Taf.UI.Core.Enums;
using System.Collections.Generic;

namespace Taf.UI.Core.Models
{
    public class TafEmButtonsBlockData
    {
        public List<TafEmProcessButtonData> Buttons { get; set; } = new List<TafEmProcessButtonData>();

        public TafEmButtonsBlockData AddButton(ProcessButtonClickAction buttonClickAction, string label, string url="", LinkButtonTarget buttonTarget=LinkButtonTarget.NewTab)
        {
            Buttons.Add(
                new TafEmProcessButtonData()
                {
                    ClickAction = buttonClickAction,
                    LinkButtonTarget = buttonTarget,
                    Label = label,
                    Url = url
                });

            return this;
        }
    }
}
