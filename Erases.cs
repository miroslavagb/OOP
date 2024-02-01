using System;
using System.Drawing;

namespace OOP_course_project
{
    public class Eraser : BaseShape
    {
        public delegate void ShapeModifiedEventHandler(object sender, EventArgs e);

        public event ShapeModifiedEventHandler ShapeModified;

        public Eraser(Pen pen, Point startPoint) : base(pen, startPoint, startPoint)
        {
        }

        public override void Draw(Graphics g)
        {
            // Implement eraser drawing logic ( draw  white line)
            Pen eraserPen = new Pen(Color.White, pen.Width); // Use a white pen for erasing
            g.DrawLine(eraserPen, startPoint, endPoint);

            OnShapeModified();
        }

        protected virtual void OnShapeModified()
        {
            ShapeModified?.Invoke(this, EventArgs.Empty);
        }
    }
}

