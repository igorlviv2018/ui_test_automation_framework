namespace Taf.UI.PageObjects.Authoring.ContentBlocks
{
    public class ImageBlock : BasePage
    {
        private readonly string imageInput = "//div[contains(@class,'image-editor')]/input[@type='file']";

        private readonly string imageInBranchInput = "//input[@type='file'] ";

        private readonly string loadingSpinner = "//div[@class='spinner-container']/span[@role='status']";

        public string GetLoadingSpinnerXpath(string blockXpath) => $"{blockXpath}{loadingSpinner}";

        public void SetImagePath(string blockXpath, string imageFilePath) => SetImage($"{blockXpath}{imageInput}", imageFilePath);

        public void SetImagePathInBranch(string branchXpath, string imageFilePath) => SetImage($"{branchXpath}{imageInBranchInput}", imageFilePath);
    }
}
