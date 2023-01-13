namespace Application.Core.Rtf
{
    using UnityEngine;

    /// <summary>
    /// General-purpose extensions to make the Rtf application process more natural.
    /// </summary>
    public static class RtfExtensions
    {
        /// <summary>
        /// Applies a set of Rtf tags to a given string.
        /// </summary>
        /// <param name="message">The string to apply the tags.</param>
        /// <param name="rtfData">The tags to apply.</param>
        /// <returns>The modified string.</returns>
        public static string Format(this string message, params IRichTextData[] rtfData)
        {
            if (rtfData == null)
            {
                return message;
            }

            foreach (var formatData in rtfData)
            {
                message = formatData.Apply(message);
            }

            return message;
        }

        /// <summary>
        /// Applies a string to an Rtf tag.
        /// </summary>
        /// <param name="rtfData">The tag to use.</param>
        /// <param name="message">The string to apply the tag.</param>
        /// <returns>The modified string.</returns>
        public static string Apply(this IRichTextData rtfData, string message)
        {
            if (rtfData == null)
            {
                return message;
            }

            return $"{rtfData.Opener}{message}{rtfData.Closer}";
        }

        /// <summary>
        /// Generates an Rtf tag to apply a given color.
        /// </summary>
        /// <param name="color">The color to use when applying the tag.</param>
        /// <returns>An Rtf tag to apply the desired color.</returns>
        public static IRichTextData Rtf(this Color color)
        {
            string hexColor = '#' + ColorUtility.ToHtmlStringRGBA(color);
            return Core.Rtf.Rtf.Color(hexColor);
        }

        /// <summary>
        /// Makes a string bold.
        /// </summary>
        /// <param name="message">The string to modify.</param>
        /// <returns>The modified string.</returns>
        public static string Bold(this string message)
        {
            return message.Format(Core.Rtf.Rtf.Bold);
        }

        /// <summary>
        /// Makes a string italic.
        /// </summary>
        /// <param name="message">The string to modify.</param>
        /// <returns>The modified string.</returns>
        public static string Italic(this string message)
        {
            return message.Format(Core.Rtf.Rtf.Italic);
        }

        /// <summary>
        /// Makes a string colored.
        /// </summary>
        /// <param name="message">The string to modify.</param>
        /// <param name="targetColor">The color to apply.</param>
        /// <returns>The modified string.</returns>
        public static string Color(this string message, Color targetColor)
        {
            return message.Format(targetColor.Rtf());
        }

        /// <summary>
        /// Makes a string a desired size.
        /// </summary>
        /// <param name="message">The string to modify.</param>
        /// <param name="targetSize">The desired font-size, in pixels.</param>
        /// <returns>The modified string.</returns>
        public static string Size(this string message, int targetSize)
        {
            return message.Format(Core.Rtf.Rtf.Size(targetSize));
        }

        /// <summary>
        /// Makes a string a certain material.
        /// </summary>
        /// <param name="message">The string to modify.</param>
        /// <param name="id">The material id. </param> // todo: figure out what this id actually means...
        /// <returns>The modified string.</returns>
        public static string Material(this string message, ushort id)
        {
            return message.Format(Core.Rtf.Rtf.Material(id));
        }
    }
}
