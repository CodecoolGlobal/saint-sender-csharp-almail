﻿using System.Windows;
using System.Windows.Media;

namespace SaintSender.DesktopUI.UserControls
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
            context.PushClip(new RectangleGeometry(new Rect((double)x, (double)y, (double)width, (double)height)));
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
    }
}
