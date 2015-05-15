using System;
using System.Windows;

namespace PSim
{
	public class LineSegment
	{
		protected Point p1;

		protected Point p2;

		public Point lineEquation
		{
			get
			{
                Point point = new Point();
                point.X = this.Slope();
                point.Y = this.p1.Y - point.X * this.p1.X;
                return point;
			}
		}

		public Point P1
		{
			get
			{
				return this.p1;
			}
			set
			{
				this.p1 = value;
			}
		}

		public Point P2
		{
			get
			{
				return this.p2;
			}
			set
			{
				this.p2 = value;
			}
		}

		public LineSegment()
		{
		}

		public LineSegment(Point p1, Point p2)
		{
			this.p1 = p1;
			this.p2 = p2;
		}

		public LineSegment(double x1, double y1, double x2, double y2)
		{
			this.p1 = new Point(x1, y1);
			this.p2 = new Point(x2, y2);
		}

		public double Diff()
		{
			double y = this.p1.Y - this.Slope() * this.p1.X;
			return y;
		}

		public Point IntersectionWith(LineSegment ls)
		{
			bool flag;
			Point y = new Point();
			Point point = this.lineEquation;
			Point point1 = ls.lineEquation;
			y.X = (point.Y - point1.Y) / (point1.X - point.X);
			y.Y = point.X * y.X + point.Y;
			if (Math.Min(this.p1.X, this.p2.X) < y.X && y.X < Math.Max(this.p1.X, this.p2.X) && Math.Min(ls.P1.X, ls.P2.X) < y.X && y.X < Math.Max(ls.P1.X, ls.P2.X))
			{
				flag = false;
			}
			else if (Math.Min(this.p1.Y, this.p2.Y) >= y.Y || y.Y >= Math.Max(this.p1.Y, this.p2.Y) || Math.Min(ls.P1.Y, ls.P2.Y) >= y.Y)
			{
				flag = true;
			}
			else
			{
				double num = y.Y;
				double y1 = ls.P1.Y;
				Point p2 = ls.P2;
				flag = num >= Math.Max(y1, p2.Y);
			}
			return (flag ? new Point(-1, -1) : y);
		}

		public double Slope()
		{
			double num;
			num = (this.p1.X != this.p2.X ? (this.p1.Y - this.p2.Y) / (this.p1.X - this.p2.X) : 100);
			return num;
		}

		public override string ToString()
		{
			string[] str = new string[] { "{", this.p1.ToString(), " -> ", this.p2.ToString(), "}" };
			return string.Concat(str);
		}

		public string ToStringR()
		{
			string[] str = new string[] { "{(", null, null, null, null, null, null, null, null };
			double num = Math.Round(this.p1.X);
			str[1] = num.ToString();
			str[2] = ", ";
			num = Math.Round(this.p1.Y);
			str[3] = num.ToString();
			str[4] = ") -> (";
			num = Math.Round(this.p2.X);
			str[5] = num.ToString();
			str[6] = ", ";
			num = Math.Round(this.p2.Y);
			str[7] = num.ToString();
			str[8] = ")}";
			return string.Concat(str);
		}
	}
}