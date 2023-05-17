using Taf.UI.Core.Configuration;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Taf.UI.Core.Helpers
{
    public class CommonHelper
    {
        private static readonly TestConfiguration testConfig = new TestConfiguration();

        public static AppsMenuItemType GetAppsMenuItemType(AppsMenuItem appsMenuItem)
        {
            AppsMenuItemType type;

            if (appsMenuItem.Role == "separator")
            {
                type = AppsMenuItemType.Separator;
            }
            else if (appsMenuItem.Role == "menuitem")
            {
                type = appsMenuItem.Href.Contains("#")
                    ? AppsMenuItemType.ClientName
                    : AppsMenuItemType.AppLink;
            }
            else if (appsMenuItem.Role == "heading")
            {
                type = AppsMenuItemType.Heading;
            }
            else
            {
                type = AppsMenuItemType.Unknown;
            }

            return type;
        }

        public static App GetApp(string href)
        {
            App app;

            if (href == null || string.IsNullOrEmpty(href))
            {
                app = App.None;
            }
            else if (href.Contains("authoring"))
            {
                app = App.TafAuth;
            }
            else if (href.Contains("advisor"))
            {
                app = App.TafAd;
            }
            else if (href.Contains("admin/config/general"))
            {
                app = App.SystemConfiguration;
            }
            else if (href.Contains("admin/users"))
            {
                app = App.UserManagement;
            }
            else if (href.Contains("admin/self-service/settings"))
            {
                app = App.TafSelfServiceConfig;
            }
            else if (href.Contains("admin/clients"))
            {
                app = App.ClientManagement;
            }
            else
            {
                app = App.Taf;
            }

            return app;
        }

        //public static App GetAppByAppLink(AppLinkType appLinkType)
        //{
        //    App app = App.None;

        //    if (appLinkType == AppLinkType.Taf)
        //    {
        //        app = App.Taf;
        //    }
        //    else if (appLinkType == AppLinkType.TafAuth)
        //    {
        //        app = App.TafAuth;
        //    }
        //    else if (appLinkType == AppLinkType.TafAd)
        //    {
        //        app = App.TafAd;
        //    }

        //    return app;
        //}

        //public static bool IsAppAtStartPage(string href, AppLinkType appLinkType)
        //{
        //    bool isAtStartPage = false;

        //    string linkLastPart = href.Split("/").Last();

        //    if (appLinkType == AppLinkType.TafAuth && linkLastPart == "articles")
        //    {
        //        isAtStartPage = true;
        //    }
        //    else if (appLinkType == AppLinkType.TafAd && (linkLastPart == "journeys?tab=active" || linkLastPart == "journeys"))
        //    {
        //        isAtStartPage = true;
        //    }
        //    else if (appLinkType == AppLinkType.Taf && linkLastPart == string.Empty)
        //    {
        //        isAtStartPage = true;
        //    }
        //    else if (appLinkType == AppLinkType.UserManagement && linkLastPart == "users")
        //    {
        //        isAtStartPage = true;
        //    }

        //    return isAtStartPage;
        //}

        //public static string GetRelativeAppLink(AppLinkType appLinkType)
        //{
        //     string relativeLink = string.Empty;

        //    if (appLinkType == AppLinkType.None)
        //    {
        //        relativeLink = "";
        //    }
        //    else if (appLinkType == AppLinkType.Taf)
        //    {
        //        relativeLink = "/";
        //    }
        //    else if (appLinkType == AppLinkType.TafAuth)
        //    {
        //        relativeLink = "/authoring";
        //    }
        //    else if (appLinkType == AppLinkType.TafAd)
        //    {
        //        relativeLink = "/advisor";
        //    }
        //    else if (appLinkType == AppLinkType.SystemConfiguration)
        //    {
        //        relativeLink = "/admin/config/general";
        //    }
        //    else if (appLinkType == AppLinkType.UserManagement)
        //    {
        //        relativeLink = "/admin/users";
        //    }
        //    else if (appLinkType == AppLinkType.TafSelfServiceConfig)
        //    {
        //        relativeLink = "/admin/self-service/settings";
        //    }
        //    else if (appLinkType == AppLinkType.ClientManagement)
        //    {
        //        relativeLink = "/admin/clients";
        //    }

        //    return relativeLink;
        //}

        public static string GetIdByApp(App app)
        {
            string id = "none";

            if (app == App.Taf)
            {
                id = "systemconfigmenu";
            }
            else if (app == App.SelfService)
            {
                id = "selfservicemenu";
            }
            else if (app == App.Retail)
            {
                id = "retailapp";
            }
            else if (app == App.TafAd)
            {
                id = "advisor";
            }
            else if (app == App.UserManagement)
            {
                id = "usermgmtmenu";
            }

            return id;
        }

        public static string GetAppName(App app)
        {
            string name = "none";

            if (app == App.Taf)
            {
                name = "SP Agents";
            }
            else if (app == App.SelfService)
            {
                name = "SP Self-Service";
            }
            else if (app == App.Retail)
            {
                name = "SP Retail";
            }
            else if (app == App.TafAd)
            {
                name = "SP Advisor";
            }
            else if (app == App.UserManagement)
            {
                name = "User management";
            }

            return name;
        }

        public static bool IsAppAtStartPage(string href, App app)
        {
            bool isAtStartPage = false;

            string linkLastPart = href.Split("/").Last();

            if (app == App.TafAuth && linkLastPart == "articles")
            {
                isAtStartPage = true;
            }
            else if (app == App.TafAd && (linkLastPart == "journeys?tab=active" || linkLastPart == "journeys"))
            {
                isAtStartPage = true;
            }
            else if (app == App.Taf && linkLastPart == string.Empty)
            {
                isAtStartPage = true;
            }
            else if (app == App.UserManagement && linkLastPart == "users")
            {
                isAtStartPage = true;
            }

            return isAtStartPage;
        }

        public static string GetRelativeAppLink(App app)
        {
            string relativeLink = string.Empty;

            if (app == App.None || app == App.Embed)
            {
                relativeLink = "";
            }
            else if (app == App.Taf)
            {
                relativeLink = "/";
            }
            else if (app == App.TafAuth)
            {
                relativeLink = "/authoring";
            }
            else if (app == App.TafAd)
            {
                relativeLink = "/advisor";
            }
            else if (app == App.SystemConfiguration)
            {
                relativeLink = "/admin/config/general";
            }
            else if (app == App.UserManagement)
            {
                relativeLink = "/admin/users";
            }
            else if (app == App.TafSelfServiceConfig)
            {
                relativeLink = "/admin/self-service/settings";
            }
            else if (app == App.ClientManagement)
            {
                relativeLink = "/admin/clients";
            }

            return relativeLink;
        }

        public static PathItemType GetPathItemType(string rawType)
        {
            PathItemType type = PathItemType.Undefined;

            if (rawType.Contains("type-process"))
            {
                type = PathItemType.Process;
            }
            else if (rawType.Contains("type-predprocess"))
            {
                type = PathItemType.PredefinedProcess;
            }
            else if (rawType.Contains("type-decision"))
            {
                type = PathItemType.Decision;
            }
            else if (rawType.Contains("type-terminator"))
            {
                type = PathItemType.Terminator;
            }
            else if (rawType.Contains("type-extconnector"))
            {
                type = PathItemType.ExternalConnector;
            }

            return type;
        }

        public static ArticleContentElementType GetContentElementType(string type, bool isRedesign=false)
        {
            ArticleContentElementType elementType;

            if (string.IsNullOrEmpty(type))
            {
                elementType = ArticleContentElementType.Undefined;
            }
            else if (type.Contains("html") || type.Contains("text-block"))
            {
                elementType = ArticleContentElementType.Text;
            }
            else if (type.Contains("img")|| type.Contains("image-block"))
            {
                elementType = ArticleContentElementType.Image;
            }
            else if (type.Contains("video"))
            {
                elementType = ArticleContentElementType.Video;
            }
            else if (type.Contains("accordion"))
            {
                elementType = ArticleContentElementType.Accordion;
            }
            else if (!isRedesign && type.Contains("collapse") || isRedesign && type.Contains("rounded"))
            {
                elementType = ArticleContentElementType.Collapse;
            }
            else if (type.Contains("steps") || type.Contains("step-by-step"))
            {
                elementType = ArticleContentElementType.StepByStep;
            }
            else if (type.Contains("buttons-block"))
            {
                elementType = ArticleContentElementType.ButtonsBlock;
            }
            else
            {
                elementType = ArticleContentElementType.Undefined;
            }

            return elementType;
        }

        public static string GetShortType(ArticleContentElementType type)
        {
            string shortType;

            if (type == ArticleContentElementType.ButtonsBlock)
            {
                shortType = "BtnBlock";
            }
            else if (type == ArticleContentElementType.InternalConnector)
            {
                shortType = "IntConnect";
            }
            else if (type == ArticleContentElementType.ExternalConnector)
            {
                shortType = "ExtConnect";
            }
            else if (type == ArticleContentElementType.Image)
            {
                shortType = "Img";
            }
            else if (type == ArticleContentElementType.PredefinedProcess)
            {
                shortType = "PredProc";
            }
            else if (type == ArticleContentElementType.StepByStep)
            {
                shortType = "StepByStep";
            }
            else if (type == ArticleContentElementType.Text)
            {
                shortType = "Txt";
            }
            else
            {
                shortType = $"{type}"[0].ToString();
            }

            return shortType;
        }

        public static ArticleContentElementType GetContentElementType(object type)
        {
            ArticleContentElementType elementType;

            if (type.GetType() == typeof(TafEmTextBlockData))
            {
                elementType = ArticleContentElementType.Text;
            }
            else if (type.GetType() == typeof(TafEmStepByStepData))
            {
                elementType = ArticleContentElementType.StepByStep;
            }
            else if (type.GetType() == typeof(TafEmImageData))
            {
                elementType = ArticleContentElementType.Image;
            }
            else if (type.GetType() == typeof(TafEmButtonsBlockData))
            {
                elementType = ArticleContentElementType.ButtonsBlock;
            }
            else if (type.GetType() == typeof(TafEmVideoBlockData))
            {
                elementType = ArticleContentElementType.Video;
            }
            else
            {
                elementType = ArticleContentElementType.Undefined;
            }

            return elementType;
        }

        public static string GetAuthoringInstrumentBlockTitle(TafEmArticleElement element)
        {
            string instrumentBlockName;

            if (element.ElementType == ArticleContentElementType.StepByStep)
            {
                instrumentBlockName = "Step-by-step";
            }
            else if (element.ElementType == ArticleContentElementType.RichText)
            {
                instrumentBlockName = "Rich text";
            }
            else if (element.ElementType == ArticleContentElementType.ButtonsBlock)
            {
                instrumentBlockName = "Button";
            }
            else
            {
                instrumentBlockName = element.ElementType.ToString();
            }

            return instrumentBlockName;
        }

        public static ArticleContentElementType GetAuthoringContentElementType(string type)
        {
            ArticleContentElementType elementType;

            if (string.IsNullOrEmpty(type))
            {
                elementType = ArticleContentElementType.Undefined;
            }
            else if (type.Contains("block-text"))
            {
                elementType = ArticleContentElementType.Text;
            }
            else if (type.Contains("block-image"))
            {
                elementType = ArticleContentElementType.Image;
            }
            else if (type.Contains("block-video"))
            {
                elementType = ArticleContentElementType.Video;
            }
            else if (type.Contains("block-accordion"))
            {
                elementType = ArticleContentElementType.Accordion;
            }
            else if (type.Contains("collapse-block"))
            {
                elementType = ArticleContentElementType.Collapse;
            }
            else if (type.Contains("step-by-step"))
            {
                elementType = ArticleContentElementType.StepByStep;
            }
            else if (type.Contains("step-block"))
            {
                elementType = ArticleContentElementType.StepInStepByStep;
            }
            else if (type.Contains("block-buttons"))
            {
                elementType = ArticleContentElementType.ButtonsBlock;
            }
            else if (type.Contains("block-table"))
            {
                elementType = ArticleContentElementType.Table;
            }
            else
            {
                elementType = ArticleContentElementType.Undefined;
            }

            return elementType;
        }

        public static ArticleContentElementType GetElementTypeInProperties(string type)
        {
            ArticleContentElementType elementType = ArticleContentElementType.Undefined;

            if (string.IsNullOrEmpty(type))
            {
                elementType = ArticleContentElementType.Undefined;
            }
            else if (type.Contains("Step-by-step"))
            {
                elementType = ArticleContentElementType.StepByStep;
            }
            else if (type == "Step")
            {
                elementType = ArticleContentElementType.StepInStepByStep;
            }
            //else if (type.Contains("Button"))
            //{
            //    elementType = ArticleContentElementType.ButtonsBlock;
            //}
            else if (Enum.TryParse(type, out ArticleContentElementType result))
            {
                elementType = result;
            }

            return elementType;
        }

        public static string GetFlowBlockTypeInAddBlockDropdown(ArticleContentElementType type)
        {
            string blockType;

            if (type == ArticleContentElementType.PredefinedProcess)
            {
                blockType = "predprocess";
            }
            else if (type == ArticleContentElementType.InternalConnector)
            {
                blockType = "intconnector";
            }
            else if (type == ArticleContentElementType.ExternalConnector)
            {
                blockType = "extconnector";
            }
            else
            {
                blockType = $"{type}".ToLower();
            }

            return blockType;
        }

        public static string GetFlowBlockMenuItem(AuthoringFlowBlockMenuItem menuItem)
        {
            string menuItemString = string.Empty;

            if (menuItem == AuthoringFlowBlockMenuItem.Copy)
            {
                menuItemString = "ss-layers";
            }
            else if (menuItem == AuthoringFlowBlockMenuItem.Remove)
            {
                menuItemString = "ss-trash";
            }
            else if (menuItem == AuthoringFlowBlockMenuItem.AddImage)
            {
                menuItemString = "ss-picture";
            }
            else if (menuItem == AuthoringFlowBlockMenuItem.RemoveImage)
            {
                menuItemString = "ss-delete";
            }

            return menuItemString;
        }

        public static string GetClickActionInFlowButtonsBlock(ProcessButtonClickAction clickAction)
        {
            string clickActionOptionValue = string.Empty;

            if (clickAction == ProcessButtonClickAction.NextStep)
            {
                clickActionOptionValue = "3";
            }
            else if (clickAction == ProcessButtonClickAction.PreviousStep)
            {
                clickActionOptionValue = "4";
            }
            else if (clickAction == ProcessButtonClickAction.RestartFlow)
            {
                clickActionOptionValue = "5";
            }
            else if (clickAction == ProcessButtonClickAction.Link)
            {
                clickActionOptionValue = "1";
            }
            else if (clickAction == ProcessButtonClickAction.JavaScript)
            {
                clickActionOptionValue = "2";
            }

            return clickActionOptionValue;
        }

        public static ArticleContentElementType GetFlowBlockType(string type)
        {
            ArticleContentElementType elementType;

            if (type.StartsWith("process"))
            {
                elementType = ArticleContentElementType.Process;
            }
            else if (type.StartsWith("predprocess"))
            {
                elementType = ArticleContentElementType.PredefinedProcess;
            }
            else if (type.Contains("decision"))
            {
                elementType = ArticleContentElementType.Decision;
            }
            else if (type.Contains("terminator"))
            {
                elementType = ArticleContentElementType.Terminator;
            }
            else if (type.Contains("int-connector"))
            {
                elementType = ArticleContentElementType.InternalConnector;
            }
            else if (type.Contains("ext-ref"))
            {
                elementType = ArticleContentElementType.ExternalConnector;
            }
            else
            {
                elementType = ArticleContentElementType.Undefined;
            }

            return elementType;
        }

        public static string GetProcessInnerBlockType(object blockData)
        {
            string blockType = "Unknown";

            if (blockData.GetType() == typeof(TafEmTextBlockData))
            {
                blockType = "Text";
            }
            else if (blockData.GetType() == typeof(TafEmImageData))
            {
                blockType = "Image";
            }
            else if (blockData.GetType() == typeof(TafEmVideoBlockData))
            {
                blockType = "Video";
            }
            else if (blockData.GetType() == typeof(TafEmStepByStepData))
            {
                blockType = "Step-by-step";
            }
            else if (blockData.GetType() == typeof(TafEmTableBlockData))
            {
                blockType = "Table";
            }
            else if (blockData.GetType() == typeof(TafEmButtonsBlockData))
            {
                blockType = "Button";
            }

            return blockType;
        }

        public static string GetIconTypeInIntConnectorDropdown(ArticleContentElementType intConnectorConnectionPointType)
        {
            string iconType;

            if (intConnectorConnectionPointType == ArticleContentElementType.PredefinedProcess)
            {
                iconType = "predprocess";
            }
            else if (intConnectorConnectionPointType == ArticleContentElementType.ExternalConnector)
            {
                iconType = "extconnector";
            }
            else
            {
                iconType = $"{intConnectorConnectionPointType}".ToLower();
            }

            return iconType;
        }

        public static ArticleType GetArticleType(string rawType)
        {
            ArticleType articleType;

            if (rawType.Contains("ss-htmltable"))
            {
                articleType = ArticleType.CustomArticle;
            }
            else if (rawType.Contains("ss-flowchart"))
            {
                articleType = ArticleType.DiagnosticFlow;
            }
            else if (rawType.Contains("ss-navigate"))
            {
                articleType = ArticleType.InteractiveNavigationMap;
            }
            else
            {
                articleType = ArticleType.None;
            }
            
            return articleType;
        }

        public static ArticleStatus GetArticleStatus(string rawStatus)
        {
            bool isConversionSuccessful = Enum.TryParse(rawStatus, out ArticleStatus resultStatus);

            ArticleStatus articleStatus = isConversionSuccessful ? resultStatus : ArticleStatus.None;

            return articleStatus;
        }

        public static string GetArticleTitlePrefixByTestType(TestType testType)
        {
            string articlePrefix = string.Empty;

            if (testType == TestType.CreateCustomArticle)
            {
                articlePrefix = "article_create_test_";
            }
            else if (testType == TestType.CreateDiagnosticFlow)
            {
                articlePrefix = "flow_create_test_";
            }
            else if (testType == TestType.ArchiveRestoreArticle)
            {
                articlePrefix = "archive_restore_article_test_";
            }
            else if (testType == TestType.DuplicateActiveArticle)
            {
                articlePrefix = "duplicate_active_article_test_";
            }
            else if (testType == TestType.DuplicateArchivedArticle)
            {
                articlePrefix = "duplicate_archived_article_test_";
            }
            else if (testType == TestType.DeleteArticle)
            {
                articlePrefix = "delete_article_test_";
            }
            else if (testType == TestType.CreateArticleWithExistingTitle)
            {
                articlePrefix = "article_with_existing_title_create_test_";
            }
            else if (testType == TestType.ArticleThatCannotBePublished)
            {
                articlePrefix = "article_cannot_be_published_test_";
            }
            else if (testType == TestType.PublishToTaf)
            {
                articlePrefix = "publish_to_sp_agents_test_";
            }
            // Advisor journeys
            else if (testType == TestType.ArchiveRestoreJourney)
            {
                articlePrefix = "archive_restore_test_";
            }
            else if (testType == TestType.DuplicateActiveJourney)
            {
                articlePrefix = "duplicate_active_journey_test_";
            }
            else if (testType == TestType.DuplicateArchivedJourney)
            {
                articlePrefix = "duplicate_archived_journey_test_";
            }
            else if (testType == TestType.DeleteJourney)
            {
                articlePrefix = "delete_journey_test_";
            }

            return articlePrefix;
        }

        public static SortOrder GetSortOrder(string rawSortOrder)
        {
            SortOrder sortOrder = SortOrder.None;

            if (rawSortOrder.Contains("ascending"))
            {
                sortOrder = SortOrder.Ascending;
            }
            else if (rawSortOrder.Contains("descending"))
            {
                sortOrder = SortOrder.Descending;
            }
            else if (rawSortOrder.Contains("none"))
            {
                sortOrder = SortOrder.None;
            }

            return sortOrder;
        }

        public static DeviceType GetDeviceType(string rawDeviceType)
        {
            DeviceType deviceType = DeviceType.None;

            if (rawDeviceType.Contains("Datacard"))
            {
                deviceType = DeviceType.Datacard;
            }
            else if (rawDeviceType.Contains("Phone"))
            {
                deviceType = DeviceType.Phone;
            }
            else if (rawDeviceType.Contains("Router"))
            {
                deviceType = DeviceType.Router;
            }
            else if (rawDeviceType.Contains("Smartwatch") || rawDeviceType.Contains("Apple Watch"))
            {
                deviceType = DeviceType.Smartwatch;
            }
            else if (rawDeviceType.Contains("Tablet") || rawDeviceType.Contains("iPad"))
            {
                deviceType = DeviceType.Tablet;
            }

            return deviceType;
        }

        public static JourneyStepType GetJourneyStepType(string rawStepType)
        {
            JourneyStepType stepType = JourneyStepType.None;

            if (rawStepType.Contains("infinity"))
            {
                stepType = JourneyStepType.Infinity;
            }
            else if (rawStepType.Contains("interval"))
            {
                stepType = JourneyStepType.Interval;
            }
            else if (rawStepType.Contains("specifications"))
            {
                stepType = JourneyStepType.Specifications;
            }
            else if (rawStepType.Contains("brands"))
            {
                stepType = JourneyStepType.Brands;
            }

            return stepType;
        }

        public static ContentItemType GetContentItemType(string itemLink)
        {
            ContentItemType itemType = ContentItemType.None;

            if (itemLink.Contains("/articles"))
            {
                itemType = ContentItemType.Article;
            }
            else if (itemLink.Contains("/devices"))
            {
                itemType = ContentItemType.Device;
            }
            else if (itemLink.Contains("/journeys"))
            {
                itemType = ContentItemType.Journey;
            }

            return itemType;
        }

        public static string GetGuideViewSwitcherSpanClass(GuideViewType viewType)
        {
            string spanClass = "ss-list";

            if (viewType == GuideViewType.Slider)
            {
                spanClass = "ss-picture";
            }

            return spanClass;
        }

        public static bool IsAppleDevice(Dictionary<string, string> selectOptions) => selectOptions.Values.Any(v => v == "iPhone" || v == "iPad");

        public static bool IsElementVisible(TafEmArticleElement element)
        {
            return element.ElementType != ArticleContentElementType.Root
                && element.ElementType != ArticleContentElementType.Accordion;
        }

        public static List<ArticleContentElementType> GetContentElementTypes(List<object> types) =>
            types.Select(t => GetContentElementType(t)).ToList();

        public static string ConvertImageUri(string imageUri, App app) =>
            app == App.Embed || app == App.SelfService
                ? imageUri.Replace("sp-agents.com", "sp-selfservice.com")
                : imageUri;

        public static string GetFirstIntegerInString(string str) =>
            Regex.Match(str, @"^\d+").Value;

        public static string GetLastIntegerInString(string str) =>
            Regex.Match(str, @"\d+$").Value;

        public static string GetIntegerInString(string str) =>
            Regex.Match(str, @"\d+").Value;

        public static string GetTestDataFolderPath() =>
           Path.GetFullPath(Path.Combine(GetTestSolutionPath(), SecretsHelper.ReadSecretValue(testConfig.ConfigRoot, "TestDataFolder")));

        public static string GetTestUsersFilePath() => Path.Combine(GetTestDataFolderPath(), CommonConstants.TestUsersFile);

        public static string GetTempFolderPath() =>
           Path.GetFullPath(Path.Combine(GetTestSolutionPath(), SecretsHelper.ReadSecretValue(testConfig.ConfigRoot, "TempFolder")));

        public static string GetPathToCreatedItems() => Path.Combine(GetTempFolderPath(), "createTest.csv");

        public static string GetPathToCreatedItems(string fileName) => Path.Combine(GetTempFolderPath(), fileName);

        public static string CreatedItemsFileName(TestType testType)
        {
            string fileName;

            if (testType == TestType.CreateCustomArticle)
            {
                fileName = PathConstants.CreatedArticlesFileName;
            }
            else if (testType == TestType.CreateDiagnosticFlow)
            {
                fileName = PathConstants.CreatedFlowsFileName;
            }
            else
            {
                fileName = PathConstants.CreatedArticlesForOperationTestsFileName;
            }

            return fileName;
        }

        public static string GetTestSolutionPath()
        {
            string result = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            for (int i = 0; i < 4; i++) // 4 levels up
            {
                result = Directory.GetParent(result).FullName;
            }

            return result;
        }
    }
}
