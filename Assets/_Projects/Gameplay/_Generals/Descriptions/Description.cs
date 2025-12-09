using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Asce.Game
{
    [System.Serializable]
    public static class Description
    {
        public static readonly string pattern = @"\{([^{}]+)\}";

        /// <summary> Extract all placeholder keys inside curly brackets: {Key}. </summary>
        public static List<string> ExtractKeys(this string description)
        {
            List<string> keys = new();

            // Regex to match {AnythingInside}
            MatchCollection matches = Regex.Matches(description, pattern);

            foreach (Match match in matches)
            {
                string key = match.Groups[1].Value; // Group 1 = content inside {}
                if (!keys.Contains(key)) keys.Add(key);
            }

            return keys;
        }

        public static string GetDescription(this string description, Dictionary<string, string> replaceContents)
        {
            if (string.IsNullOrEmpty(description)) return string.Empty;
            if (replaceContents == null) return description;

            List<string> keys = description.ExtractKeys();
            foreach (string key in keys)
            {
                if (replaceContents.TryGetValue(key, out string value))
                {
                    description = description.Replace($"{{{key}}}", value);
                }
            }

            return description;
        }
    }
}
