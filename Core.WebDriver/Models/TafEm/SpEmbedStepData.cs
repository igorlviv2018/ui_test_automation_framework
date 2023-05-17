namespace Taf.UI.Core.Models
{
    public class TafEmStepData
    {
        public string Title { get; set; } = string.Empty;

        public string ImageFilePath { get; set; } = string.Empty;

        public TafEmTextBlockData TextData { get; set; } = new TafEmTextBlockData();

        public void AddStepTextLink(string linkText, string linkUri)
        {
            TextData.AddLink(linkText, linkUri);
        }
    }
}
