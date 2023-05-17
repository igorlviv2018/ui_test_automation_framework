using Microsoft.Extensions.Configuration;
using NLog;
using Taf.UI.Core.Configuration;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Exceptions;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models;
using Taf.UI.PageObjects;
using System.Collections.Generic;

namespace Taf.UI.Steps
{
    public class TafConfigSteps : BaseSteps
    {
        public TafConfigSteps(App app, ILogger logger) : base(app, logger)
        {
            TestConfig = new TestConfiguration().ConfigRoot;
        }

        private readonly IConfigurationRoot TestConfig;

        private readonly LoginPage loginPage = new LoginPage();

        private readonly DashboardPage dashboardPage = new DashboardPage();

        private readonly BrowserStorageHelper localStorage = new BrowserStorageHelper();

        private bool rememberMe;

        private readonly Spinner spinner = new Spinner(App.Taf);

        public string ExpandApplicationBlock(string email, bool rememberMe = false)
        {
            string userPass = SecretsHelper.GetUserPasswordByEmail(TestConfig, email) ?? string.Empty;

            //string err = TryLogin(email, userPass, roles:null, rememberMe);

            return "";
        }
    }
}
