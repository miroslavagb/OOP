using System;
using System.Drawing;

namespace OOP_course_project
{
    public abstract class BaseShape
    {
        protected Pen pen;
        protected Point startPoint;
        protected Point endPoint;

        // Define a custom event argument class
        public class ShapeChangedEventArgs : EventArgs
        {
            public string ChangeType { get; set; } //
        }

        // protected event for shape changes
        protected event EventHandler<ShapeChangedEventArgs> ShapeChanged;

        public BaseShape(Pen pen, Point startPoint, Point endPoint)
        {
            this.pen = pen;
            this.startPoint = startPoint;
            this.endPoint = endPoint;
        }

        protected void OnShapeChanged(string changeType)
        {
            if (ShapeChanged != null)
            {
                var args = new ShapeChangedEventArgs { ChangeType = changeType };

                ShapeChanged(this, args);
            }
        }

        public abstract void Draw(Graphics g);
    }
}

