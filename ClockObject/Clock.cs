using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClockObject
{
    class Clock
    {
        int second = 0;
        int minute = 10;
        int hour = 0;

        DateTime time;

        //
        //Private Variables
        //
        private int w;
        private int h;
        private int x;
        private int y;
        private int size;
        private Graphics g;
        private Bitmap bmp;
        private Rectangle rect;
        private Rectangle circle;
        private Rectangle cover;
        private Point center;
        private PictureBox pb;
        //
        //Pens
        //
        private static Pen blackPen = new Pen(Color.FromArgb(255, 15, 15, 15), 3.8f);
        private static Pen markPen = new Pen(Color.FromArgb(215, 215, 0, 0), 2f);
        private static Pen markPen2 = new Pen(Color.FromArgb(255, 30, 30, 30), 2f);
        private static Pen markPen3 = new Pen(Color.FromArgb(255, 5, 5, 5), 2f);
        private static Pen fineLiner = new Pen(Color.FromArgb(255, 25, 25, 25), 2f);
        //
        //Brushes
        //
        private static SolidBrush Blue = new SolidBrush(Color.FromArgb(180, 64, 64, 255));
        private static SolidBrush Beige = new SolidBrush(Color.Beige);
        private static SolidBrush Black = new SolidBrush(Color.FromArgb(180, 25, 220, 25));
        private static SolidBrush Gray = new SolidBrush(Color.FromArgb(150, 51,51, 255));
        //
        private Color backGround = Color.FromArgb(255, 192, 192, 192);
        //
        private void tmr_Tick(object sender, EventArgs e)
        {
            //ticking
            //second++;
            //second %= 60;
            
            time = DateTime.Now;
            second = time.Second;
            minute = time.Minute;
            hour = time.Hour;
            //
            g = Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            DrawCover();
            DrawHour();
            DrawMinute();
            DrawSecond();
            DrawDot();
            pb.Image = bmp;
            g.Dispose();
        }
        //
        public Clock(PictureBox pb)
        {
            time = DateTime.Now;
            second = time.Second;
            minute = time.Minute;
            hour = time.Hour;
            //Setting up timer
            Timer tmr = new Timer();
            tmr.Interval = 250;
            tmr.Tick += new EventHandler(tmr_Tick);
            tmr.Start();
            //
            this.pb = pb;
            CalculateSquare();
            bmp = new Bitmap(w + 1, h + 1);
            g = Graphics.FromImage(bmp);
            g.Clear(backGround);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //
            //Draw
            //
            g.FillRectangle(Blue, rect);
            DrawCircle();
            DrawBorder();
            DrawOutLine();
            //
            //DrawOnce
            //
            DrawHour();
            DrawMinute();
            DrawSecond();
            DrawDot();
            //
            //End of Draw
            //
            pb.Image = bmp;
            g.Dispose();
        }
        private void DrawCircle()
        {
            g.FillEllipse(Beige, circle);
        }
        //
        private void DrawBorder()
        {
            g.DrawEllipse(blackPen, circle);
        }
        //
        private void DrawOutLine()
        {
            int r = (int)(size * 0.44f);
            int rDot = (int)(size * 0.008f);
            int sigma = (int)(size * 0.008f);
            for (int i = 0; i < 360; i += 30)
            {
                Point n = EndPoint(center, i, r);
                g.DrawEllipse(markPen, n.X - rDot, n.Y - rDot, 2 * rDot, 2 * rDot);
                for (int j = i + 6; j < i + 30; j += 6)
                {

                    Point l1 = EndPoint(center, j, r - (2 * sigma));
                    Point l2 = EndPoint(center, j, r + sigma);
                    g.DrawLine(markPen2, l1, l2); //pen 2

                }

            }
        }
        //
        private void DrawDot()
        {
            int r = (int)(size * 0.009f);
            g.DrawEllipse(markPen3, center.X - r, center.Y - r, 2 * r, 2 * r);
        }
        //
        private void DrawSecond()
        {
            int r = (int)(size * 0.4f);
            int Angle = 90 - (second * 6);
            Point s = EndPoint(center, Angle, r);
            Point d = EndPoint(center, 180 + Angle, (int)(size * 0.088f));
            Point c = EndPoint(center, 180 + Angle, (int)(size * 0.1f));
            Point l = EndPoint(center, Angle, r + (int)(size * 0.15f));
            //
            g.DrawLine(markPen, s, center);
            g.DrawLine(markPen, d, center);
            g.DrawEllipse(markPen, c.X - (int)(size * 0.015f), c.Y - (int)(size * 0.015f), (int)(size * 0.03f), (int)(size * 0.03f));
        }
        //
        private void DrawMinute()
        {
            int Angle = 90 - (minute * 6);
            Angle -= (int)((second / (double)60.0) * 6);
            Point p1 = EndPoint(center, Angle, (int)(size * 0.37f));
            Point p2 = EndPoint(center, Angle + 17, (int)(size * 0.06f));
            Point p3 = EndPoint(center, Angle - 17, (int)(size * 0.06f));
            Point p4 = EndPoint(center, Angle + 180, (int)(size * 0.12f));
            Point p5 = EndPoint(center, Angle + 197, (int)(size * 0.06f));
            Point p6 = EndPoint(center, Angle + 163, (int)(size * 0.06f));
            //
            g.DrawPolygon(fineLiner, new Point[] { center, p3, p1, p2 });
            g.DrawPolygon(fineLiner, new Point[] { center, p6, p4, p5 });
            //
            g.FillPolygon(Gray, new Point[] { center, p3, p1, p2 });
            g.FillPolygon(Gray, new Point[] { center, p6, p4, p5 });
        }
        //
        private void DrawHour()
        {
            int Angle = 90 - (hour * 30);
            Angle -= (int)((minute / (double)60.0) * 30);
            //
            Point p1 = EndPoint(center, Angle, (int)(size * 0.28f));
            Point p2 = EndPoint(center, Angle + 23, (int)(size * 0.06f));
            Point p3 = EndPoint(center, Angle - 23, (int)(size * 0.06f));
            Point p4 = EndPoint(center, Angle + 180, (int)(size * 0.13f));
            Point p5 = EndPoint(center, Angle + 203, (int)(size * 0.06f));
            Point p6 = EndPoint(center, Angle + 157, (int)(size * 0.06f));
            //
            g.DrawPolygon(fineLiner, new Point[] { center, p3, p1, p2 });
            g.DrawPolygon(fineLiner, new Point[] { center, p6, p4, p5 });
            //
            g.FillPolygon(Black, new Point[] { center, p3, p1, p2 });
            g.FillPolygon(Black, new Point[] { center, p6, p4, p5 });
        }
        //
        private void DrawCover()
        {
            g.FillEllipse(Beige, cover);
        }
        //
        private void CalculateSquare()
        {
            pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;

            //w = pb.Width;
            //h = pb.Height;
            w = 1200;//static size.
            h = 1200;//static size.
            x = 0;
            y = 0;
            size = w;
            if (w > h)
            {
                y = 0;
                x = (w - h) / 2;
                size = h;
            }
            else if (w < h)
            {
                x = 0;
                y = (h - w) / 2;
                size = w;
            }
            rect = new Rectangle(x, y, size, size);
            //
            //
            int left = (int)(rect.X + rect.Width * 0.025f);
            int top = (int)(rect.Y + rect.Height * 0.025f);
            int wid = (int)(rect.Width * 0.95f);
            int heig = (int)(rect.Height * 0.95f);
            circle = new Rectangle(left, top, wid, heig);
            //
            int cl = (int)(circle.X + circle.Width * 0.06f);
            int ct = (int)(circle.Y + circle.Height * 0.06f);
            int cw = (int)(circle.Width * 0.88f);
            int ch = (int)(circle.Height * 0.88f);
            cover = new Rectangle(cl, ct, cw, ch);
            //
            center = new Point((int)(w / 2), (int)(h / 2));
            //
            blackPen.Width = size * 0.015f;
            markPen.Width = size * 0.006f;
            markPen2.Width = size * 0.007f;
            markPen3.Width = size * 0.0145f;
            fineLiner.Width = size * 0.007f;
        }
        //
        private static Point EndPoint(Point o, int angle, int length)
        {
            var endPoint = o;
            endPoint.X += (int)(length * Math.Cos(Radian(angle)));
            endPoint.Y -= (int)(length * Math.Sin(Radian(angle)));
            return endPoint;
        }
        //
        private static double Radian(double angle)
        {
            return (Math.PI / 180.0) * angle;
        }
        //
        private static double Angle(double Radian)
        {
            return Radian * (180 / Math.PI);
        }
    }
}