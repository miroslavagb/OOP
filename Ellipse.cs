using System;
using System.Drawing;

namespace OOP_course_project
{
    public class Ellipse : BaseShape
    {
        public delegate void ShapeModifiedEventHandler(object sender, EventArgs e);

        public event ShapeModifiedEventHandler ShapeModified;

        public Ellipse(Pen pen, Point startPoint, Point endPoint) : base(pen, startPoint, endPoint)
        {
        }

        public override void Draw(Graphics g)
        {
            // Draw an ellipse
            int x = Math.Min(startPoint.X, endPoint.X);
            int y = Math.Min(startPoint.Y, endPoint.Y);
            int width = Math.Abs(startPoint.X - endPoint.X);
            int height = Math.Abs(startPoint.Y - endPoint.Y);
            g.DrawEllipse(pen, x, y, width, height);

            OnShapeModified();
        }

        protected virtual void OnShapeModified()
        {
            ShapeModified?.Invoke(this, EventArgs.Empty);
        }
    }
}
