using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;
using System.Collections.Generic;

namespace Taf.UI.PageObjects
{
    public class ImageBlock : ContentBlockBase
    {
        public ImageBlock(App app, bool isRedesign=false) : base(app, isRedesign)
        {
            locators = SetLocators(app, locatorsCommon, locatorsEmbed, locatorsTaf,
                agentsRedesign: locatorsTafRedesign, isRedesign: isRedesign);
        }

        private readonly Dictionary<string, string> locators;

        private readonly Dictionary<string, string> locatorsCommon =

            new Dictionary<string, string>()
            {

            };

        private readonly Dictionary<string, string> locatorsTaf =

            new Dictionary<string, string>()
            {
                { "image", "//span[contains(@class,'img-wrap')]//img"},
                { "image title", "/div/h2"}
            };

        private readonly Dictionary<string, string> locatorsEmbed =

            new Dictionary<string, string>()
            {
                { "image", "//span[contains(@class,'img-wrap')]//img"},
                { "image title", "/div/h4"}
            };

        private readonly Dictionary<string, string> locatorsTafRedesign =

           new Dictionary<string, string>()
           {
                { "image", "//span[contains(@class,'image')]/img"},
                { "image title", "//div[@class='image-block']/h2"}
           };

        public string ImageInDxFlowProcessXpath(int pathItemNum, int blockNum) =>
            DxFlowProcessContentBlockXpath(pathItemNum, blockNum) + GetXpath("image", locators);

        public string GetImageRelativeXpath() => GetXpath("image", locators);

        public string GetImageTitle(string baseXpath) => new Element(baseXpath + GetXpath("image title", locators)).Text;
    }
}
