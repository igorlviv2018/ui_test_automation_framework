using Microsoft.Extensions.Configuration;
using Taf.UI.Core.Configuration;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Taf.UI.Core.Helpers
{
    public class SecretsHelper
    {
        private readonly TestConfiguration testConfig;

        private readonly string currentEnv;

        public SecretsHelper()
        {
            testConfig = new TestConfiguration();

            currentEnv = GetTestEnv();
        }

        public static string ReadSecretValue(IConfigurationRoot config, string pathToKey) => config[pathToKey];

        public static string GetUserPasswordByEmail(IConfigurationRoot config, string email) =>
            ReadSecretValue(config, $"EmailAccounts:{email}");

        public static string GetUserPasswordByEmail(string email, TestEnvironment testEnvironment, List<User> availableUsers)
        {
            User user = availableUsers.FirstOrDefault(u => u.Email == email && u.TestEnvironment == testEnvironment);

            string password = user == null ? string.Empty : user.Password;

            return password;
        }

        public string GetTestEnv() => testConfig.ConfigRoot["TestEnv"];

        public static string GetEnvironmentVariable(string name) => Environment.GetEnvironmentVariable(name);

        public string GetTestEnvUrl(App app, bool isRedesign=false)
        {
            string pathToKey = string.Empty;

            if (app == App.Taf || app == App.TafAuth || app == App.TafAd || app == App.SystemConfiguration)
            {
                pathToKey = isRedesign? $"TafUrlsRedesign:{currentEnv}" : $"TafUrls:{currentEnv}";
            }
            else if (app == App.Embed || app == App.SelfService)
            { 
                pathToKey = $"TafEmUrls:{currentEnv}";
            }

            return testConfig.ConfigRoot[pathToKey];
        }

        // to replace by GetTestEnvUrl(App app) in CI/CD environment
        public string GetTestEnvUrl(App app, string env)
        {
            string pathToKey = string.Empty;

            if (app == App.Taf || app == App.TafAuth)
            {
                pathToKey = $"TafUrls:{env}";
            }
            else if (app == App.Embed || app == App.SelfService)
            {
                pathToKey = $"TafEmUrls:{env}";
            }

            return testConfig.ConfigRoot[pathToKey];
        }
    }
}