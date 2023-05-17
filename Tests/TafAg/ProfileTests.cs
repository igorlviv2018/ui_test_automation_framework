using NLog;
using Taf.UI.Steps;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Constants;
using System.Collections.Generic;
using Xunit;

namespace Tests
{
    public class ProfileTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture fixture;

        private readonly ILogger log;

        private readonly LoginSteps loginSteps;

        private readonly ProfileSettingsSteps profileSettingsSteps;

        private readonly ProfileMenuSteps profileMenuSteps;

        private readonly ToastAlertSteps toastAlertSteps;

        private readonly string clientName;

        public ProfileTests(TestFixture fixture)
        {
            this.fixture = fixture;

            log = LogManager.GetLogger("TafProfileUI");

            clientName = fixture.TestConfig["ClientName"];

            loginSteps = new LoginSteps(log);

            profileSettingsSteps = new ProfileSettingsSteps(log);

            profileMenuSteps = new ProfileMenuSteps(log);

            toastAlertSteps = new ToastAlertSteps();

            //clientId = fixture.DbHelper.GetClientIdByName(clientName);
        }

        [Fact(DisplayName = "Profile menu user name and email check test")]
        [Trait("Category", "TafCheckProfile")]
        public void ProfileMenuUserDataCheck()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            string userEmail = "aqa.profile.test@gmail.com";

            string password = SecretsHelper.GetUserPasswordByEmail(userEmail, fixture.TestEnvironment, fixture.TestUsers);

            profileMenuSteps.TrySignOut();

            string err = loginSteps.TryLogin(userEmail, password);

            Assert.True(string.IsNullOrEmpty(err), err);

            //Act
            err = profileMenuSteps.CheckUserNameAndEmail();

            LogHelper.LogResult(log, "User name and email checked", err);

            //Assert
            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());

            Assert.True(string.IsNullOrEmpty(err), err);
        }

        [Fact(DisplayName = "Profile update test")]
        [Trait("Category", "TafCheckProfile")]
        public void ProfileUpdateCheck()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            string userEmail = "aqa.profile.test@gmail.com";

            string altEmail = CommonConstants.ProfileUpdateNewEmail;

            string userPass = SecretsHelper.GetUserPasswordByEmail(userEmail, fixture.TestEnvironment, fixture.TestUsers);

            loginSteps.LoginWithOtherEmailRetry(userEmail, userPass, altEmail);

            profileMenuSteps.OpenProfileSettings();

            List<string> errors = new List<string>();

            //Act
            string err = profileSettingsSteps.CheckProfileBeforeUpdate();

            ErrorHelper.AddToErrorList(errors, err);

            profileSettingsSteps.UpdateProfile();

            err = profileSettingsSteps.CheckUpdateProfileAlert();

            Assert.True(string.IsNullOrEmpty(err), err);

            err = profileSettingsSteps.CheckEmailUpdated(altEmail, userPass);
            
            ErrorHelper.AddToErrorList(errors, err);

            err = profileSettingsSteps.CheckProfileUpdated();

            ErrorHelper.AddToErrorList(errors, err);

            err = profileSettingsSteps.RestoreProfile();

            ErrorHelper.AddToErrorList(errors, err);

            //Assert
            string allErrors = string.Join("; ", errors.ToArray());

            LogHelper.LogTestEnd(log, allErrors, XUnitHelper.FactDisplayName());

            Assert.True(string.IsNullOrEmpty(allErrors), allErrors);
        }

        [Fact(DisplayName = "Password update test")]
        public void PasswordUpdateCheck()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            string userEmail = "aqa.profile.test@gmail.com";

            string newPass = SecretsHelper.ReadSecretValue(fixture.TestConfig, "TestPasswords:ChangePasswordNewPassword");

            string currentPass = SecretsHelper.GetUserPasswordByEmail(userEmail, fixture.TestEnvironment, fixture.TestUsers);

            loginSteps.LoginWithOtherPasswordRetry(userEmail, currentPass, newPass);

            profileMenuSteps.OpenProfileSettings();

            List<string> errors = new List<string>();

            //Act
            profileSettingsSteps.UpdatePassword(currentPass, newPass, newPass);

            LogHelper.Log(log, "User password updated");

            string err = profileSettingsSteps.CheckUpdateProfileAlert();

            ErrorHelper.AddToErrorList(errors, err);

            err = profileSettingsSteps.CheckPasswordUpdated(userEmail, newPass);

            ErrorHelper.AddToErrorList(errors, err);

            err = profileSettingsSteps.RestorePassword(newPass, currentPass);

            ErrorHelper.AddToErrorList(errors, err);

            //Assert
            string allErrors = string.Join("; ", errors.ToArray());

            LogHelper.LogTestEnd(log, errors, XUnitHelper.FactDisplayName());

            Assert.True(errors.Count == 0, allErrors);
        }

        [Fact(DisplayName = "Redesign: Password update test")]
        public void PasswordUpdateCheckRedesign()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            string userEmail = "aqa.profile.test@gmail.com";

            string newPass = SecretsHelper.ReadSecretValue(fixture.TestConfig, "TestPasswords:ChangePasswordNewPassword");

            string currentPass = SecretsHelper.GetUserPasswordByEmail(userEmail, fixture.TestEnvironment, fixture.TestUsers);

            loginSteps.LoginWithOtherPasswordRetry(userEmail, currentPass, newPass, isRedesign:true);

            profileMenuSteps.OpenProfileSettings(isRedesign:true);

            List<string> errors = new List<string>();

            //Act
            profileSettingsSteps.UpdatePassword(currentPass, newPass, newPass, isRedesign:true);

            LogHelper.Log(log, "User password updated");

            string err = profileSettingsSteps.CheckUpdateProfileAlert(isRedesign:true);

            ErrorHelper.AddToErrorList(errors, err);

            err = profileSettingsSteps.CheckPasswordUpdated(userEmail, newPass);

            ErrorHelper.AddToErrorList(errors, err);

            err = profileSettingsSteps.RestorePassword(newPass, currentPass, isRedesign:true);

            ErrorHelper.AddToErrorList(errors, err);

            //Assert
            string allErrors = string.Join("; ", errors.ToArray());

            LogHelper.LogTestEnd(log, errors, XUnitHelper.FactDisplayName());

            Assert.True(errors.Count == 0, allErrors);
        }

        [Fact(DisplayName = "Password update test - negative")]
        [Trait("Category", "TafCheckProfile")]
        public void PasswordUpdateCheckNegative()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            string userEmail = "aqa.profile.test@gmail.com";

            string newPass = SecretsHelper.ReadSecretValue(fixture.TestConfig, "TestPasswords:ChangePasswordNewPassword");

            string currentPass = SecretsHelper.GetUserPasswordByEmail(userEmail, fixture.TestEnvironment, fixture.TestUsers);

            string invalidCurrentPass = $"{currentPass}1";

            string mismatchingNewPass = $"{newPass}1";

            loginSteps.LoginWithOtherPasswordRetry(userEmail, currentPass, newPass);

            profileMenuSteps.OpenProfileSettings();

            List<string> errors = new List<string>();

            //Act

            // Confirm password is not equal to new password
            profileSettingsSteps.UpdatePassword(currentPass, newPass, mismatchingNewPass);
            LogHelper.Log(log, "Confirm password not equal to new password validated");

            string err = profileSettingsSteps.CheckErrorMessage(MessageConstants.ConfirmPassNotEqualToNewPass);
            ErrorHelper.AddToErrorList(errors, err);

            // Too short new password
            profileSettingsSteps.UpdatePassword(currentPass, CommonConstants.TooShortPassword, CommonConstants.TooShortPassword);
            LogHelper.Log(log, "Too short new password validated");

            err = profileSettingsSteps.CheckErrorMessage(MessageConstants.PasswordShouldBeSpecificLong);
            ErrorHelper.AddToErrorList(errors, err);

            // New password with letters only
            profileSettingsSteps.UpdatePassword(currentPass, CommonConstants.LettersOnlyPassword, CommonConstants.LettersOnlyPassword);
            LogHelper.Log(log, "Letters only password validated");

            err = profileSettingsSteps.CheckErrorMessage(MessageConstants.PasswordShouldBeSpecificLong);
            ErrorHelper.AddToErrorList(errors, err);

            // Invalid current password
            profileSettingsSteps.UpdatePassword(invalidCurrentPass, newPass, newPass);
            LogHelper.Log(log, "Incorrect current password validated");

            err = profileSettingsSteps.CheckErrorMessage(MessageConstants.IncorectCurrentPassword);
            ErrorHelper.AddToErrorList(errors, err);

            toastAlertSteps.WaitAlertToDisappear();

            //Assert
            string allErrors = string.Join("; ", errors.ToArray());

            LogHelper.LogTestEnd(log, allErrors, XUnitHelper.FactDisplayName());

            Assert.True(string.IsNullOrEmpty(allErrors), allErrors);
        }

        [Fact(DisplayName = "Redesign: Password update test - negative")]
        [Trait("Category", "TafCheckProfile")]
        public void PasswordUpdateCheckNegativeRedesign()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            string userEmail = "aqa.profile.test@gmail.com";

            string newPass = SecretsHelper.ReadSecretValue(fixture.TestConfig, "TestPasswords:ChangePasswordNewPassword");

            string currentPass = SecretsHelper.GetUserPasswordByEmail(userEmail, fixture.TestEnvironment, fixture.TestUsers);

            string invalidCurrentPass = $"{currentPass}1";

            string mismatchingNewPass = $"{newPass}1";

            loginSteps.LoginWithOtherPasswordRetry(userEmail, currentPass, newPass, isRedesign:true);

            profileMenuSteps.OpenProfileSettings(isRedesign:true);

            List<string> errors = new List<string>();

            //Act

            // Confirm password is not equal to new password
            profileSettingsSteps.UpdatePassword(currentPass, newPass, mismatchingNewPass, isRedesign:true);
            LogHelper.Log(log, "Confirm password not equal to new password validated");

            string err = profileSettingsSteps.CheckErrorMessage(MessageConstants.ConfirmPassNotEqualToNewPass, isRedesign:true);

            ErrorHelper.AddToErrorList(errors, err);

            // Too short new password
            profileSettingsSteps.UpdatePassword(currentPass, CommonConstants.TooShortPassword, CommonConstants.TooShortPassword, isRedesign: true);
            LogHelper.Log(log, "Too short new password validated");

            err = profileSettingsSteps.CheckErrorMessage(MessageConstants.PasswordShouldBeSpecificLong, isRedesign: true);
            ErrorHelper.AddToErrorList(errors, err);

            // Invalid current password
            profileSettingsSteps.UpdatePassword(invalidCurrentPass, newPass, newPass, isRedesign: true);
            LogHelper.Log(log, "Incorrect current password validated");

            toastAlertSteps.WaitAlertToAppear(isRedesign: true);

            toastAlertSteps.WaitAlertToDisappear(isRedesign: true);

            err = profileSettingsSteps.CheckErrorMessage(MessageConstants.IncorectCurrentPassword, isRedesign: true);
            ErrorHelper.AddToErrorList(errors, err);

            //Assert
            string allErrors = string.Join("; ", errors.ToArray());

            LogHelper.LogTestEnd(log, allErrors, XUnitHelper.FactDisplayName());

            Assert.True(string.IsNullOrEmpty(allErrors), allErrors);
        }

        [Fact(DisplayName = "Redesign: Profile menu user name and email check test")]
        [Trait("Category", "TafCheckProfile")]
        public void ProfileMenuUserDataCheckRedesign()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            string userEmail = "aqa.profile.test@gmail.com";

            string password = SecretsHelper.GetUserPasswordByEmail(userEmail, fixture.TestEnvironment, fixture.TestUsers);

            profileMenuSteps.TrySignOut(isRedesign: true);

            string err = loginSteps.TryLogin(userEmail, password, isRedesign:true);

            Assert.True(string.IsNullOrEmpty(err), err);

            //Act
            err = profileMenuSteps.CheckUserNameAndEmail(isRedesign:true);

            LogHelper.LogResult(log, "User name and email checked", err);

            //Assert
            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());

            Assert.True(string.IsNullOrEmpty(err), err);
        }

        [Fact(DisplayName = "Redesign: Profile update test")]
        [Trait("Category", "TafCheckProfile")]
        public void ProfileUpdateCheckRedesign()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            string userEmail = "aqa.profile.test@gmail.com";

            string altEmail = CommonConstants.ProfileUpdateNewEmail;

            string userPass = SecretsHelper.GetUserPasswordByEmail(userEmail, fixture.TestEnvironment, fixture.TestUsers);

            loginSteps.LoginWithOtherEmailRetry(userEmail, userPass, altEmail, isRedesign:true);

            profileMenuSteps.OpenProfileSettings(isRedesign:true);

            List<string> errors = new List<string>();

            //Act
            string err = profileSettingsSteps.CheckProfileBeforeUpdate(isRedesign:true);

            ErrorHelper.AddToErrorList(errors, err);

            err = profileSettingsSteps.UpdateProfile(isRedesign:true);

            ErrorHelper.AddToErrorList(errors, err);

            err = profileSettingsSteps.CheckEmailUpdated(altEmail, userPass);

            ErrorHelper.AddToErrorList(errors, err);

            err = profileSettingsSteps.CheckProfileUpdated(isRedesign:true);

            ErrorHelper.AddToErrorList(errors, err);

            err = profileSettingsSteps.RestoreProfile(isRedesign:true);

            ErrorHelper.AddToErrorList(errors, err);

            profileMenuSteps.TrySignOut(isRedesign:true);//deb

            //Assert
            string allErrors = string.Join("; ", errors.ToArray());

            LogHelper.LogTestEnd(log, allErrors, XUnitHelper.FactDisplayName());

            Assert.True(string.IsNullOrEmpty(allErrors), allErrors);
        }
    }
}
