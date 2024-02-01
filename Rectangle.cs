using System;
using System.Drawing;

namespace OOP_course_project
{
    public class RectangleShape : BaseShape
    {
        public delegate void ShapeModifiedEventHandler(object sender, EventArgs e);

        public event ShapeModifiedEventHandler ShapeModified;

        public RectangleShape(Pen pen, Point startPoint, Point empty) : base(pen, startPoint, startPoint)
        {
        }

        public override void Draw(Graphics g)
        {
            int x = Math.Min(startPoint.X, endPoint.X);
            int y = Math.Min(startPoint.Y, endPoint.Y);
            int width = Math.Abs(startPoint.X - endPoint.X);
            int height = Math.Abs(startPoint.Y - endPoint.Y);
            g.DrawRectangle(pen, x, y, width, height);

            OnShapeModified();
        }

        protected virtual void OnShapeModified()
        {
            ShapeModified?.Invoke(this, EventArgs.Empty);
        }
    }
}
