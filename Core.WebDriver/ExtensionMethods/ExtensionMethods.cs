using System.Collections.Generic;
using System;

namespace Taf.UI.Core.ExtensionMethods
{
    public static class ExtensionMethods
    {
        public static string CapitalizeFirstLetter(this string s) =>
            string.IsNullOrEmpty(s) ? string.Empty : char.ToUpper(s[0]) + s.Substring(1);

        public static void Shuffle<T>(this List<T> list)
        {
            Random random = new Random();
            int n = list.Count;

            for (int i = list.Count - 1; i > 1; i--)
            {
                int rnd = random.Next(i + 1);

                T value = list[rnd];
                list[rnd] = list[i];
                list[i] = value;
            }
        }
    }
}
