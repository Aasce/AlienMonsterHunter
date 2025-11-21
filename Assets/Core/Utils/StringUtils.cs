using System;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Asce.Managers.Utils
{
    /// <summary>
    ///     Utility class for applying Unity rich text formatting to strings.
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        ///     Precompiled regex used to sanitize strings by removing 
        ///     non-alphanumeric characters except spaces.
        /// </summary>
        private static readonly Regex sanitizeRegex = new("[^a-zA-Z0-9 ]", RegexOptions.Compiled);

        /// <summary>
        ///     Wraps the string in a Unity rich text color tag using a hex color code.
        ///     <br/>
        ///     Returns the original string if the color code is invalid.
        /// </summary>
        /// <param name="text"> The input string to format. </param>
        /// <param name="hexColorCode"> Hexadecimal color code (e.g., "#FF0000"). </param>
        /// <returns>
        ///     Color-wrapped string or original string if code is invalid.
        /// </returns>
        public static string ColorWrap(this string text, string hexColorCode)
        {
            if (!ColorUtils.IsHexColorCode(hexColorCode)) return text;
            return $"<color={hexColorCode}>{text}</color>";
        }

        /// <summary>
        ///     Wraps the string in a Unity rich text color tag using a UnityEngine.Color.
        /// </summary>
        /// <param name="text"> The input string to format. </param>
        /// <param name="color"> UnityEngine.Color value. </param>
        /// <returns> Color-wrapped string. </returns>
        public static string ColorWrap(this string text, Color color)
        {
            return text.ColorWrap($"#{ColorUtility.ToHtmlStringRGBA(color)}");
        }

        /// <summary>
        ///     Wraps the string in Unity rich text bold tags.
        /// </summary>
        public static string BoldWrap(this string text) => $"<b>{text}</b>";

        /// <summary>
        ///     Wraps the string in Unity rich text italic tags.
        /// </summary>
        public static string ItalicWrap(this string text) => $"<i>{text}</i>";

        /// <summary>
        ///     Wraps the string in Unity rich text underline tags.
        /// </summary>
        public static string UnderlineWrap(this string text) => $"<u>{text}</u>";

        /// <summary>
        ///     Wraps the string in Unity rich text size tags.
        /// </summary>
        /// <param name="text"> The input string to format. </param>
        /// <param name="size"> Font size to apply. </param>
        public static string SizeWrap(this string text, int size) => $"<size={size}>{text}</size>";

        /// <summary>
        ///     Sanitizes the input string by removing special characters and converting it to CamelCase.
        /// </summary>
        /// <param name="input"> The input string to sanitize and convert. </param>
        /// <returns>
        ///     A CamelCase version of the sanitized input string.
        ///     <br/>
        ///     If the input is null, empty, or contains no valid characters, 
        ///     returns <see cref="string.Empty"/>.
        /// </returns>
        public static string SanitizeAndCamelCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            // Remove all characters that are not letters, digits, or spaces
            var clean = sanitizeRegex.Replace(input, "");

            // Split the string into words by space
            string[] words = clean.Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

            if (words.Length == 0) return string.Empty;

            // Capitalize the first letter of each word
            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i];
                words[i] = char.ToUpper(word[0]) + (word.Length > 1 ? word.Substring(1) : "");
            }

            // Combine the words into a single CamelCase string
            return string.Join("", words);
        }

        /// <summary>
        ///     Convert a string to standard snake_case:
        ///     - Split words by CamelCase, PascalCase, spaces, and symbols.
        ///     - Convert to lowercase.
        ///     - Collapse consecutive separators into a single underscore.
        ///     - Trim leading and trailing underscores.
        /// </summary>
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            // Normalize whitespace and make splitting easier
            string result = input;

            // Insert underscore before capital letters (CamelCase tp snake_case)
            // But not at the very start.
            result = Regex.Replace(result, "([a-z0-9])([A-Z])", "$1_$2");

            // Replace any non-alphanumeric with an underscore
            result = Regex.Replace(result, "[^a-zA-Z0-9]", "_");

            // Collapse multiple underscores
            result = Regex.Replace(result, "_+", "_");

            // Lowercase
            result = result.ToLowerInvariant();

            // Trim leading/trailing underscores
            result = result.Trim('_');

            return result;
        }

        /// <summary>
        ///     Convert an integer to a string with a custom thousands separator.
        /// </summary>
        /// <param name="value"> The integer to format. </param>
        /// <param name="sep"> Thousands separator string. Default is a single space. </param>
        /// <returns>
        ///     Formatted string with separator every 3 digits from the right.
        /// </returns>
        public static string ToThousandsSeparatedString(this int value, string sep = " ")
        {
            if (string.IsNullOrEmpty(sep)) return value.ToString();

            bool negative = value < 0;
            int abs = negative ? -value : value;

            string digits = abs.ToString(); // Convert number to raw digits
            int groups = (digits.Length - 1) / 3; // Number of separators needed

            // Total output length
            int outputLength = digits.Length + groups * sep.Length + (negative ? 1 : 0);
            return String.Create(outputLength, (digits, sep, negative), static (span, state) =>
            {
                var (digits, sep, negative) = state;

                int write = span.Length - 1;
                int count = 0;

                // Write digits right-to-left
                for (int i = digits.Length - 1; i >= 0; i--)
                {
                    span[write--] = digits[i];
                    count++;
                    if (count == 3 && i != 0) // Every 3 digits, insert separator (except at the start)
                    {
                        for (int s = sep.Length - 1; s >= 0; s--)
                            span[write--] = sep[s];

                        count = 0;
                    }
                }

                if (negative) span[0] = '-'; // Add minus sign if needed
            });
        }
    }
}
