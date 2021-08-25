using System;
using System.Windows;
using System.Windows.Media;
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
        #endregion



        #region Rendering
        protected override void Render(DrawingContext drawingContext)
        {
            if (emails == null)
                emails = TestEmailData();

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
        void RenderEmailListItem(DrawingContext drawingContext, EmailData email, int index)
        {
            float yPosition = -scrollY + index * ItemHeight;
            Color backColor = ItemBackground(index);
            Color foreColor = ItemForeground(index);

            if (yPosition > OutsideHeight || yPosition + LineHeight < 0)
                return;

            drawingContext.DrawRectangle(new SolidColorBrush(backColor), null, new Rect(new Point(0, yPosition), new Size(OutsideWidth, LineHeight)));

            drawingContext.ClipRectangle(InsideLeft, yPosition, ShrinkWidth * DateWidth, LineHeight);
            FormattedText emailText = DrawUtil.FormatText(email.From, new SolidColorBrush(foreColor), 12, true);
            FormattedText dateText = DrawUtil.FormatText(email.DateTime.ToString("yyyy.MM.dd HH:mm"), new SolidColorBrush(foreColor), 12);
            drawingContext.DrawText(emailText, new Point(InsideLeft, yPosition + LineHeight / 2 - emailText.Height));
            drawingContext.DrawText(dateText, new Point(InsideLeft, yPosition + LineHeight / 2));
            drawingContext.ResetClip();

            drawingContext.ClipRectangle(InsideLeft + SidePadding + ShrinkWidth * DateWidth, yPosition, ShrinkWidth * TitleWidth, LineHeight);
            FormattedText titleText = DrawUtil.FormatText(email.Title, new SolidColorBrush(foreColor), 14, true);
            drawingContext.DrawText(titleText, new Point(InsideLeft + SidePadding + ShrinkWidth * DateWidth, yPosition + LineHeight / 2f - titleText.Height / 2f));
            drawingContext.ResetClip();

            if (titleText.Width > ShrinkWidth * TitleWidth)
                drawingContext.DrawGradient(new Rect(InsideLeft + ShrinkWidth * (DateWidth + TitleWidth), yPosition, SidePadding * 2, LineHeight), new Color[] { DrawUtil.ColorAlpha(backColor, 0), DrawUtil.ColorAlpha(backColor, 1), DrawUtil.ColorAlpha(backColor, 0) });

            FormattedText bodyText = DrawUtil.FormatText(email.Body, new SolidColorBrush(foreColor), 12);
            drawingContext.DrawText(bodyText, new Point(InsideLeft + SidePadding * 2 + ShrinkWidth * (DateWidth + TitleWidth), bodyText.Height <= LineHeight ? yPosition + LineHeight / 2 - bodyText.Height / 2 : 0));
        }

        /// <summary>
        /// Renders a faded small "scrollbar" rectangle on the right side of the component
        /// </summary>
        /// <param name="drawingContext">The DrawingContext instance</param>
        void RenderScrollbar(DrawingContext drawingContext)
        {
            if (!mouseInside)
                return;

            float viewScale = OutsideHeight / (ScrollMax + OutsideHeight);

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
        }
        #endregion



        #region Private & Protected
        private float scrollY = 0;
        private float ShrinkWidth => InsideWidth - PaddingHorizonal * 2;

        protected override float PaddingHorizonal => SidePadding;

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

        private float ScrollMax
        {
            get
            {
                if (emails == null)
                    return 0;

                return emails.Length * ItemHeight - InsideHeight;
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



        EmailData[] emails = null;

        static EmailData[] TestEmailData()
        {
            Random random = new Random();
            int emailCount = random.Next(30, 70);
            EmailData[] emails = new EmailData[emailCount];

            for(int emailIndex=0; emailIndex <emails.Length; emailIndex++)
            {
                int randomSentenceIndex = random.Next(0, EmailData.Sentences.Length);
                string randomSentence = EmailData.Sentences[randomSentenceIndex];

                int randomEmailIndex = random.Next(0, EmailData.Emails.Length);
                string randomEmail = EmailData.Emails[randomEmailIndex];

                emails[emailIndex] = new EmailData();
                emails[emailIndex].Title = randomSentence.Substring(0, Math.Min(30, randomSentence.Length));
                emails[emailIndex].Body = randomSentence;
                emails[emailIndex].From = randomEmail;
                emails[emailIndex].DateTime = new DateTime(random.Next(1999, 2021), random.Next(1, 12), random.Next(1, 29), random.Next(0, 23), random.Next(0, 59), random.Next(0, 59));
            }

            return emails;
        }
    }

    struct EmailData
    {
        public string Title;
        public string Body;
        public DateTime DateTime;
        public string From;

        public static string[] Sentences
        {
            get
            {
                return new string[]
                {
                    "She was sad to hear that fireflies are facing extinction due to artificial light, habitat loss, and pesticides.",
                    "The overpass went under the highway and into a secret world.",
                    "The sudden rainstorm washed crocodiles into the ocean.",
                    "Nobody has encountered an explosive daisy and lived to tell the tale.",
                    "His mind was blown that there was nothing in space except space itself.",
                    "You bite up because of your lower jaw.",
                    "Henry couldn't decide if he was an auto mechanic or a priest.",
                    "She was the type of girl who wanted to live in a pink house.",
                    "We have young kids who often walk into our room at night for various reasons including clowns in the closet.",
                    "He wore the surgical mask in public not to keep from catching a virus, but to keep people away from him.",
                    "While all her friends were positive that Mary had a sixth sense, she knew she actually had a seventh sense.",
                    "Happiness can be found in the depths of chocolate pudding.",
                    "When nobody is around, the trees gossip about the people who have walked under them.",
                    "It's not often you find a soggy banana on the street.",
                    "He realized there had been several deaths on this road, but his concern rose when he saw the exact number.",
                    "I would have gotten the promotion, but my attendance wasn’t good enough.",
                    "The beach was crowded with snow leopards.",
                    "Behind the window was a reflection that only instilled fear.",
                    "Boulders lined the side of the road foretelling what could come next.",
                    "Lightning Paradise was the local hangout joint where the group usually ended up spending the night.",
                    "Italy is my favorite country; in fact, I plan to spend two weeks there next year.",
                    "I love bacon, beer, birds, and baboons.",
                    "He learned the hardest lesson of his life and had the scars, both physical and mental, to prove it.",
                    "Tuesdays are free if you bring a gnome costume.",
                    "It was always dangerous to drive with him since he insisted the safety cones were a slalom course.",
                    "Dan ate the clouds like cotton candy.",
                    "The two walked down the slot canyon oblivious to the sound of thunder in the distance.",
                    "When confronted with a rotary dial phone the teenager was perplexed.",
                    "It was at that moment that he learned there are certain parts of the body that you should never Nair.",
                    "He played the game as if his life depended on it and the truth was that it did.",
                    "She cried diamonds.",
                    "It had been sixteen days since the zombies first attacked.",
                    "As the years pass by, we all know owners look more and more like their dogs.",
                    "When I was little I had a car door slammed shut on my hand and I still remember it quite vividly.",
                    "A song can make or ruin a person’s day if they let it get to them.",
                    "I purchased a baby clown from the Russian terrorist black market.",
                    "He barked orders at his daughters but they just stared back with amusement.",
                    "I'd rather be a bird than a fish.",
                    "For the 216th time, he said he would quit drinking soda after this last Coke.",
                    "The family’s excitement over going to Disneyland was crazier than she anticipated.",
                    "One small action would change her life, but whether it would be for better or for worse was yet to be determined.",
                    "The thunderous roar of the jet overhead confirmed her worst fears.",
                    "The skeleton had skeletons of his own in the closet.",
                    "The irony of the situation wasn't lost on anyone in the room.",
                    "He had unknowingly taken up sleepwalking as a nighttime hobby.",
                    "There's an art to getting your way, and spitting olive pits across the table isn't it.",
                    "I just wanted to tell you I could see the love you have for your child by the way you look at her.",
                    "After fighting off the alligator, Brian still had to face the anaconda.",
                    "We will not allow you to bring your pet armadillo along.",
                    "I am counting my calories, yet I really want dessert."
                };
            }
        }
        public static string[] Emails
        {
            get
            {
                return new string[]
                {
                    "mschwartz@att.net",
                    "arnold@aol.com",
                    "sagal@comcast.net",
                    "scotfl@comcast.net",
                    "flakeg@sbcglobal.net",
                    "msusa@yahoo.com",
                    "lipeng@gmail.com",
                    "chaki@comcast.net",
                    "petersen@att.net",
                    "starstuff@mac.com",
                    "afeldspar@verizon.net",
                    "agolomsh@live.com",
                    "malin@yahoo.com",
                    "alfred@aol.com",
                    "animats@yahoo.com",
                    "manuals@verizon.net",
                    "ehood@msn.com",
                    "carmena@yahoo.ca",
                    "houle@icloud.com",
                    "leocharre@aol.com",
                    "rfisher@yahoo.ca",
                    "stevelim@att.net",
                    "goresky@live.com",
                    "scato@msn.com",
                    "mwilson@msn.com",
                    "floxy@sbcglobal.net",
                    "kalpol@sbcglobal.net",
                    "mkearl@sbcglobal.net",
                    "sassen@msn.com",
                    "petersen@verizon.net",
                    "parkes@verizon.net",
                    "ournews@yahoo.com",
                    "mhoffman@msn.com",
                    "forsberg@optonline.net",
                    "sravani@gmail.com",
                    "wildixon@sbcglobal.net",
                    "kenja@sbcglobal.net",
                    "ninenine@icloud.com",
                    "gordonjcp@comcast.net",
                    "tfinniga@hotmail.com",
                    "choset@mac.com",
                    "adamk@verizon.net",
                    "amcuri@yahoo.ca",
                    "pkilab@optonline.net",
                    "citadel@msn.com",
                    "dbanarse@sbcglobal.net",
                    "suresh@hotmail.com",
                    "oevans@live.com",
                    "stinson@yahoo.com",
                    "cyrus@mac.com"
                };
            }
        }
    }
}
