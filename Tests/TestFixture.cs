using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Extensions.Logging;
using Taf.UI.Core.Configuration;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models;
using Taf.WebDriver.Wrapper;
using System;
using System.Collections.Generic;

namespace Tests
{
    public class TestFixture : IDisposable
    {
        public IConfigurationRoot TestConfig { get; private set; }

        public TestEnvironment TestEnvironment { get; private set; }

        public List<User> TestUsers { get; private set; } = new List<User>();

        private Browser browser;

        public TestFixture()
        {
            TestConfig = new TestConfiguration().ConfigRoot;

            bool isSuccessful = Enum.TryParse(SecretsHelper.GetEnvironmentVariable("TEST_ENV"), out TestEnvironment testEnvironment);

            if (isSuccessful)
            {
                TestEnvironment = testEnvironment;
            }

            TestUsers = UserHelper.GetTestUsers();

            //DbHelper = new DbHelper(Config.Config["DBConnectStr"]);

            //DbHelper.OpenConnection();

            LogManager.Configuration = new NLogLoggingConfiguration(TestConfig.GetSection("NLog"));

            InitBrowser();
        }

        public void Dispose()
        {
            //DbHelper.CloseConnection();
            browser.Quit();
        }

        public void InitBrowser()
        {
            bool isSucc = Enum.TryParse("Chrome", out BrowserType type); //Chrome Firefox Remote

            browser = new Browser(type);

            browser.Init();

            browser.Maximize();
        }

        public void ReopenBrowser()
        {
            Dispose();

            InitBrowser();
        }
    }
}
