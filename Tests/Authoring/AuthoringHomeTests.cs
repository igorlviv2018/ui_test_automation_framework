using Xunit;
using NLog;
using Taf.UI.Steps;
using Taf.UI.Core.Enums;
using Taf.UI.Steps.Authoring;
using Taf.UI.PageObjects.Authoring;
using Taf.UI.Core.Element;
using Taf.UI.Core.Models.TafAuth;
using System.Collections.Generic;
using Taf.UI.Core.Helpers;
using Taf.UI.Steps.TafSteps;

namespace Tests
{
    public class TafAuthHomeTests : TestBaseTaf, IClassFixture<TestFixture>
    {
        private readonly TestFixture fixture;

        private readonly LoginSteps loginSteps;

        private readonly AppsMenuSteps appsMenuSteps;

        private readonly CommonAuthoringSteps authoringSteps;

        private readonly SidebarSteps sideMenuSteps;

        private readonly ArticleTableSteps articleTableSteps;

        private readonly ArticlesPageSteps agentsArticlesPageSteps;

        private readonly string clientName;

        private readonly ArticlesPage articlesPage = new ArticlesPage();

        public TafAuthHomeTests(TestFixture fixture) : base(LogManager.GetLogger("TafAuthHomePageTestsUI"))
        {
            this.fixture = fixture;

            clientName = fixture.TestConfig["ClientName"];

            loginSteps = new LoginSteps(log);

            appsMenuSteps = new AppsMenuSteps(log);

            authoringSteps = new CommonAuthoringSteps(App.TafAuth, log);

            sideMenuSteps = new SidebarSteps(log);

            articleTableSteps = new ArticleTableSteps(log);

            agentsArticlesPageSteps = new ArticlesPageSteps(log);

            //clientId = fixture.DbHelper.GetClientIdByName(clientName);
        }

        //[Fact(DisplayName ="Side bar Articles menu")]
        //public void CreateArticle()
        //{
        //    string testDesc = "Side bar Articles menu test";

        //    log.Info($"*** {testDesc} started ***");

        //    loginSteps.OpenApp();

        //    string userEmail = "aqa.movistar.pe.adm.dev@gmail.com";// "aqa.swisscomch.adm.dev@gmail.com"; //"aqa.movistar.pe.adm.dev@gmail.com";

        //    string userPass = SecretsHelper.GetUserPasswordByEmail(fixture.TestConfig, userEmail);

        //    loginSteps.Login(userEmail, userPass);

        //    Assert.True(navigationBarSteps.NavigationBarIsPresent(), "Not a home page!");
        //    log.Info($"Logged in successful");

        //    appsMenuSteps.OpenApp(AppLinkType.TafAuth);

        //    authoringSteps.CheckArticlesMenuHasSubItems();

        //    authoringSteps.OpenArticle("article_001");

        //    authoringSteps.OpenEditorTab("Location");
        //    //int spinners = authoringSteps.EditorSave();

        //    //authoringSteps.OpenScheduleSettings();

        //    ArticleProperties props = authoringSteps.GetArticleProperties();

        //    DateTime dat = DateTime.ParseExact("2/8/2021", "d/M/yyyy", CultureInfo.InvariantCulture);

        //    string pDate = authoringSteps.GetPublishDate();

        //    authoringSteps.OpenPublishDatePicker();

        //    authoringSteps.SetSpecialDate(Taf.UI.Core.Enums.DatePickerButton.Immediately);

        //    authoringSteps.OpenExpirationDatePicker();

        //    authoringSteps.SetSpecialDate(Taf.UI.Core.Enums.DatePickerButton.Evergreen);

        //    authoringSteps.SetDateInDatePicker(DateTime.Now.AddDays(-1));

        //    ArticleProperties prop = authoringSteps.GetArticleProperties();

        //    authoringSteps.OpenScheduleSettings();

        //    authoringSteps.OpenPublishDatePicker();

        //    authoringSteps.OpenExpirationDatePicker();

        //    authoringSteps.OpenPublishDatePicker();

        //    List<string> owners = authoringSteps.GetOwnerList();

        //    authoringSteps.OpenEditorTab("Content");

        //    authoringSteps.OpenEditorTab("Location");

        //    authoringSteps.OpenEditorTab("Properties");

        //    authoringSteps.OpenEditorTab("Properties");

        //    //List<AppsMenuItem> menuItems = new AppsMenu().GetAllMenuItems(); //deb

        //    LogHelper.Log(log, "SP Authoring opened");

        //    authoringSteps.PlaceElementOnContentHolder("Text");
        //    Thread.Sleep(300);

        //    authoringSteps.PlaceElementOnContentHolder("Image");
        //    Thread.Sleep(300);

        //    authoringSteps.PlaceElementOnContentHolder("Step-by-step");
        //    Thread.Sleep(300);

        //    authoringSteps.PlaceElementOnContentHolder("Video");
        //    Thread.Sleep(300);

        //    authoringSteps.PlaceElementOnContentHolder("Text");
        //    Thread.Sleep(300);

        //    authoringSteps.PlaceElementOnContentHolder("Rich text");
        //    Thread.Sleep(300);

        //    authoringSteps.PlaceElementOnContentHolder("Image");
        //    Thread.Sleep(300);

        //    authoringSteps.PlaceElementOnContentHolder("Step-by-step");

        //    authoringSteps.AddImage("D:\\Images\\bh_52.png"); //debug

        //    //authoringSteps.UpdateImage("D:\\Images\\headset_01.jpg"); //debug

        //    List<string> errors = new List<string>();

        //    //Assert
        //    //bool testPassed = errors.Count == 0;
        //    string allErrors = string.Join("; ", errors.ToArray());

        //    LogHelper.LogTestEnd(log, errors, testDesc);

        //    Assert.True(errors.Count == 0, allErrors);
        //}
    }
}
