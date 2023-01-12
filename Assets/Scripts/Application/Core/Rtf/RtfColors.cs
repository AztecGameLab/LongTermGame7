using System.Diagnostics.CodeAnalysis;

namespace Application.Core.Rtf
{
    /// <summary>
    /// A collection of color tag shortcuts.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Documentation is self-evident.")]
    public static partial class Rtf
    {
        public static readonly IRichTextData AquaColor = new ColorFormat("#00ffffff");

        public static readonly IRichTextData BlackColor = new ColorFormat("#000000ff");

        public static readonly IRichTextData BlueColor = new ColorFormat("#0000ffff");

        public static readonly IRichTextData BrownColor = new ColorFormat("#a52a2aff");

        public static readonly IRichTextData CyanColor = new ColorFormat("#00ffffff");

        public static readonly IRichTextData DarkBlueColor = new ColorFormat("#0000a0ff");

        public static readonly IRichTextData FuchsiaColor = new ColorFormat("#ff00ffff");

        public static readonly IRichTextData GreenColor = new ColorFormat("#008000ff");

        public static readonly IRichTextData GreyColor = new ColorFormat("#808080ff");

        public static readonly IRichTextData LightBlueColor = new ColorFormat("#add8e6ff");

        public static readonly IRichTextData LimeColor = new ColorFormat("#00ff00ff");

        public static readonly IRichTextData MagentaColor = new ColorFormat("#ff00ffff");

        public static readonly IRichTextData MaroonColor = new ColorFormat("#800000ff");

        public static readonly IRichTextData NavyColor = new ColorFormat("#000080ff");

        public static readonly IRichTextData OliveColor = new ColorFormat("#808000ff");

        public static readonly IRichTextData OrangeColor = new ColorFormat("#ffa500ff");

        public static readonly IRichTextData PurpleColor = new ColorFormat("#800080ff");

        public static readonly IRichTextData RedColor = new ColorFormat("#ff0000ff");

        public static readonly IRichTextData SilverColor = new ColorFormat("#c0c0c0ff");

        public static readonly IRichTextData TealColor = new ColorFormat("#008080ff");

        public static readonly IRichTextData WhiteColor = new ColorFormat("#ffffffff");

        public static readonly IRichTextData YellowColor = new ColorFormat("#ffff00ff");
    }
}