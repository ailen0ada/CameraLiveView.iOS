using System;
using UIKit;
using CoreGraphics;
namespace CameraLiveView.iOS
{
    [Foundation.Register("LiveView")]
    public class LiveView : UIView
    {
        public LiveView(IntPtr handle) : base(handle)
        {
        }

        public LiveView(CGRect frame) : base(frame)
        {
        }

        public override void Draw(CoreGraphics.CGRect rect)
        {
            base.Draw(rect);
            UIGraphics.BeginImageContext(rect.Size);
            var context = UIGraphics.GetCurrentContext();
            context.SetFillColor(0, 0, 0, 0.5f);
            context.FillRect(rect);

            this.Layer.BorderColor = UIColor.White.CGColor;
            this.Layer.BorderWidth = 2.0f;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            if (Layer != null && Layer.Sublayers != null)
                Layer.Sublayers[0].Frame = this.Bounds;
        }
    }
}
