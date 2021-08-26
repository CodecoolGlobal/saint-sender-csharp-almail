using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using SaintSender.Core.Interfaces;
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
            if (openedEmail == null)
            {
                for (int emailIndex = 0; emailIndex < emails.Length; emailIndex++)
                    RenderEmailListItem(drawingContext, emails[emailIndex], emailIndex);
            }
            else
                RenderEmailDetails(drawingContext);

            RenderScrollbar(drawingContext);
        }

        void RenderEmailDetails(DrawingContext drawingContext)
        {
            SolidColorBrush backColor = new SolidColorBrush(ItemBackground(-2));
            SolidColorBrush foreColor = new SolidColorBrush(ItemForeground(-2));

            float cursorY = -scrollY;

            // From & To & Date:
            FormattedText fromLabelText = DrawUtil.FormatText("From:", foreColor, 14, true);
            FormattedText fromText = DrawUtil.FormatText(openedEmail.Sender + "\n" + openedEmail.SentTime.ToString("yyyy.MM.dd HH:mm"), foreColor, 13, false);
            Rect fromRect = new Rect(0, cursorY, (OutsideWidth - SidePadding) / 2, fromLabelText.Height + fromText.Height + SidePadding * 2);

            FormattedText toLabelText = DrawUtil.FormatText("To:", foreColor, 14, true);
            string toList = string.Join(",\n", openedEmail.Receiver);
            FormattedText toText = DrawUtil.FormatText(toList, foreColor, 13);
            Rect toRect = new Rect(fromRect.Width + SidePadding, cursorY, (OutsideWidth - SidePadding) / 2, toLabelText.Height + toText.Height + SidePadding * 2);

            if (fromRect.Height > toRect.Height)
                toRect.Height = fromRect.Height;
            if (toRect.Height > fromRect.Height)
                fromRect.Height = toRect.Height;

            drawingContext.DrawRectangle(backColor, null, fromRect);
            drawingContext.DrawText(fromLabelText, new Point(fromRect.X + SidePadding, fromRect.Y + SidePadding));
            drawingContext.DrawText(fromText, new Point(fromRect.X + SidePadding, fromRect.Y + SidePadding + fromLabelText.Height));

            drawingContext.DrawRectangle(backColor, null, toRect);
            drawingContext.DrawText(toLabelText, new Point(toRect.X + SidePadding, toRect.Y + SidePadding));
            drawingContext.DrawText(toText, new Point(toRect.X + SidePadding, toRect.Y + SidePadding + toLabelText.Height));

            cursorY += (float)toRect.Height + SidePadding;

            // Body
            FormattedText bodyText = DrawUtil.FormatText(openedEmail.Body, foreColor, 12);
            Rect bodyRect = new Rect(0, cursorY, OutsideWidth, bodyText.Height + SidePadding * 2);
            drawingContext.DrawRectangle(backColor, null, bodyRect);
            drawingContext.DrawText(bodyText, new Point(bodyRect.X + SidePadding, bodyRect.Y + SidePadding));

            openedViewHeight = cursorY + (float)bodyRect.Height + SidePadding + scrollY;
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
            FormattedText titleText = DrawUtil.FormatText(email.Subject, new SolidColorBrush(foreColor), 14, !email.IsRead);
            drawingContext.DrawText(titleText, new Point(InsideLeft + SidePadding + ShrinkWidth * DateWidth, yPosition + LineHeight / 2f - titleText.Height / 2f));
            drawingContext.ResetClip();

            
            if (titleText.Width > ShrinkWidth * TitleWidth)
                drawingContext.DrawGradient(new Rect(InsideLeft + ShrinkWidth * (DateWidth + TitleWidth), yPosition, SidePadding * 2, LineHeight), new Color[] { DrawUtil.ColorAlpha(backColor, 0), DrawUtil.ColorAlpha(backColor, 1), DrawUtil.ColorAlpha(backColor, 0) });
            

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

            float viewScale = OutsideHeight / ViewHeight;

            if (viewScale >= 1)
                return;

            float scrollbarHeight = OutsideHeight * viewScale;

            drawingContext.DrawRectangle(new SolidColorBrush(Color.FromArgb(100, 0, 0, 0)), null, new Rect(OutsideRight - 8, viewScale * scrollY, 8, scrollbarHeight));
        }
        #endregion



        #region Events
        protected override void ScrollEvent(float scrollDelta)
        {
            scrollY -= scrollDelta;
            scrollY = Math.Max(0, scrollY);
            scrollY = Math.Min(ScrollMax, scrollY);

            if (ViewHeight <= OutsideHeight)
                scrollY = 0;
        }

        protected override void ClickEvent()
        {
            if (HoverIndex == -1)
                return;

            int emailIndex = GetEmailIndex(emails[HoverIndex]);
            if (emailIndex == -1)
                return;

            openedEmail = allEmails[emailIndex];
            openedEmail.IsRead = true;
            scrollY = 0;

            if (EmailReadStatusChanged != null)
                EmailReadStatusChanged.Invoke(this, new EmailReadStatusEventArgs(emailIndex, openedEmail.IsRead));

            UpdateFiltering();

            if (EmailOpenedChanged != null)
                EmailOpenedChanged.Invoke(this, new EventArgs());
        }
        #endregion



        #region Public
        public event EventHandler<EmailReadStatusEventArgs> EmailReadStatusChanged = null;
        public event EventHandler EmailOpenedChanged = null;

        public bool IsEmailOpened => openedEmail != null;

        /// <summary>
        /// Updates visible email list
        /// </summary>
        /// <param name="emailList">The array of the EmailMessages</param>
        public void UpdateEmailList(EmailMessage[] emailList)
        {
            allEmails = new EmailMessage[emailList.Length];

            for (int i=0; i < emailList.Length; i++)
            {
                allEmails[i] = emailList[i].Clone();
            }

            UpdateFiltering();
            Refresh();
        }

        /// <summary>
        /// Navigates to the next page
        /// </summary>
        public void NavigatePageNext()
        {
            Navigate(1);
        }

        /// <summary>
        /// Navigates to the previous page
        /// </summary>
        public void NavigatePagePrevious()
        {
            Navigate(-1);
        }
        public bool CanNavigateNext => currentPage < Pages - 1;
        public bool CanNavigatePrevious => currentPage > 0;
        public string PaginationText => string.Format("{0} / {1} ({2} emails)", currentPage + 1, Pages, searchedEmails.Length);

        /// <summary>
        /// Filter the email list by the sender and receiver (Sent, Received, All)
        /// </summary>
        /// <param name="filter">Sent / Received / All</param>
        public void FilterEmails(MailFilter filter)
        {
            scrollY = 0;
            openedEmail = null;
            emailFilter = filter;
            UpdateFiltering();
            Refresh();
        }

        /// <summary>
        /// Closes the currently opened email detail view
        /// </summary>
        public void CloseOpenedEmail()
        {
            FilterEmails(emailFilter);
        }

        /// <summary>
        /// Searches for a text in the message Sender, Subject, and Body fields
        /// </summary>
        /// <param name="text">The search phrase</param>
        public void SearchText(string text)
        {
            scrollY = 0;
            openedEmail = null;
            searchString = text;
            UpdateFiltering();
            Refresh();
        }
        #endregion



        #region Private & Protected
        string searchString = "";

        EmailMessage openedEmail = null;
        float openedViewHeight = 0;

        MailFilter emailFilter = MailFilter.Received;

        static int MAIL_PER_PAGE = 24;

        int currentPage = 0;
        int Pages => (int)Math.Ceiling((float)searchedEmails.Length / (float)MAIL_PER_PAGE);

        EmailMessage[] allEmails = new EmailMessage[0];

        EmailMessage[] filteredEmails = new EmailMessage[0];

        EmailMessage[] searchedEmails = new EmailMessage[0];

        EmailMessage[] emails = new EmailMessage[0];


        private float scrollY = 0;
        private float ShrinkWidth => InsideWidth - PaddingHorizonal * 2;

        protected override bool Scrollable => true;

        protected override bool HasFramerate => false;

        protected override bool DebugDraw => false;

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
        private float ViewHeight => openedEmail == null ? AllEmailsHeight : openedViewHeight;

        private float ScrollMax
        {
            get
            {
                if (emails == null)
                    return 0;

                return ViewHeight - OutsideHeight;
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

        private int GetEmailIndex(EmailMessage message)
        {
            for (int i = 0; i < allEmails.Length; i++)
            {
                if (allEmails[i].Compare(message))
                    return i;
            }
            return -1;
        }

        private void Navigate(int direction)
        {
            scrollY = 0;

            currentPage += direction;

            UpdateFiltering();
            Refresh();
        }

        private void UpdateFiltering()
        {
            List<EmailMessage> emailList = new List<EmailMessage>();

            foreach(EmailMessage message in allEmails)
            {
                if (emailFilter == MailFilter.All ||
                    (emailFilter == MailFilter.Sent && message.IsSent) ||
                    (emailFilter == MailFilter.Received && message.IsReceived))
                    emailList.Add(message);
            }

            filteredEmails = emailList.ToArray();

            UpdateSearch();
        }

        private void UpdateSearch()
        {
            List<EmailMessage> emailList = new List<EmailMessage>();

            foreach (EmailMessage message in filteredEmails)
            {
                if (searchString.Equals("") ||
                    message.Sender.ToLower().Contains(searchString.ToLower()) ||
                    message.Subject.ToLower().Contains(searchString.ToLower()) ||
                    (message.Body != null && message.Body.ToLower().Contains(searchString.ToLower())) ||
                    (message.HTMLBody != null && message.HTMLBody.ToLower().Contains(searchString.ToLower())))
                    emailList.Add(message);
            }

            searchedEmails = emailList.ToArray();

            UpdatePagination();
        }

        private void UpdatePagination()
        {
            currentPage = Math.Max(0, Math.Min(Pages - 1, currentPage));

            int startIndex = MAIL_PER_PAGE * currentPage;
            int endIndex = Math.Min(startIndex + MAIL_PER_PAGE, searchedEmails.Length);

            emails = new EmailMessage[endIndex - startIndex];

            int actualIndex = 0;
            for (int index = startIndex; index < endIndex; index++)
            {
                emails[actualIndex] = searchedEmails[searchedEmails.Length - index - 1].Clone();
                emails[actualIndex].Subject = DrawUtil.TextMaxWidth(emails[actualIndex].Subject, 20);
                emails[actualIndex].Body = DrawUtil.TextMaxHeight(emails[actualIndex].Body, 3, "[...]", 200);
                actualIndex++;
            }
        }
        #endregion
    }

    public enum MailFilter
    {
        Received = 0,
        Sent = 1,
        All = 2
    }
}
