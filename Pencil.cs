using System;
using System.Drawing;

namespace OOP_course_project
{
    public class Pencil : BaseShape
    {
        public delegate void ShapeModifiedEventHandler(object sender, EventArgs e);

        public event ShapeModifiedEventHandler ShapeModified;

        public Pencil(Pen pen, Point startPoint) : base(pen, startPoint, startPoint)
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

