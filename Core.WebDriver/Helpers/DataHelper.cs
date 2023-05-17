using ObjectsComparer;
using Taf.UI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Taf.UI.Core.Helpers
{
    public class DataHelper
    {
        // to complete
        public static List<string> GetPluralDeviceTypes(List<string> singulars)
        {
            List<string> plurals = new List<string>();

            foreach (var singular in singulars)
            {
                if (singular.EndsWith("x") || singular.EndsWith("ch"))
                {
                    plurals.Add(singular + "es");
                }
                else if (singular.EndsWith("y"))
                {
                    string singularButLastChar = singular.Remove(singular.Length - 1);
                    plurals.Add(singularButLastChar + "ies");
                }
                else
                {
                    plurals.Add(singular + "s");
                }
            }

            return plurals;
        }

        public static string GetPluralDeviceType(string singular)
        {
            string plural;

            
            if (singular.EndsWith("x") || singular.EndsWith("ch"))
            {
                plural = singular + "es";
            }
            else if (singular.EndsWith("y"))
            {
                string singularButLastChar = singular.Remove(singular.Length - 1);
                plural = singularButLastChar + "ies";
            }
            else
            {
                plural = singular + "s";
            }

            return plural;
        }

        public static T GetRandomElement<T>(List<T> list)
        {
            T randomElement = default;

            if (list.Count > 0)
            {
                int randomIndex = new Random().Next(0, list.Count);

                randomElement = list[randomIndex];
            }

            return randomElement;
        }

        public static List<T> GetRandomElements<T>(List<T> list, int numberOfElements)
        {
            List<T> randomElements = new List<T>();

            Random random = new Random();

            if (list.Count > 0)
            {
                for (int i = 0; i < numberOfElements; i++)
                {
                    int randomIndex = random.Next(0, list.Count);

                    randomElements.Add(list[randomIndex]);
                }
            }

            return randomElements;
        }

        public static T GetRandomEnum<T>()
        {
            //T randomElement = default;

            int enumCount = Enum.GetValues(typeof(T)).Length;

            int randomIndex = GetRandomInteger(enumCount) - 1;

            T randomElement = (T)Enum.ToObject(typeof(T), randomIndex);

            return randomElement;
        }

        public static List<string> GetRandomSubList(List<string> inputList)
        {
            Random random = new Random();

            int randomSubListLength = GetRandomInteger(inputList.Count);

            return inputList.OrderBy(x => random.Next()).Take(randomSubListLength).ToList();
        }

        /// <summary>
        /// Get random int in range [1..upperLimit]
        /// </summary>
        /// <param name="upperLimit"></param>
        /// <returns></returns>
        public static int GetRandomInteger(int upperLimit) => new Random().Next(1, upperLimit);

        public static T GetRandomElement<T>(List<T> list, int numOfFirstElements)
        {
            T randomElement = default;

            int count = numOfFirstElements < list.Count
                ? numOfFirstElements
                : list.Count;

            if (count > 0)
            {
                Random random = new Random();

                int randomIndex = random.Next(0, list.Count);

                randomElement = list[randomIndex];
            }

            return randomElement;
        }

        public static string CompareObjects<T>(T actual, T expected)
        {
            bool isEqual = new Comparer().Compare(actual, expected, out IEnumerable<Difference> differences);

            List<string> errors = new List<string>();

            if (!isEqual)
            {
                foreach (var diff in differences)
                {
                    string err = diff.DifferenceType == DifferenceTypes.ValueMismatch
                        ? $"position (0-based) - '{diff.MemberPath}': actual - '{diff.Value1}', expected - '{diff.Value2}'"
                        : $"'{diff.DifferenceType}' (path='{diff.MemberPath}'): actual - '{diff.Value1}', expected - '{diff.Value2}'";

                    ErrorHelper.AddToErrorList(errors, err);
                }
            }

            return string.Join("; ", errors);
        }

        public static string CompareListsIgnoreOrder<T>(List<T> actual, List<T> expected) =>
            CompareObjects(actual.OrderBy(e => e), expected.OrderBy(e => e));

        public static string CompareStringLists(List<string> actual, List<string> expected) =>
            !actual.SequenceEqual(expected)
            ? $"actual: {string.Join(", ", actual)}, expected: {string.Join(", ", expected)}"
            : string.Empty;

        public static string DateTimeString() => DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss").Replace("-", "");

        public T GetElementData<T>(TafEmArticleElement element) where T : class, new() =>
            element.Data.GetType() == typeof(T) ? (T)element.Data : new T();

        public TafEmArticleElement GetElementById(int id, List<TafEmArticleElement> articleElements)
        {
            if (articleElements.Any(x => x.Id == id))
            {
                return articleElements.Where(x => x.Id == id).First();
            }

            return null;
        }

        public TafEmArticleElement GetElementByButtonPosition(DxFlowProcessButtonPosition buttonPosition, List<TafEmArticleElement> articleElements) =>
            GetElementById(buttonPosition.ProcessId, articleElements);

        public string GetElementDescriptionByButtonPosition(DxFlowProcessButtonPosition buttonPosition, List<TafEmArticleElement> articleElements)
        {
            TafEmArticleElement element = GetElementByButtonPosition(buttonPosition, articleElements);

            return element != null
                ? $"'{element.Title}' {element.ElementType} (id={buttonPosition.ProcessId}) at position {buttonPosition.ProcessPosition}"
                : string.Empty;
        }

        public static string GetUserInitials(string firstName, string lastName) => (firstName[0].ToString() + lastName[0].ToString()).ToUpper();

        public static string GetArticleLinkPart(string fullLink) => new Regex(@"\/articles.*").Match(fullLink).Value;

        public static string GetArticleId(string link) => link.Split("/").Last();
    }
}
