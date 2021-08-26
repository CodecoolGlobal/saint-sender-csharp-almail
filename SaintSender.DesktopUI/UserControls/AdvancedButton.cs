using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using SaintSender.DesktopUI.Utility;

namespace SaintSender.DesktopUI.UserControls
{
    class AdvancedButton : CustomComponent
    {
        #region Designer parameters
        public static readonly DependencyProperty BackgroundProperty =
           DependencyProperty.Register(nameof(Background), typeof(Color), typeof(EmailDisplay), new PropertyMetadata(Colors.White));
        public Color Background
        {
            get => (Color)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }


        public static readonly DependencyProperty ForegroundProperty =
           DependencyProperty.Register(nameof(Foreground), typeof(Color), typeof(EmailDisplay), new PropertyMetadata(Colors.Black));
        public Color Foreground
        {
            get => (Color)GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }


        public static readonly DependencyProperty HoverBackgroundProperty =
           DependencyProperty.Register(nameof(HoverBackground), typeof(Color), typeof(EmailDisplay), new PropertyMetadata(Colors.Blue));
        public Color HoverBackground
        {
            get => (Color)GetValue(HoverBackgroundProperty);
            set => SetValue(HoverBackgroundProperty, value);
        }


        public static readonly DependencyProperty HoverForegroundProperty =
           DependencyProperty.Register(nameof(HoverForeground), typeof(Color), typeof(EmailDisplay), new PropertyMetadata(Colors.White));
        public Color HoverForeground
        {
            get => (Color)GetValue(HoverForegroundProperty);
            set => SetValue(HoverForegroundProperty, value);
        }


        public static readonly DependencyProperty PressedBackgroundProperty =
           DependencyProperty.Register(nameof(PressedBackground), typeof(Color), typeof(EmailDisplay), new PropertyMetadata(Colors.Black));
        public Color PressedBackground
        {
            get => (Color)GetValue(PressedBackgroundProperty);
            set => SetValue(PressedBackgroundProperty, value);
        }


        public static readonly DependencyProperty PressedForegroundProperty =
           DependencyProperty.Register(nameof(PressedForeground), typeof(Color), typeof(EmailDisplay), new PropertyMetadata(Colors.White));
        public Color PressedForeground
        {
            get => (Color)GetValue(PressedForegroundProperty);
            set => SetValue(PressedForegroundProperty, value);
        }

        public static readonly DependencyProperty FontFamilyProperty =
           DependencyProperty.Register(nameof(FontFamily), typeof(string), typeof(EmailDisplay), new PropertyMetadata("Segoe UI"));
        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }

        public static readonly DependencyProperty FontSizeProperty =
           DependencyProperty.Register(nameof(FontSize), typeof(int), typeof(EmailDisplay), new PropertyMetadata(12));
        public int FontSize
        {
            get => (int)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public static readonly DependencyProperty TextProperty =
           DependencyProperty.Register(nameof(Text), typeof(string), typeof(EmailDisplay), new PropertyMetadata("Button"));
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextAlignProperty =
           DependencyProperty.Register(nameof(TextAlign), typeof(int), typeof(EmailDisplay), new PropertyMetadata(2));
        public int TextAlign
        {
            get => (int)GetValue(TextAlignProperty);
            set => SetValue(TextAlignProperty, Math.Max(1, Math.Min(3, value)));
        }

        public static readonly DependencyProperty IconProperty =
           DependencyProperty.Register(nameof(Icon), typeof(string), typeof(EmailDisplay), new PropertyMetadata("apple-alt"));
        public string Icon
        {
            get => (string)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty IconAlignProperty =
           DependencyProperty.Register(nameof(IconAlign), typeof(int), typeof(EmailDisplay), new PropertyMetadata(1));
        public int IconAlign
        {
            get => (int)GetValue(IconAlignProperty);
            set => SetValue(IconAlignProperty, value == 1 || value == 3 ? value : 1);
        }

        public static readonly DependencyProperty HorizontalPaddingProperty =
           DependencyProperty.Register(nameof(HorizontalPadding), typeof(float), typeof(EmailDisplay), new PropertyMetadata(8f));
        public float HorizontalPadding
        {
            get => (float)GetValue(HorizontalPaddingProperty);
            set => SetValue(HorizontalPaddingProperty, value);
        }
        protected override float PaddingHorizonal => HorizontalPadding;
        #endregion



        #region Public
        public event EventHandler OnClick = null;
        #endregion



        #region Rendering
        protected override void Render(DrawingContext drawingContext)
        {
            if (!IsEnabled)
                drawingContext.PushOpacity(0.3);

            drawingContext.DrawRectangle(new SolidColorBrush(BackgroundColor), null, OutsideRect);

            FormattedText labelText = DrawUtil.FormatText(Text, new SolidColorBrush(ForegroundColor), FontSize, false, FontFamily);

            float labelX = InsideLeft + InsideWidth / 2f - (float)labelText.Width / 2f;

            if (TextAlign == 1)
                labelX = InsideLeft;
            else if (TextAlign == 3)
                labelX = InsideRight - (float)labelText.Width;

            drawingContext.DrawText(labelText, new Point(labelX, OutsideHeight / 2 - (float)labelText.Height / 2));

            if (!IsEnabled)
                drawingContext.Pop();
        }
        #endregion



        #region Events
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (mouseLeftButton && OnClick != null)
                OnClick.Invoke(this, new EventArgs());

            base.OnMouseLeftButtonUp(e);
        }
        #endregion



        #region Private & Protected
        protected override bool RestrictedRendering => false;

        private Color BackgroundColor
        {
            get
            {
                if (mouseInside && mouseLeftButton)
                    return PressedBackground;
                if (mouseInside)
                    return HoverBackground;
                return Background;
            }
        }
        private Color ForegroundColor
        {
            get
            {
                if (mouseInside && mouseLeftButton)
                    return PressedForeground;
                if (mouseInside)
                    return HoverForeground;
                return Foreground;
            }
        }
        #endregion
    }
}
