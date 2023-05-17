namespace Taf.UI.Core.Models
{
    public class DropdownMenuItem
    {
        public string Name { get; set; }

        public bool HasImage { get; set; } = false;

        public bool IsImageDisplayed { get; set; } = false;

        public TafEmBranchData ToBranchData()
        {
            return new TafEmBranchData()
            {
                Answer = Name,
                HasImage = HasImage,
                IsImageDisplayed = IsImageDisplayed
            };
        }
    }
}
