using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_course_project
{
    public partial class Form1 : Form
    {
        public delegate void CanvasClearedEventHandler(object sender, EventArgs e);
        public delegate void DrawingCompletedEventHandler(object sender, EventArgs e);
        public delegate void ColorChangedEventHandler(object sender, ColorChangedEventArgs e);

        public event CanvasClearedEventHandler CanvasCleared;
        public event DrawingCompletedEventHandler DrawingCompleted;
        public event ColorChangedEventHandler ColorChanged;

        public Form1()  //сцена 
        {
            InitializeComponent();

            this.Width = 950;
            this.Height = 700;
            Bitmap = new Bitmap(pic.Width, pic.Height);
            Graphics = Graphics.FromImage(Bitmap);
            Graphics.Clear(Color.White);
            pic.Image = Bitmap;
        }

        public Bitmap Bitmap { get; set; }
        public Graphics Graphics { get; set; }
        public new bool Paint { get; set; }
        public Point Px { get; set; }
        public Point Py { get; set; }
        public Pen PencilPen { get; set; } = new Pen(Color.Black, 1);
        public Pen EraserPen { get; set; } = new Pen(Color.White, 10);
        public int Index { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int SX { get; set; }
        public int SY { get; set; }
        public int CX { get; set; }
        public int CY { get; set; }
        public ColorDialog ColorDialog { get; set; } = new ColorDialog();
        public Color NewColor { get; set; }


        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            Paint = true;
            Py = e.Location;

            CX = e.X;
            CY = e.Y;
        }

        private void pic_MouseMove(object sender, MouseEventArgs e)
        {
            if (Paint)
            {
                if (Index == 1)
                {
                    Px = e.Location;
                    Graphics.DrawLine(PencilPen, Px, Py);
                    Py = Px;
                }

                if (Index == 2)
                {
                    Px = e.Location;
                    Graphics.DrawLine(EraserPen, Px, Py);
                    Py = Px;
                }
            }
            pic.Refresh();

            X = e.X;
            Y = e.Y;
            SX = e.X - CX;
            SY = e.Y - CY;
        }

        private void pic_MouseUp(object sender, MouseEventArgs e)
        {
            Paint = false;

            SX = X - CX;
            SY = Y - CY;

            if (Index == 3)
            {
                Graphics.DrawEllipse(PencilPen, CX, CY, SX, SY);
            }

            if (Index == 4)
            {
                Graphics.DrawRectangle(PencilPen, CX, CY, SX, SY);
            }

            if (Index == 5)
            {
                Graphics.DrawLine(PencilPen, CX, CY, X, Y);
            }

            OnDrawingCompleted(EventArgs.Empty);
        }



        private void btn_pencil_Click(object sender, EventArgs e)
        {
            Index = 1;
        }

        private void btn_eraser_Click(object sender, EventArgs e)
        {
            Index = 2;
        }

        private void btn_ellipse_Click(object sender, EventArgs e)
        {
            Index = 3;

        }

        private void btn_rect_Click(object sender, EventArgs e)
        {
            Index = 4;
        }

        private void btn_line_Click(object sender, EventArgs e)
        {
            Index = 5;
        }

        private void pic_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (Paint)
            {
                if (Index == 3)
                {
                    g.DrawEllipse(PencilPen, CX, CY, SX, SY);
                }

                if (Index == 4)
                {
                    g.DrawRectangle(PencilPen, CX, CY, SX, SY);
                }

                if (Index == 5)
                {
                    g.DrawLine(PencilPen, CX, CY, X, Y);
                }
            }
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            Graphics.Clear(Color.White);
            pic.Image = Bitmap;
            Index = 0;

            OnCanvasCleared(EventArgs.Empty);
        }

        private void btn_color_Click(object sender, EventArgs e) //
        {
            ColorDialog.ShowDialog();
            NewColor = ColorDialog.Color;
            pic_color.BackColor = ColorDialog.Color;

            // up
            PencilPen.Color = ColorDialog.Color;

            OnColorChanged(new ColorChangedEventArgs(ColorDialog.Color));
        }

        static Point set_point(PictureBox pb, Point pt)
        {
            float pX = 1f * pb.Image.Width / pb.Width;
            float pY = 1f * pb.Image.Height / pb.Height;
            return new Point((int)(pt.X * pX), (int)(pt.Y * pY));
        }

        private void color_picker_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = set_point(color_picker, e.Location);
            pic_color.BackColor = ((Bitmap)color_picker.Image).GetPixel(point.X, point.Y);
            NewColor = pic_color.BackColor;
            PencilPen.Color = pic_color.BackColor;

            OnColorChanged(new ColorChangedEventArgs(pic_color.BackColor));
        }

        private void validate(Bitmap bm, Stack<Point> sp, int x, int y, Color old_color, Color new_color)
        {
            Color cx = bm.GetPixel(x, y);
            if (cx == old_color)
            {
                sp.Push(new Point(x, y));
                bm.SetPixel(x, y, new_color);
            }
        }

        protected void Fill(Bitmap bm, int x, int y, Color new_clr)
        {
            Color old_color = bm.GetPixel(x, y);
            Stack<Point> pixel = new Stack<Point>();
            pixel.Push(new Point(x, y));
            bm.SetPixel(x, y, new_clr);
            if (old_color == new_clr) return;

            while (pixel.Count > 0)
            {
                Point pt = (Point)pixel.Pop();
                if (pt.X > 0 && pt.Y > 0 && pt.X < bm.Width - 1 && pt.Y < bm.Height - 1)
                {
                    validate(bm, pixel, pt.X - 1, pt.Y, old_color, new_clr);
                    validate(bm, pixel, pt.X, pt.Y - 1, old_color, new_clr);
                    validate(bm, pixel, pt.X + 1, pt.Y, old_color, new_clr);
                    validate(bm, pixel, pt.X, pt.Y + 1, old_color, new_clr);
                }
            }
        }

        private void pic_MouseClick(object sender, MouseEventArgs e)
        {
            if (Index == 7)
            {
                Point point = set_point(pic, e.Location);
                Fill(Bitmap, point.X, point.Y, NewColor);
            }
        }

        private void btn_fill_Click(object sender, EventArgs e)
        {
            Index = 7;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "Image(*.jpg)|*.jpg|(*.*|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, pic.Width, pic.Height);
                Bitmap btm = Bitmap.Clone(rect, Bitmap.PixelFormat);
                btm.Save(sfd.FileName, ImageFormat.Jpeg);
                MessageBox.Show("Image Saved Successfully!");
            }
        }

        protected virtual void OnCanvasCleared(EventArgs e)
        {
            CanvasCleared?.Invoke(this, e);
        }

        protected virtual void OnDrawingCompleted(EventArgs e)
        {
            DrawingCompleted?.Invoke(this, e);
        }

        protected virtual void OnColorChanged(ColorChangedEventArgs e)
        {
            ColorChanged?.Invoke(this, e);
        }
    }

    public class ColorChangedEventArgs : EventArgs
    {
        public Color NewColor { get; }

        public ColorChangedEventArgs(Color newColor)
        {
            NewColor = newColor;
        }
    }
}