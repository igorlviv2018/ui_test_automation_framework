//using Taf.Api.Tests.Common.Enums;
//using Taf.Api.Tests.Db;
using Taf.Api.Tests.Helpers;
using Taf.Api.Tests.JsonModels.AuthModels;
using Taf.Api.Tests.JsonModels.ProfileModels;
using Taf.Api.Tests.JsonModels.UserModels;
using Taf.Api.Tests.Models;
using Taf.Api.Tests.RestSharp;
//using Taf.Core.Models.Settings.ContentSharing;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Exceptions;
using System.Collections.Generic;
using System.Linq;
using UserHelper = Taf.Api.Tests.Helpers.UserHelper;

namespace Taf.UI.Core.Helpers
{
    public class ApiHelper
    {
        private HttpHelper httpHelper;

        private Taf.Api.Tests.Helpers.UserHelper userHelper;

        //private DbTestHelper dbTestHelper;

        //private AdminHelper adminHelper;

        private SecretsHelper secretsHelper;

        public ApiHelper()
        {
            httpHelper = new HttpHelper();

            //userHelper = new UserHelper(new DbHelper(""));
            userHelper = new Taf.Api.Tests.Helpers.UserHelper();

            //adminHelper = new AdminHelper();

            secretsHelper = new SecretsHelper();

            //dbTestHelper = new DbTestHelper();
        }

        //public List<string> GetClientApps(int clientId)
        //{
        //    return dbTestHelper.GetClientApps(clientId);
        //}

        //public string GetAuthToken(string email, string password = "")
        //{
        //    string userPass = password == "" ? secretsHelper.GetUserPassword(email) : password;

        //    LoginRequest loginRequest = new LoginRequest()
        //    {
        //        LoginType = JsonValueType.UserDefined,
        //        Login = email,
        //        PasswordType = JsonValueType.UserDefined,
        //        Password = userPass ?? string.Empty
        //    };

        //    LoginResponseData response = httpHelper.GetLoginResponseData(loginRequest);

        //    string authToken;

        //    if (response.Errors.Count != 0)
        //    {
        //        //to do - add logging
        //        throw new ApiHelperException(ErrorHelper.ConvertErrorsToString(response.Errors));
        //    }
        //    else
        //    {
        //        authToken = response.Token;
        //    }

        //    return authToken;
        //}

        public Profile GetProfile(string authToken)
        {
            UserGetProfileResponse response = userHelper.GetUserProfile(authToken);

            Profile profile;

            if (response.Errors.Count != 0)
            {
                throw new ApiHelperException(ErrorHelper.ConvertErrorsToString(response.Errors));
            }
            else
            {
                profile = response.Profile;
            }

            return profile;
        }

        public LoginResponseData Login(string email, string password)
        {
            LoginResponseData response = httpHelper.GetLoginResponseData(email, password);

            return response;
        }

        public string UpdateProfile(string newFirstName, string newLastName, string newEmail, string authToken)
        {
            UserUpdateProfileRequest request = new UserUpdateProfileRequest()
            {
                FirstName = newFirstName,
                LastName = newLastName,
                Email = newEmail
            };

            UserUpdateProfileResponse response = userHelper.UpdateUserProfile(request, authToken);

            return response.Errors.Count != 0 ? ErrorHelper.ConvertErrorsToString(response.Errors) : string.Empty;
        }

        public string RestoreProfile(string authToken)
        {
            string errMessage = UpdateProfile(CommonConstants.ProfileUpdateOriginalFirstName,
                          CommonConstants.ProfileUpdateOriginalLastName,
                          CommonConstants.ProfileUpdateOriginalEmail,
                          authToken);

            return string.IsNullOrEmpty(errMessage) ?
                errMessage :
                $"API Helper: {errMessage}";
        }

        public string RestorePassword(string currentPass, string newPass, string authToken)
        {
            UserUpdatePasswordRequest request = new UserUpdatePasswordRequest()
            {
                CurrentPassword = currentPass,
                NewPassword = newPass,
                NewPasswordConfirm = newPass
            };

            UserUpdatePasswordResponse response = userHelper.UpdateUserPassword(request, authToken);

            string errMessage = ErrorHelper.ConvertErrorsToString(response.Errors);

            return string.IsNullOrEmpty(errMessage) ?
                errMessage :
                $"API Helper: {errMessage}";
        }

        //public Dictionary<UrlTemplateType, string> GetContentSharingUrls(string authToken)
        //{
        //    ContentSharingSettingsGetResponse response = adminHelper.GetContentSharingSettings(authToken);

        //    if (response.Errors.Count != 0)
        //    {
        //        throw new ApiHelperException(ErrorHelper.ConvertErrorsToString(response.Errors));
        //    }

        //    Api.Tests.JsonModels.AdminModels.ContentSharingSettings settings = response.Settings;

        //    Dictionary<UrlTemplateType, string> urls = new Dictionary<UrlTemplateType, string>();

        //    if (!settings.Setting.Enabled)
        //    {
        //        return urls;
        //    }

        //    string baseUrl = "";

        //    bool isDynamic = settings.Setting.IsDynamic;

        //    if (settings.Setting.LanguageBaseUrl != null)
        //    {
        //        foreach (var key in settings.Setting.LanguageBaseUrl.Keys)
        //        {
        //            baseUrl = settings.Setting.LanguageBaseUrl[key];
        //        }
        //    }

        //    List<UrlTemplateType> requireDeviceTemplate = new List<UrlTemplateType>()
        //    {
        //        UrlTemplateType.Specification,
        //        UrlTemplateType.ThreeDView,
        //        UrlTemplateType.VirtualDevice
        //    };

        //    string deviceTemplate = settings.Setting.UrlTemplates
        //        .Where(t => t.TemplateType == UrlTemplateType.Device)
        //        .Select(x => x.Template)
        //        .FirstOrDefault();

        //    foreach (var template in settings.Setting.UrlTemplates)
        //    {
        //        string url = baseUrl;

        //        if (isDynamic)
        //        {
        //            if (template.PrependTemplate != null)
        //            {
        //                url = $"{url}{template.PrependTemplate.Template}";
        //            }

        //            if (requireDeviceTemplate.Contains(template.TemplateType))
        //            {
        //                url = $"{url}{deviceTemplate}";
        //            }

        //            url = $"{url}{template.Template}";
        //        }

        //        urls[template.TemplateType] = url;
        //    }

        //    return urls;
        //}

        //public string GetExpectedContentSharingUrl(UrlTemplateType templateType, string authToken)
        //{
        //    Dictionary<UrlTemplateType, string> urls = GetContentSharingUrls(authToken);

        //    return (urls.Count > 0 && urls.Keys.Contains(templateType))
        //        ? urls[templateType]
        //        : string.Empty;
        //}
    }
}
