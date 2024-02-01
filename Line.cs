using System;
using System.Drawing;

namespace OOP_course_project
{
    public class Line : BaseShape
    {
        public delegate void ShapeModifiedEventHandler(object sender, EventArgs e);

        public event ShapeModifiedEventHandler ShapeModified;

        public Line(Pen pen, Point startPoint, Point endPoint) : base(pen, startPoint, endPoint)
        {
        }

        public override void Draw(Graphics g)
        {
            g.DrawLine(pen, startPoint, endPoint);

            OnShapeModified();
        }

        protected virtual void OnShapeModified()
        {
            ShapeModified?.Invoke(this, EventArgs.Empty);
        }
    }
}

