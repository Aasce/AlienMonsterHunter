using System.Text.RegularExpressions;
using UnityEngine;

namespace Asce.Core.Utils
{
    /// <summary>
    ///     Utility class for working with Color values beyond what Unity provides.
    /// </summary>
    public static class ColorUtils
    {
        /// <summary>
        ///     Regular expression to validate hex color codes.
        ///     Supports 3-digit, 6-digit, and 8-digit hex values with '#' prefix.
        /// </summary>
        public static readonly Regex hexColorRegex = new(@"^#(?:[0-9a-fA-F]{3}|[0-9a-fA-F]{6}|[0-9a-fA-F]{8})$");

        /// <summary>
        ///     Checks whether a given string is a valid hex color code.
        /// </summary>
        /// <param name="hexCode"> The hex string to validate (e.g., "#FF0000" or "#FFF"). </param>
        /// <returns>
        ///     True if the string is a valid hex color; otherwise, false.
        /// </returns>
        public static bool IsHexColorCode(string hexCode)
        {
            return !string.IsNullOrEmpty(hexCode) && hexColorRegex.IsMatch(hexCode);
        }

        /// <summary>
        ///     Returns a new <see cref="Color"/> with the specified alpha value,
        ///     clamped between 0 and 1.
        /// </summary>
        /// <param name="color"> The original color. </param>
        /// <param name="alpha"> The alpha value to apply, clamped to the range [0, 1]. </param>
        /// <returns>
        ///     A new <see cref="Color"/> with the same RGB components as the original,
        ///     but with the specified alpha value.
        /// </returns>
        public static Color WithAlpha(this Color color, float alpha)
        {
            alpha = Mathf.Clamp01(alpha);
            color.a = alpha;
            return color;
        }

        /// <summary>
        ///     Returns a grayscale color with the specified intensity and alpha.
        ///     <br/>
        ///     The grayscale value is calculated by multiplying white with the given intensity.
        /// </summary>
        /// <param name="intensity"> Grayscale intensity between 0 (black) and 1 (white). </param>
        /// <param name="alpha"> Alpha value for the resulting color (default is 1). </param>
        /// <returns> Returns a new Color with equal RGB channels and specified alpha. </returns>
        public static Color Grayscale(float intensity, float alpha = 1f)
        {
            return (Color.white * intensity).WithAlpha(alpha);
        }
    }
}
