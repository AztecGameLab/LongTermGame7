using System.Diagnostics.CodeAnalysis;

namespace Application.Core.Rtf
{
    /// <summary>
    /// General-purpose extensions to quickly add color to messages.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Documentation is self-evident.")]
    public static class RtfColorExtensions
    {
        public static string Aqua(this string message)
        {
            return message.Format(Rtf.AquaColor);
        }

        public static string Black(this string message)
        {
            return message.Format(Rtf.BlackColor);
        }

        public static string Blue(this string message)
        {
            return message.Format(Rtf.BlueColor);
        }

        public static string Brown(this string message)
        {
            return message.Format(Rtf.BrownColor);
        }

        public static string Cyan(this string message)
        {
            return message.Format(Rtf.CyanColor);
        }

        public static string DarkBlue(this string message)
        {
            return message.Format(Rtf.DarkBlueColor);
        }

        public static string Fuchsia(this string message)
        {
            return message.Format(Rtf.FuchsiaColor);
        }

        public static string Green(this string message)
        {
            return message.Format(Rtf.GreenColor);
        }

        public static string Grey(this string message)
        {
            return message.Format(Rtf.GreyColor);
        }

        public static string LightBlue(this string message)
        {
            return message.Format(Rtf.LightBlueColor);
        }

        public static string Lime(this string message)
        {
            return message.Format(Rtf.LimeColor);
        }

        public static string Magenta(this string message)
        {
            return message.Format(Rtf.MagentaColor);
        }

        public static string Maroon(this string message)
        {
            return message.Format(Rtf.MaroonColor);
        }

        public static string Navy(this string message)
        {
            return message.Format(Rtf.NavyColor);
        }

        public static string Olive(this string message)
        {
            return message.Format(Rtf.OliveColor);
        }

        public static string Orange(this string message)
        {
            return message.Format(Rtf.OrangeColor);
        }

        public static string Purple(this string message)
        {
            return message.Format(Rtf.PurpleColor);
        }

        public static string Red(this string message)
        {
            return message.Format(Rtf.RedColor);
        }

        public static string Silver(this string message)
        {
            return message.Format(Rtf.SilverColor);
        }

        public static string Teal(this string message)
        {
            return message.Format(Rtf.TealColor);
        }

        public static string White(this string message)
        {
            return message.Format(Rtf.WhiteColor);
        }

        public static string Yellow(this string message)
        {
            return message.Format(Rtf.YellowColor);
        }
    }
}