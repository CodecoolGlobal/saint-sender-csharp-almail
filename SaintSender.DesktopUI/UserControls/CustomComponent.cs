using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using SaintSender.DesktopUI.Utility;

namespace SaintSender.DesktopUI.UserControls
{
    abstract class CustomComponent : FrameworkElement
    {
        protected Point mousePosition { get; private set; } = new Point(0, 0);
        protected bool mouseInside { get; private set; } = false;
        protected bool mouseLeftButton { get; private set; } = false;
        protected bool mouseRightButton { get; private set; } = false;

        protected virtual float PaddingHorizonal => 0;
        protected virtual float PaddingVertical => 0;

        protected virtual bool RestrictedRendering => true;
        protected virtual bool Scrollable => false;

        protected virtual bool HasFramerate => false;
        protected virtual int Framerate => 50;

        private int FrameInterval => (int)Math.Round(1f / (float)Framerate * 1000f);

        protected virtual bool DebugDraw => false;
        private int debugDrawIndex = 0;

        protected float InsideLeft { get; private set; } = 0;
        protected float InsideRight { get; private set; } = 0;
        protected float InsideTop { get; private set; } = 0;
        protected float InsideBottom { get; private set; } = 0;
        protected float InsideWidth { get; private set; } = 0;
        protected float InsideHeight { get; private set; } = 0;
        protected float OutsideLeft { get; private set; } = 0;
        protected float OutsideRight { get; private set; } = 0;
        protected float OutsideTop { get; private set; } = 0;
        protected float OutsideBottom { get; private set; } = 0;
        protected float OutsideWidth { get; private set; } = 0;
        protected float OutsideHeight { get; private set; } = 0;

        protected Rect InsideRect => new Rect(InsideLeft, InsideTop, InsideWidth, InsideHeight);
        protected Rect OutsideRect => new Rect(OutsideLeft, OutsideTop, OutsideWidth, OutsideHeight);

        private System.Windows.Forms.Timer updateTimer = null;

        protected override void OnRender(DrawingContext drawingContext)
        {
            InsideLeft = PaddingHorizonal;
            InsideRight = (float)RenderSize.Width - PaddingHorizonal;
            InsideTop = PaddingVertical;
            InsideBottom = (float)RenderSize.Height - PaddingVertical;
            InsideWidth = InsideRight - InsideLeft;
            InsideHeight = InsideBottom - InsideTop;

            OutsideLeft = OutsideTop = 0;
            OutsideRight = OutsideWidth = (float)RenderSize.Width;
            OutsideBottom = OutsideHeight = (float)RenderSize.Height;

            // Mouse scroll fix
            if (Scrollable)
                drawingContext.DrawRectangle(new SolidColorBrush(Color.FromArgb(1, 0, 0, 0)), null, OutsideRect);

            if (RestrictedRendering)
                drawingContext.ClipRectangle(OutsideRect);

            Render(drawingContext);

            if (RestrictedRendering)
                drawingContext.ResetClip();

            if (DebugDraw)
            {
                debugDrawIndex++;
                if (debugDrawIndex > 20)
                    debugDrawIndex = 0;

                for (int i=0; i <debugDrawIndex; i++)
                {
                    drawingContext.DrawRectangle(Brushes.Red, null, new Rect(10 + 20 * i, 10, 10, 10));
                }
            }
        }

        protected abstract void Render(DrawingContext drawingContext);

        protected virtual void ScrollEvent(float scrollDelta) { /* cirip cirip */ }

        protected virtual void ClickEvent() { /* cirip cirip */ }

        private bool IsMouseInBounds(MouseEventArgs e)
        {
            return OutsideRect.Contains(e.GetPosition(this));
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            mouseInside = true;

            if (e.LeftButton == MouseButtonState.Pressed)
                mouseLeftButton = true;
            if (e.RightButton == MouseButtonState.Pressed)
                mouseRightButton = true;

            Refresh();
        }
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            mouseInside = false;
            mouseLeftButton = false;
            mouseRightButton = false;

            Refresh();
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (this.IsMouseInBounds(e))
                OnMouseEnter(e);
            else
                OnMouseLeave(e);

            mousePosition = e.GetPosition(this);

            Refresh();
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            mouseLeftButton = true;
            Refresh();
        }
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (mouseInside && mouseLeftButton)
                ClickEvent();

            base.OnMouseRightButtonDown(e);
            mouseLeftButton = false;
            Refresh();
        }
        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseRightButtonDown(e);
            mouseRightButton = true;
            Refresh();
        }
        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseRightButtonUp(e);
            mouseRightButton = false;
            Refresh();
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            if (e.Delta != 0)
            {
                ScrollEvent(e.Delta);
                Refresh();
            }
        }

        public void Refresh()
        {
            if (!HasFramerate)
                InvalidateVisual();
            else
            {
                if (updateTimer == null)
                {
                    updateTimer = new System.Windows.Forms.Timer();
                    updateTimer.Interval = FrameInterval;
                    updateTimer.Tick += UpdateTimer_Tick;
                }
                else if (updateTimer.Interval != FrameInterval)
                {
                    updateTimer.Stop();
                    updateTimer.Interval = FrameInterval;
                    updateTimer.Start();
                }

                if (!updateTimer.Enabled)
                    updateTimer.Start();
            }
        }

        private void UpdateTimer_Tick(object sender, System.EventArgs e)
        {
            InvalidateVisual();
            updateTimer.Start();
        }
    }
}
