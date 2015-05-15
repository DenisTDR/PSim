using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using Point = System.Windows.Point;

namespace TDR_Graphics
{
    public class TPanel : Panel
    {
        public TGraphic TGraphic;
        public bool AutoScrollLeft { get; set; }
        public TPanel()
        {
            this.Controls.Add(TGraphic = new TGraphic());
            this.SizeChanged += TPanel_SizeChanged;
            this.AutoScroll = true;
            this.AutoScrollLeft = true;
        }

        void TPanel_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                if (TGraphic.Width > this.Width)
                    TGraphic.Height = this.Height - 25;
                else
                    TGraphic.Height = this.Height - 5;
                TGraphic.Top = 0;
                TGraphic.Left = 0;
                TGraphic.Refresh();
            }
            catch { }
        }
        Point lastPoint;
        int lastW = 0;

        public void AddTPoint( double y)
        {
            if (lastPoint.X>=0)
                TGraphic.Lines.Add(new GraphicLine()
                {
                    P1 = lastPoint,
                    P2 = new Point(lastW, y),
                    Width = 3,
                    Color = Color.Green
                });
            TGraphic.Points.Add(new GraphicPoint()
            {
                Point = lastPoint = new Point(lastW, y),
                Size = 7,
                FillColor = Color.Red,
                BorderWidth = 2F,
                BorderColor = Color.Blue
            });
            TGraphic.Redraw();
            if (this.AutoScrollLeft)
            {
                this.HorizontalScroll.Value = this.HorizontalScroll.Maximum;
                this.HorizontalScroll.Value = this.HorizontalScroll.Maximum;
            }
            lastW += 20;
        }
        public void Clear()
        {
            TGraphic.Width = 50;
            lastW = 0;
            lastPoint.X = -1;
            TGraphic.Lines.Clear();
            TGraphic.Points.Clear();
            TGraphic.Redraw();
            TGraphic.Width = 50;
        }
    }
    public class TGraphic : Panel
    {
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<GraphicPoint> Points { get; set; }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<GraphicLine> Lines { get; set; }
        public Color AxesColor { get; set; }
        public float AxesWidth { get; set; }
        public PointF AxesOffset { get; set; }
        bool DockInParentHeight { get; set; }
        public TGraphic()
        {
            this.BackColor = Color.White;
            this.DoubleBuffered = true;
            Points = new List<GraphicPoint>();
            Lines = new List<GraphicLine>();
            this.Paint += TPanel_Paint;
            AxesColor = Color.Gray;
            AxesWidth = 3;
            AxesOffset = new PointF(25, 25);
            DockInParentHeight = false;
            this.Redraw();
        }

        int minWidth = 0;
        void TPanel_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                minWidth = 0;
                Graphics g = e.Graphics;
                g.Clear(BackColor);


                g.DrawLine(new Pen(AxesColor, AxesWidth), new PointF(AxesOffset.X, this.Height), new PointF(AxesOffset.X, 10));
                g.DrawLine(new Pen(AxesColor, AxesWidth), new PointF(AxesOffset.X, 10), new PointF(AxesOffset.X + 15 * (float)Math.Sin(Math.PI / 6),
                                                                                  10 + 15 * (float)Math.Cos(Math.PI / 6)));
                g.DrawLine(new Pen(AxesColor, AxesWidth), new PointF(AxesOffset.X, 10), new PointF(AxesOffset.X - 15 * (float)Math.Sin(Math.PI / 6),
                                                                                  10 + 15 * (float)Math.Cos(Math.PI / 6)));

                g.DrawLine(new Pen(AxesColor, AxesWidth), new PointF(0, this.Height - AxesOffset.Y), new PointF(this.Width - 10, this.Height - AxesOffset.Y));

                g.DrawLine(new Pen(AxesColor, AxesWidth), new PointF(this.Width - 10, this.Height - AxesOffset.Y),
                                                          new PointF(this.Width - 10 - 15 * (float)Math.Sin(Math.PI / 3),
                                                                     this.Height - AxesOffset.Y + 15 * (float)Math.Cos(Math.PI / 3)));

                g.DrawLine(new Pen(AxesColor, AxesWidth), new PointF(this.Width - 10, this.Height - AxesOffset.Y),
                                                          new PointF(this.Width - 10 - 15 * (float)Math.Sin(Math.PI / 3),
                                                                     this.Height - AxesOffset.Y - 15 * (float)Math.Cos(Math.PI / 3)));

                foreach (GraphicLine gl in Lines)
                {
                    g.DrawLine(new Pen(gl.Color, gl.Width), new PointF((float)gl.P1.X + AxesOffset.X, this.Height - (float)gl.P1.Y - AxesOffset.Y), new PointF((float)gl.P2.X + AxesOffset.X, this.Height - (float)gl.P2.Y - AxesOffset.Y));
                    minWidth = Math.Max(minWidth, (int)gl.P1.X + 1);
                    minWidth = Math.Max(minWidth, (int)gl.P2.X + 1);
                }
                foreach (GraphicPoint gp in Points)
                {
                    g.FillEllipse(new SolidBrush(gp.FillColor), new RectangleF((float)(gp.Point.X + AxesOffset.X - gp.Size / 2),
                        (float)(this.Height - gp.Point.Y - AxesOffset.Y - gp.Size / 2), gp.Size, gp.Size));
                    g.DrawEllipse(new Pen(gp.BorderColor, gp.BorderWidth), new RectangleF((float)(gp.Point.X + AxesOffset.X - gp.Size / 2),
                        (float)(this.Height - gp.Point.Y - AxesOffset.Y - gp.Size / 2), gp.Size, gp.Size));

                    minWidth = Math.Max(minWidth, (int)gp.Point.X + 1);
                }
                minWidth += (int)AxesOffset.X + 15;
                if (this.Width < minWidth)
                    this.Width = minWidth;
            }
            catch  { }
        }
        public void Redraw()
        {
            this.Invalidate();
        }

    }
    [Serializable]
    public class GraphicPoint
    {
        public Point Point { get; set; }
        public Color FillColor { get; set; }
        public Color BorderColor { get; set; }
        public int Size { get; set; }
        public float BorderWidth { get; set; }
        public GraphicPoint()
        {
            Point = new Point(50, 100);
            FillColor = BorderColor = Color.Black;
            Size = 5;
            BorderWidth = 0;
        }
    }
    [Serializable]
    public class GraphicLine
    {
        public Point P1 { get; set; }
        public Point P2 { get; set; }
        public int Width { get; set; }
        public Color Color { get; set; }
        public GraphicLine()
        {
            P1 = new Point(50, 50);
            P2 = new Point(100, 150);
            Width = 2;
            Color = Color.Black;
        }
    }
    public static class extensions
    {
        public static System.Drawing.PointF ToPointF(this Point p)
        {
            return new System.Drawing.PointF((float)p.X, (float)p.Y);
        }
    }


}
