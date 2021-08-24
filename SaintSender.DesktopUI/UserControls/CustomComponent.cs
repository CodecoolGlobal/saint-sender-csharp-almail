using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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

        protected float ViewLeft { get; private set; } = 0;
        protected float ViewRight { get; private set; } = 0;
        protected float ViewTop { get; private set; } = 0;
        protected float ViewBottom { get; private set; } = 0;
        protected float ViewWidth { get; private set; } = 0;
        protected float ViewHeight { get; private set; } = 0;

        protected override void OnRender(DrawingContext drawingContext)
        {
            ViewLeft = PaddingHorizonal;
            ViewRight = (float)RenderSize.Width - PaddingHorizonal;
            ViewTop = PaddingVertical;
            ViewBottom = (float)RenderSize.Height - PaddingVertical;
            ViewWidth = ViewRight - ViewLeft;
            ViewHeight = ViewBottom - ViewTop;

            Render(drawingContext);
        }

        protected abstract void Render(DrawingContext drawingContext);

        private bool IsMouseInBounds(MouseEventArgs e)
        {
            Rect bounds = new Rect(0, 0, this.ActualWidth, this.ActualHeight);
            return bounds.Contains(e.GetPosition(this));
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            mouseInside = true;

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
        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonDown(e);
            mouseLeftButton = false;
            Refresh();
        }
        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseRightButtonDown(e);
            mouseRightButton = true;
            Refresh();
        }
        protected override void OnPreviewMouseRightButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseRightButtonUp(e);
            mouseRightButton = false;
            Refresh();
        }

        protected void Refresh()
        {
            InvalidateVisual();
            //UpdateLayout();
        }
    }
}
