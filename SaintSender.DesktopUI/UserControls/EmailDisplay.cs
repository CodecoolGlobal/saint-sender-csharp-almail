using System;
using System.Windows;
using System.Windows.Media;
using SaintSender.Core.Models;
using SaintSender.DesktopUI.Utility;

namespace SaintSender.DesktopUI.UserControls
{
    class EmailDisplay : CustomComponent
    {
        #region Designer parameters
        public static readonly DependencyProperty LineHeightProperty =
            DependencyProperty.Register(nameof(LineHeight), typeof(float), typeof(EmailDisplay), new PropertyMetadata(30f));
        public float LineHeight
        {
            get => (float)GetValue(LineHeightProperty);
            set => SetValue(LineHeightProperty, value);
        }


        public static readonly DependencyProperty TitleWidthProperty =
           DependencyProperty.Register(nameof(TitleWidth), typeof(float), typeof(EmailDisplay), new PropertyMetadata(0.2f));
        public float TitleWidth
        {
            get => (float)GetValue(TitleWidthProperty);
            set => SetValue(TitleWidthProperty, value);
        }


        public static readonly DependencyProperty DateWidthProperty =
           DependencyProperty.Register(nameof(DateWidth), typeof(float), typeof(EmailDisplay), new PropertyMetadata(0.16f));
        public float DateWidth
        {
            get => (float)GetValue(DateWidthProperty);
            set => SetValue(DateWidthProperty, value);
        }


        public static readonly DependencyProperty ListItemBackgroundProperty =
           DependencyProperty.Register(nameof(ListItemBackground), typeof(Color), typeof(EmailDisplay), new PropertyMetadata(Colors.Transparent));
        public Color ListItemBackground
        {
            get => (Color)GetValue(ListItemBackgroundProperty);
            set => SetValue(ListItemBackgroundProperty, value);
        }


        public static readonly DependencyProperty ListItemForegroundProperty =
           DependencyProperty.Register(nameof(ListItemForeground), typeof(Color), typeof(EmailDisplay), new PropertyMetadata(Colors.Black));
        public Color ListItemForeground
        {
            get => (Color)GetValue(ListItemForegroundProperty);
            set => SetValue(ListItemForegroundProperty, value);
        }


        public static readonly DependencyProperty ListItemHoverBackgroundProperty =
           DependencyProperty.Register(nameof(ListItemHoverBackground), typeof(Color), typeof(EmailDisplay), new PropertyMetadata(Colors.Transparent));
        public Color ListItemHoverBackground
        {
            get => (Color)GetValue(ListItemHoverBackgroundProperty);
            set => SetValue(ListItemHoverBackgroundProperty, value);
        }


        public static readonly DependencyProperty ListItemHoverForegroundProperty =
           DependencyProperty.Register(nameof(ListItemHoverForeground), typeof(Color), typeof(EmailDisplay), new PropertyMetadata(Colors.Black));
        public Color ListItemHoverForeground
        {
            get => (Color)GetValue(ListItemHoverForegroundProperty);
            set => SetValue(ListItemHoverForegroundProperty, value);
        }


        public static readonly DependencyProperty BorderThicknessProperty =
           DependencyProperty.Register(nameof(BorderThickness), typeof(float), typeof(EmailDisplay), new PropertyMetadata(5f));
        public float BorderThickness
        {
            get => (float)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }


        public static readonly DependencyProperty SidePaddingProperty =
           DependencyProperty.Register(nameof(SidePadding), typeof(float), typeof(EmailDisplay), new PropertyMetadata(20f));
        public float SidePadding
        {
            get => (float)GetValue(SidePaddingProperty);
            set => SetValue(SidePaddingProperty, value);
        }
        protected override float PaddingHorizonal => SidePadding;
        #endregion



        #region Rendering
        protected override void Render(DrawingContext drawingContext)
        {
            drawingContext.DrawText(DrawUtil.FormatText(string.Format("{1}px{0}{2}{0}{3}", "\n", scrollY, OutsideHeight, AllEmailsHeight), Brushes.Red, 15, true), new Point(16, 16));

            for (int emailIndex = 0; emailIndex < emails.Length; emailIndex++)
                RenderEmailListItem(drawingContext, emails[emailIndex], emailIndex);

            RenderScrollbar(drawingContext);
        }

        /// <summary>
        /// Draws a line of email list
        /// </summary>
        /// <param name="drawingContext">The DrawingContext instance</param>
        /// <param name="email">The EmailData instance</param>
        /// <param name="yPosition">The line Y position</param>
        void RenderEmailListItem(DrawingContext drawingContext, EmailMessage email, int index)
        {

            float yPosition = -scrollY + index * ItemHeight;

            drawingContext.ClipRectangle(0, yPosition, OutsideWidth, LineHeight);

            Color backColor = ItemBackground(index);
            Color foreColor = ItemForeground(index);

            drawingContext.DrawRectangle(new SolidColorBrush(backColor), null, new Rect(new Point(0, yPosition), new Size(OutsideWidth, LineHeight)));

            drawingContext.ClipRectangle(InsideLeft, yPosition, ShrinkWidth * DateWidth, LineHeight);
            FormattedText emailText = DrawUtil.FormatText(email.Sender, new SolidColorBrush(foreColor), 12, true);
            FormattedText dateText = DrawUtil.FormatText(email.SentTime.ToString("yyyy.MM.dd HH:mm"), new SolidColorBrush(foreColor), 12);
            drawingContext.DrawText(emailText, new Point(InsideLeft, yPosition + LineHeight / 2 - emailText.Height));
            drawingContext.DrawText(dateText, new Point(InsideLeft, yPosition + LineHeight / 2));
            drawingContext.ResetClip();

            drawingContext.ClipRectangle(InsideLeft + SidePadding + ShrinkWidth * DateWidth, yPosition, ShrinkWidth * TitleWidth, LineHeight);
            FormattedText titleText = DrawUtil.FormatText(email.Subject, new SolidColorBrush(foreColor), 14, true);
            drawingContext.DrawText(titleText, new Point(InsideLeft + SidePadding + ShrinkWidth * DateWidth, yPosition + LineHeight / 2f - titleText.Height / 2f));
            drawingContext.ResetClip();

            if (titleText.Width > ShrinkWidth * TitleWidth)
                drawingContext.DrawGradient(new Rect(InsideLeft + ShrinkWidth * (DateWidth + TitleWidth), yPosition, SidePadding * 2, LineHeight), new Color[] { DrawUtil.ColorAlpha(backColor, 0), DrawUtil.ColorAlpha(backColor, 1), DrawUtil.ColorAlpha(backColor, 0) });

            //drawingContext.ClipRectangle()
            FormattedText bodyText = DrawUtil.FormatText(email.Body, new SolidColorBrush(foreColor), 12);
            drawingContext.DrawText(bodyText, new Point(InsideLeft + SidePadding * 2 + ShrinkWidth * (DateWidth + TitleWidth), bodyText.Height <= LineHeight ? yPosition + LineHeight / 2 - bodyText.Height / 2 : yPosition + 8));

            drawingContext.ResetClip();
        }

        /// <summary>
        /// Renders a faded small "scrollbar" rectangle on the right side of the component
        /// </summary>
        /// <param name="drawingContext">The DrawingContext instance</param>
        void RenderScrollbar(DrawingContext drawingContext)
        {
            if (!mouseInside)
                return;

            float viewScale = OutsideHeight / AllEmailsHeight;

            if (viewScale >= 1)
                return;

            float scrollbarHeight = OutsideHeight * viewScale;

            drawingContext.DrawRectangle(new SolidColorBrush(Color.FromArgb(100, 0, 0, 0)), null, new Rect(OutsideRight - 8, viewScale * scrollY, 8, scrollbarHeight));
        }
        #endregion



        #region Events
        protected override void OnScroll(float scrollDelta)
        {
            scrollY -= scrollDelta;
            scrollY = Math.Max(0, scrollY);
            scrollY = Math.Min(ScrollMax, scrollY);

            if (AllEmailsHeight <= OutsideHeight)
                scrollY = 0;
        }
        #endregion



        #region Public
        public void UpdateEmailList(EmailMessage[] emailList)
        {
            emails = emailList;
            Refresh();
        }
        #endregion



        #region Private & Protected
        private float scrollY = 0;
        private float ShrinkWidth => InsideWidth - PaddingHorizonal * 2;

        protected override bool Scrollable => true;

        private int HoverIndex
        {
            get
            {
                if (!mouseInside)
                    return -1;

                return (int)Math.Floor((mousePosition.Y + scrollY) / ItemHeight);
            }
        }

        private float ItemHeight => LineHeight + BorderThickness;

        private float AllEmailsHeight => emails.Length * ItemHeight;

        private float ScrollMax
        {
            get
            {
                if (emails == null)
                    return 0;

                return AllEmailsHeight - OutsideHeight;
            }
        }

        Color ItemBackground(int itemIndex)
        {
            if (itemIndex == HoverIndex)
                return ListItemHoverBackground;
            return ListItemBackground;
        }
        Color ItemForeground(int itemIndex)
        {
            if (itemIndex == HoverIndex)
                return ListItemHoverForeground;
            return ListItemForeground;
        }
        #endregion



        EmailMessage[] emails = new EmailMessage[0];
    }
}
