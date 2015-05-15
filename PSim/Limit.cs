using System;
using System.Windows;
using System.Windows.Media;
//using System.Windows.Media.
namespace PSim
{
	public class Limit : LineSegment
	{
		public double Length
		{
			get
			{
				double num = Math.Sqrt(Math.Pow(this.p1.X - this.p2.X, 2) + Math.Pow(this.p1.Y - this.p2.Y, 2));
				return num;
			}
		}
        Brush _lineColor = Brushes.Black;
        public Brush LineColor
        {
            get { return _lineColor; }
            set { _lineColor = value; }
        }
        public Limit()
        { }

		public Limit(Point p1, Point p2)
		{
			this.p1 = p1;
			this.p2 = p2;
		}

		public Limit(double x1, double y1, double x2, double y2)
		{
			this.p1 = new Point(x1, y1);
			this.p2 = new Point(x2, y2);
		}

		public Limit Scalare(double factor)
		{
			this.p1.X = this.p1.X * factor;
			this.p1.Y = this.p1.Y * factor;
			this.p2.X = this.p2.X * factor;
			this.p2.Y = this.p2.Y * factor;
            return this;
		}
        public Limit Translatare(Point p)
        {
            this.p1.X += p.X;
            this.p1.Y += p.Y;
            this.p2.X += p.X;
            this.p2.Y += p.Y;
            return this;
        }
        public Limit GetCopy()
        {
            Limit copy = new Limit(this.p1, this.p2);
            copy.LineColor = this._lineColor;
            return copy;
        }
	}
}