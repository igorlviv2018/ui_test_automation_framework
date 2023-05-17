using System.Collections.Generic;
using System.Linq;

namespace Taf.UI.Core.Helpers
{
    public class ErrorHelper
    {
        public static void AddToErrorList(List<string> errList, string errMessage, string prefix="")
        {
            if (!string.IsNullOrEmpty(errMessage))
            {
                errList.Add($"{prefix}{errMessage}");
            }
        }

        public static void AddToErrorList(List<string> errList, List<string> errMessages)
        {
            if (errMessages.Count > 0)
            {
                errList.AddRange(errMessages);
            }
        }

        public static string AddPrefixToError(string errMessage, string prefix = "") =>
            string.IsNullOrEmpty(errMessage) ? errMessage : $"{prefix}{errMessage}";

        public static List<string> AddPrefixToErrorList(List<string> errList, string prefix = "")
        {
            if (errList.Count > 0 && prefix != "")
            {
                errList.Insert(0, prefix);
            }

            return errList;
        }

        public static string AddPostfixToError(string errMessage, string postfix = "") =>
            string.IsNullOrEmpty(errMessage) ? errMessage : $"{errMessage}{postfix}";

        public static string AddSemicolon(string errMessage) => AddPostfixToError(errMessage, "; ");

        public static List<string> GetUniqueErrors(List<string> errList) => errList.Distinct().ToList();

        public static string ConvertErrorsToString(List<string> errList, string errPrefix = "")
        {
            string errs = string.Join("; ", errList.Where(s => !string.IsNullOrEmpty(s)));

            return !string.IsNullOrEmpty(errs) ? $"{errPrefix}{errs}" : string.Empty;
        }

        public static string InvalidElementError(string pageName, string elementName, string actual, string expected) =>
            $"'{pageName}' page: Invalid {elementName}: '{actual}' (but expected '{expected}')";

        public static bool IsAnyCriticalError(List<string> errMessages) => errMessages.Any(m => m.Contains("[Critical]"));

        public static bool IsCriticalError(string errMessage) => errMessage.Contains("[Critical]");
    }
}
