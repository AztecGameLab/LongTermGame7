namespace Application.Core.Rtf
{
    using System;
    using System.Linq;
    using System.Text;
    using Color = UnityEngine.Color;

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
        public static readonly IRichTextData Bold = new BoldFormat();

        /// <summary>
        /// An Rtf tag that makes a string italic.
        /// </summary>
        /// <see cref="RtfExtensions.Italic"/>
        public static readonly IRichTextData Italic = new ItalicFormat();

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
        /// <param name="unityColor">The Unity color struct to use.</param>
        /// <returns>The generated tag data.</returns>
        public static IRichTextData Color(Color unityColor)
        {
            return unityColor.Rtf();
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
        /// Generates an Rtf tag that makes a string use a custom material.
        /// </summary>
        /// <param name="index">The index of the text-mesh's material to use.</param>
        /// <returns>The generated tag data.</returns>
        /// <remarks>This tag is only useful for text meshes.</remarks>
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

        private readonly struct BoldFormat : IRichTextData, IEquatable<BoldFormat>
        {
            public string Opener => "<b>";

            public string Closer => "</b>";

            public bool Equals(BoldFormat other)
            {
                return true;
            }

            public override bool Equals(object obj)
            {
                return obj is BoldFormat other && Equals(other);
            }

            public override int GetHashCode()
            {
                return 0;
            }
        }

        private readonly struct ItalicFormat : IRichTextData, IEquatable<ItalicFormat>
        {
            public string Opener => "<i>";

            public string Closer => "</i>";

            public bool Equals(ItalicFormat other)
            {
                return true;
            }

            public override bool Equals(object obj)
            {
                return obj is ItalicFormat other && Equals(other);
            }

            public override int GetHashCode()
            {
                return 0;
            }
        }

        private readonly struct ColorFormat : IRichTextData, IEquatable<ColorFormat>
        {
            private readonly string _hexColor;

            public ColorFormat(string hexColor)
            {
                _hexColor = hexColor;
            }

            public string Opener => $"<color={_hexColor}>";

            public string Closer => "</color>";

            public bool Equals(ColorFormat other)
            {
                return _hexColor == other._hexColor;
            }

            public override bool Equals(object obj)
            {
                return obj is ColorFormat other && Equals(other);
            }

            public override int GetHashCode()
            {
                return _hexColor != null ? _hexColor.GetHashCode() : 0;
            }
        }

        private readonly struct SizeFormat : IRichTextData, IEquatable<SizeFormat>
        {
            private readonly int _pixels;

            public SizeFormat(int pixels)
            {
                _pixels = pixels;
            }

            public string Opener => $"<size={_pixels}>";

            public string Closer => "</size>";

            public bool Equals(SizeFormat other)
            {
                return _pixels == other._pixels;
            }

            public override bool Equals(object obj)
            {
                return obj is SizeFormat other && Equals(other);
            }

            public override int GetHashCode()
            {
                return _pixels;
            }
        }

        private readonly struct MaterialFormat : IRichTextData, IEquatable<MaterialFormat>
        {
            private readonly ushort _index;

            public MaterialFormat(ushort index)
            {
                _index = index;
            }

            public string Opener => $"<material={_index}>";

            public string Closer => "</material>";

            public bool Equals(MaterialFormat other)
            {
                return _index == other._index;
            }

            public override bool Equals(object obj)
            {
                return obj is MaterialFormat other && Equals(other);
            }

            public override int GetHashCode()
            {
                return _index.GetHashCode();
            }
        }

        private readonly struct CompositeFormat : IRichTextData, IEquatable<CompositeFormat>
        {
            public CompositeFormat(params IRichTextData[] data)
            {
                // Aggregate openers.
                var openerBuilder = new StringBuilder();

                foreach (var textData in data)
                {
                    openerBuilder.Append(textData.Opener);
                }

                Opener = openerBuilder.ToString();

                // Aggregate closers.
                var closerBuilder = new StringBuilder();

                foreach (var textData in data.Reverse())
                {
                    closerBuilder.Append(textData.Closer);
                }

                Closer = closerBuilder.ToString();
            }

            public string Opener { get; }

            public string Closer { get; }

            public bool Equals(CompositeFormat other)
            {
                return Opener == other.Opener && Closer == other.Closer;
            }

            public override bool Equals(object obj)
            {
                return obj is CompositeFormat other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Opener != null ? Opener.GetHashCode() : 0) * 397) ^ (Closer != null ? Closer.GetHashCode() : 0);
                }
            }
        }
    }
}
