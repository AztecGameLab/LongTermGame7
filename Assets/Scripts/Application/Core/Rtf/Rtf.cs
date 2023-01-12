using System.Linq;
using System.Text;
using UnityEngine;

namespace Application.Core.Rtf
{
    /// <summary>
    /// Provides utility methods for quickly encoding rich-text information into strings,
    /// such as bold, italic, size, color, ect.
    /// </summary>
    public static partial class Rtf
    {
        /// <summary>
        /// An Rtf tag that makes a string bold.
        /// <seealso cref="RtfExtensions.Bold"/>
        /// </summary>
        public static readonly IRichTextData Bold = default(BoldFormat);

        /// <summary>
        /// An Rtf tag that makes a string italic.
        /// </summary>
        /// <see cref="RtfExtensions.Italic"/>
        public static readonly IRichTextData Italic = default(ItalicFormat);

        /// <summary>
        /// Common error formatting.
        /// </summary>
        public static readonly IRichTextData Error = Composite(Bold, UnityEngine.Color.red.Rtf());

        /// <summary>
        /// Common warning formatting.
        /// </summary>
        public static readonly IRichTextData Warning = Composite(Bold, UnityEngine.Color.yellow.Rtf());

        /// <summary>
        /// Common hint formatting.
        /// </summary>
        public static readonly IRichTextData Hint = Composite(Italic, UnityEngine.Color.gray.Rtf());

        /// <summary>
        /// Common success formatting.
        /// </summary>
        public static readonly IRichTextData Success = Composite(Bold, UnityEngine.Color.green.Rtf());

        /// <summary>
        /// Common failure formatting.
        /// </summary>
        public static readonly IRichTextData Failure = Composite(Bold, UnityEngine.Color.red.Rtf());

        /// <summary>
        /// Generates an Rtf tag that makes a string a certain color.
        /// <seealso cref="RtfColorExtensions"/>
        /// <seealso cref="RtfExtensions.Color"/>
        /// </summary>
        /// <param name="hexColor">The hex-code of the color: for example, #FF22BB00.</param>
        /// <returns>The generated tag data.</returns>
        public static IRichTextData Color(string hexColor)
        {
            return new ColorFormat(hexColor);
        }

        /// <summary>
        /// Generates an Rtf tag that makes a string a certain color.
        /// <seealso cref="RtfColorExtensions"/>
        /// <seealso cref="RtfExtensions.Color"/>
        /// </summary>
        /// <param name="color">The Unity color struct to use.</param>
        /// <returns>The generated tag data.</returns>
        public static IRichTextData Color(Color color)
        {
            return color.Rtf();
        }

        /// <summary>
        /// Generates an Rtf tag that makes a string a certain size.
        /// </summary>
        /// <param name="pixels">The font-size of this string, in pixels.</param>
        /// <seealso cref="RtfExtensions.Size"/>
        /// <returns>The generated tag data.</returns>
        public static IRichTextData Size(int pixels)
        {
            return new SizeFormat(pixels);
        }

        /// <summary>
        /// The most un-used RTF tag. Who knows, maybe someone will use it! todo: figure out how this works.
        /// </summary>
        /// <param name="index">Who knows.</param>
        /// <returns>The generated tag data.</returns>
        public static IRichTextData Material(ushort index)
        {
            return new MaterialFormat(index);
        }

        /// <summary>
        /// Generates an Rtf tag that combines a group of other tags.
        /// </summary>
        /// <param name="rtfData">The tags to consolidate into one big tag.</param>
        /// <returns>The generated tag data.</returns>
        public static IRichTextData Composite(params IRichTextData[] rtfData)
        {
            return new CompositeFormat(rtfData);
        }

        private readonly struct BoldFormat : IRichTextData
        {
            public string Opener => "<b>";

            public string Closer => "</b>";
        }

        private readonly struct ItalicFormat : IRichTextData
        {
            public string Opener => "<i>";

            public string Closer => "</i>";
        }

        private readonly struct ColorFormat : IRichTextData
        {
            private readonly string _hexColor;

            public ColorFormat(string hexColor)
            {
                _hexColor = hexColor;
            }

            public string Opener => $"<color={_hexColor}>";

            public string Closer => "</color>";
        }

        private readonly struct SizeFormat : IRichTextData
        {
            private readonly int _pixels;

            public SizeFormat(int pixels)
            {
                _pixels = pixels;
            }

            public string Opener => $"<size={_pixels}>";

            public string Closer => "</size>";
        }

        private readonly struct MaterialFormat : IRichTextData
        {
            private readonly ushort _index;

            public MaterialFormat(ushort index)
            {
                _index = index;
            }

            public string Opener => $"<material={_index}>";

            public string Closer => "</material>";
        }

        private readonly struct CompositeFormat : IRichTextData
        {
            public CompositeFormat(params IRichTextData[] data)
            {
                // Aggregate openers.
                var sbO = new StringBuilder();

                foreach (var textData in data)
                {
                    sbO.Append(textData.Opener);
                }

                Opener = sbO.ToString();

                // Aggregate closers.
                var sbC = new StringBuilder();

                foreach (var textData in data.Reverse())
                {
                    sbC.Append(textData.Closer);
                }

                Closer = sbC.ToString();
            }

            public string Opener { get; }

            public string Closer { get; }
        }
    }
}