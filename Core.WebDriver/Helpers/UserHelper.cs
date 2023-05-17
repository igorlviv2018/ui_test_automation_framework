using Taf.UI.Core.Enums;
using Taf.UI.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Taf.UI.Core.Helpers
{
    public class UserHelper
    {
        public static List<User> ReadUsersFile()
        {
            string filePath = CommonHelper.GetTestUsersFilePath();

            List<User> users = new List<User>();

            if (File.Exists(filePath))
            {
                List<string> lines = CsvHelper.ReadCsvLines(filePath);

                foreach (var line in lines)
                {
                    var values = line.Split(';');

                    if (values.Length != 4)
                    {
                        continue;
                    }

                    User user = new User
                    {
                        Email = values[0],
                        Roles = GetRolesFromString(values[1]),
                        TestEnvironment = GetTestEnvironmentFromString(values[2]),
                        ClientId = values[3]
                    };

                    users.Add(user);
                }
            }

            return users;
        }

        public static List<UserRole> GetRolesFromString(string roles)
        {
            var values = roles.Split(',');

            List<UserRole> result = new List<UserRole>();

            foreach (var role in values)
            {
                bool isParseSuccessful = Enum.TryParse(role, out UserRole parsedRole);

                if (isParseSuccessful)
                {
                    result.Add(parsedRole);
                }
            }

            return result;
        }

        public static TestEnvironment GetTestEnvironmentFromString(string testEnvironment)
        {  
            bool isParseSuccessful = Enum.TryParse(testEnvironment, out TestEnvironment result);

            return isParseSuccessful ? result : TestEnvironment.Dev;
        }

        public static List<User> GetTestUsers()
        {
            List<User> users = ReadUsersFile();

            List<UserCredentials> userCredentials = GetUserCredentials();

            foreach (var user in users)
            {
                UserCredentials credentials = userCredentials.Where(c => user.Email == c.Email).FirstOrDefault();

                user.Password = credentials?.Password ?? string.Empty;
            }

            return users;
        }

        public static User GetUserByRoles(List<User> availableUsers, List<UserRole> roles, TestEnvironment testEnvironment, string client, int position = 1)
        {
            List<User> resultUsers = availableUsers
                .Where(u => u.TestEnvironment == testEnvironment && DataHelper.CompareListsIgnoreOrder(u.Roles, roles) == string.Empty
                           && GetUserPositionFromEmail(u.Email) == position
                           && u.ClientId == client)
                .ToList();

            return resultUsers.Count > 0 ? resultUsers[0] : null;
        }

        public static User GetBasicUser(List<User> availableUsers, TestEnvironment testEnvironment, int position = 1, string client = "0") =>
            GetUserByRoles(availableUsers, new List<UserRole> { UserRole.BasicUser }, testEnvironment, client, position);

        public static User GetContentAuthorUser(List<User> availableUsers, TestEnvironment testEnvironment, int position = 1, string client = "0") =>
            GetUserByRoles(availableUsers, new List<UserRole> { UserRole.ContentAuthor }, testEnvironment, client, position);

        public static User GetAdminUser(List<User> availableUsers, TestEnvironment testEnvironment, int position = 1, string client = "0") =>
            GetUserByRoles(availableUsers, new List<UserRole> { UserRole.Administrator }, testEnvironment, client, position);

        public static User GetAdvisorAuthorUser(List<User> availableUsers, TestEnvironment testEnvironment, int position = 1, string client = "0") =>
            GetUserByRoles(availableUsers, new List<UserRole> { UserRole.AdvisorAuthor }, testEnvironment, client, position);

        public static int GetUserPositionFromEmail(string email)
        {
            Match match = new Regex(@"(\d+)(?=@)").Match(email);

            return match.Success ? int.Parse(match.Value) : 1;
        }

        public static List<UserCredentials> GetUserCredentials()
        {
            string emailPasswords = SecretsHelper.GetEnvironmentVariable("TEST_USERS");

            var splited = emailPasswords.Split(";");

            List<UserCredentials> userCredentialsList = new List<UserCredentials>();

            foreach (var emailPassword in splited)
            {
                var splittedEmailPass = emailPassword.Split(":");

                if (splittedEmailPass.Length == 2)
                {
                    UserCredentials userCredentials = new UserCredentials()
                    {
                        Email = splittedEmailPass[0].Trim(),
                        Password = splittedEmailPass[1].Trim()
                    };

                    userCredentialsList.Add(userCredentials);
                }
            }

            return userCredentialsList;
        }
    }
}
