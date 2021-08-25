using System.Windows;
using System.Windows.Media;
using static SaintSender.DesktopUI.Utility.DrawUtil;

namespace SaintSender.DesktopUI.Utility
{
    internal static class DrawingContextExtension
    {
        /// <summary>
        /// Sets a clip rectangle for restrict drawing area.
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Rectangle width</param>
        /// <param name="height">Rectangle height</param>
        public static void ClipRectangle(this DrawingContext context, float x, float y, float width, float height)
        {
            context.ClipRectangle(new Rect(x, y, width, height));
        }
        /// <summary>
        /// Sets a clip rectangle for restrict drawing area.
        /// </summary>
        /// <param name="rectangle">Rectangle</param>
        public static void ClipRectangle(this DrawingContext context, Rect rectangle)
        {
            context.PushClip(new RectangleGeometry(rectangle));
        }

        /// <summary>
        /// Resets all clip rectangles
        /// </summary>
        public static void ResetClip(this DrawingContext context)
        {
            context.Pop();
        }

        /// <summary>
        /// Draws a linear gradient in a rectangle with evenly distributed colors
        /// </summary>
        /// <param name="rectangle">Target rectangle</param>
        /// <param name="colors">Array of colors</param>
        /// <param name="angle">Gradient angle in degrees (0 - horizontal, 90 - vertical)</param>
        public static void DrawGradient(this DrawingContext context, Rect rectangle, Color[] colors, float angle = 0)
        {
            context.DrawRectangle(new LinearGradientBrush(DrawUtil.Gradient(colors), (double)angle), null, rectangle);
        }

        public static void DrawIcon(this DrawingContext context, string iconName, Color color, Rect rectangle, out Size textSize, IconStyle style = IconStyle.Solid, int fontSize = 12)
        {
            textSize = new Size(0, 0);
            if (!Icons[(int)style].ContainsKey(iconName))
                return;

            /*//FormattedText iconText = FormatText(Icons[(int)style][iconName], new SolidColorBrush(color), FontawesomeFont, fontSize, false);
            FormattedText iconText = FormatText(Icons[(int)style][iconName], new SolidColorBrush(color), FontawesomeFont, fontSize, false);
            textSize = new Size(iconText.Width, iconText.Height);
            context.DrawText(iconText, new Point(rectangle.X + rectangle.Width / 2 - iconText.Width / 2, rectangle.Y + rectangle.Height / 2 - iconText.Height / 2));*/
        }
    }
}
