using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SaintSender.DesktopUI.UserControls
{
    class DrawUtil
    {
        /// <summary>
        /// Formats a text by style parameters
        /// </summary>
        /// <param name="text">The text string</param>
        /// <param name="color">The color brush</param>
        /// <param name="fontSize">The size of the text</param>
        /// <param name="bold">Is the text bold?</param>
        /// <param name="fontFamily">Font family name as string</param>
        /// <returns>Measurable FormattedText instance</returns>
        public static FormattedText FormatText(string text, Brush color, int fontSize = 12, bool bold = false, string fontFamily = "Segoe UI")
        {
            return new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                                new Typeface(new FontFamily(fontFamily),
                                        FontStyles.Normal,
                                        bold ? FontWeights.Bold : FontWeights.Normal,
                                        FontStretches.Normal),
                                    fontSize,
                                    color,
                                    null,
                                    TextFormattingMode.Display);
        }

        /// <summary>
        /// Changes the alpha value of a color and returns it.
        /// </summary>
        /// <param name="color">Base color</param>
        /// <param name="alpha">The alpha (between 0 and 1)</param>
        /// <param name="rgbMultiplier">The Red, Green, Blue component's multiplier (between 0 and 1)</param>
        /// <returns>Alpha changed color</returns>
        public static Color ColorAlpha(Color color, float alpha = 1, float rgbMultiplier = 1)
        {
            return Color.FromArgb((byte)Math.Round(alpha * 255), (byte)(color.R * rgbMultiplier), (byte)(color.G * rgbMultiplier), (byte)(color.B * rgbMultiplier));
        }
    }
}
