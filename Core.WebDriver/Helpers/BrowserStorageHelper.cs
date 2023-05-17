using Taf.Api.Tests.Helpers;
using Taf.UI.Core.Enums;
using Taf.WebDriver.Wrapper;
using System.Collections.Generic;

namespace Taf.UI.Core.Helpers
{
    public class BrowserStorageHelper
    {
        public Dictionary<string, string> LocalStorage { get; set; } = new Dictionary<string, string>();

        public void SetItem(string key, string value, BrowserStorage storage)
        {
            string javaScript = storage == BrowserStorage.Local
                ? $"localStorage.setItem('{key}','{value}');"
                : $"sessionStorage.setItem('{key}','{value}');";

            if (!string.IsNullOrEmpty(value))
            {
                Browser.Current.ExecuteJavaScript(javaScript);
            }
        }

        public string GetItem(string key, BrowserStorage storage)
        {
            string javaScript = storage == BrowserStorage.Local
                ? $"return localStorage.getItem('{key}');"
                : $"return sessionStorage.getItem('{key}');";

            return string.IsNullOrEmpty(key)
                ? string.Empty
                : (string)Browser.Current.ExecuteJavaScript(javaScript);
        }

        public void SaveItem(string key, BrowserStorage storage)
        {
            string value = "";

            if (!string.IsNullOrEmpty(key))
            {
                value = GetItem(key, storage);
            }

            if (!string.IsNullOrEmpty(value))
            {
                LocalStorage[key] = value;
            }
        }

        public void RestoreItem(string key, BrowserStorage storage)
        {
            if (LocalStorage.ContainsKey(key))
            {
                SetItem(key, LocalStorage[key], storage);
            }
        }

        public void ClearLocalStorage()
        {
            Browser.Current.ExecuteJavaScript($"localStorage.clear();");
        }

        public void SetSessionItem(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Browser.Current.ExecuteJavaScript($"sessionStorage.setItem('{key}','{value}');");
            }
        }

        public string GetAuthToken(BrowserStorage storage = BrowserStorage.Session, bool isRedesign=false)
        {
            string vuex = GetItem(isRedesign ? "sp::auth" : "vuex", storage);

            if (isRedesign)
            {
                vuex = vuex.Replace("\\", "");

                vuex = vuex[1..^1]; //remove first and last \" (double quotes)
            }

            JsonValue authToken = new JsonHelper().GetJsonValue(vuex, isRedesign ? "accessToken" : "user.auth.token");

            return string.IsNullOrEmpty(authToken.Err)
                ? (string)authToken.Value
                : string.Empty;
        }
    }
}
