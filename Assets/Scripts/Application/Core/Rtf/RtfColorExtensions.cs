namespace Application.Core.Rtf
{
    /// <summary>
    /// General-purpose extensions to quickly add color to messages.
    /// </summary>
    public static class RtfColorExtensions
    {
        /// <inheritdoc cref="Rtf.AquaColor"/>
        public static string Aqua(this string message)
        {
            return message.Format(Rtf.AquaColor);
        }

        /// <inheritdoc cref="Rtf.BlackColor"/>
        public static string Black(this string message)
        {
            return message.Format(Rtf.BlackColor);
        }

        /// <inheritdoc cref="Rtf.BlueColor"/>
        public static string Blue(this string message)
        {
            return message.Format(Rtf.BlueColor);
        }

        /// <inheritdoc cref="Rtf.BrownColor"/>
        public static string Brown(this string message)
        {
            return message.Format(Rtf.BrownColor);
        }

        /// <inheritdoc cref="Rtf.CyanColor"/>
        public static string Cyan(this string message)
        {
            return message.Format(Rtf.CyanColor);
        }

        /// <inheritdoc cref="Rtf.DarkBlueColor"/>
        public static string DarkBlue(this string message)
        {
            return message.Format(Rtf.DarkBlueColor);
        }

        /// <inheritdoc cref="Rtf.FuschiaColor"/>
        public static string Fuchsia(this string message)
        {
            return message.Format(Rtf.FuchsiaColor);
        }

        /// <inheritdoc cref="Rtf.GreenColor"/>
        public static string Green(this string message)
        {
            return message.Format(Rtf.GreenColor);
        }

        /// <inheritdoc cref="Rtf.GreyColor"/>
        public static string Grey(this string message)
        {
            return message.Format(Rtf.GreyColor);
        }

        /// <inheritdoc cref="Rtf.LightBlueColor"/>
        public static string LightBlue(this string message)
        {
            return message.Format(Rtf.LightBlueColor);
        }

        /// <inheritdoc cref="Rtf.LimeColor"/>
        public static string Lime(this string message)
        {
            return message.Format(Rtf.LimeColor);
        }

        /// <inheritdoc cref="Rtf.MagentaColor"/>
        public static string Magenta(this string message)
        {
            return message.Format(Rtf.MagentaColor);
        }

        /// <inheritdoc cref="Rtf.MaroonColor"/>
        public static string Maroon(this string message)
        {
            return message.Format(Rtf.MaroonColor);
        }

        /// <inheritdoc cref="Rtf.NavyColor"/>
        public static string Navy(this string message)
        {
            return message.Format(Rtf.NavyColor);
        }

        /// <inheritdoc cref="Rtf.OliveColor"/>
        public static string Olive(this string message)
        {
            return message.Format(Rtf.OliveColor);
        }

        /// <inheritdoc cref="Rtf.OrangeColor"/>
        public static string Orange(this string message)
        {
            return message.Format(Rtf.OrangeColor);
        }

        /// <inheritdoc cref="Rtf.PurpleColor"/>
        public static string Purple(this string message)
        {
            return message.Format(Rtf.PurpleColor);
        }

        /// <inheritdoc cref="Rtf.RedColor"/>
        public static string Red(this string message)
        {
            return message.Format(Rtf.RedColor);
        }

        /// <inheritdoc cref="Rtf.SilverColor"/>
        public static string Silver(this string message)
        {
            return message.Format(Rtf.SilverColor);
        }

        /// <inheritdoc cref="Rtf.TealColor"/>
        public static string Teal(this string message)
        {
            return message.Format(Rtf.TealColor);
        }

        /// <inheritdoc cref="Rtf.WhiteColor"/>
        public static string White(this string message)
        {
            return message.Format(Rtf.WhiteColor);
        }

        /// <inheritdoc cref="Rtf.YellowColor"/>
        public static string Yellow(this string message)
        {
            return message.Format(Rtf.YellowColor);
        }
    }
}
