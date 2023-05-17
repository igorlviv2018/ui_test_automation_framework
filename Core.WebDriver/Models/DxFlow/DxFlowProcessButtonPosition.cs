namespace Taf.UI.Core.Models
{
    public class DxFlowProcessButtonPosition
    {
        public int ProcessPosition { get; set; } = -1;

        public int ButtonsBlockPosition { get; set; } = -1;

        public int ButtonPosition { get; set; } = -1;

        public int ProcessId { get; set; } = -1;

        public TafEmProcessButtonData ButtonData { get; set; } = new TafEmProcessButtonData();

        public override string ToString() =>
            $"button in process (Id={ProcessId}) at position: {ProcessPosition}, buttons block position: {ButtonsBlockPosition}, button position: {ButtonPosition}";
    }
}
