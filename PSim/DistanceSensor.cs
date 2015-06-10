using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PSim
{
	public class DistanceSensor
	{
		private double angle;

		private double angleDiff;

		private bool stChanged;

		private Ellipse Start;

		private Ellipse Stop;

		private Point hit;

		public double Angle
		{
			get
			{
				return this.angle + this.angleDiff;
			}
			set
			{
				this.angle = value;
				this.stChanged = true;
			}
		}

		public double AngleDiff
		{
			get
			{
				return this.angleDiff;
			}
			set
			{
				this.angleDiff = value;
				this.stChanged = true;
			}
		}

		public double AngleRads
		{
			get
			{
				double num = (this.angle + this.angleDiff) * 3.14159265358979 / 180;
				return num;
			}
		}

		public double Distance
		{
			get
			{
				return this.Location.DistanceTo(this.Hit);
			}
		}

		public Point Hit
		{
			get
			{
				Point point;
				if (this.stChanged)
				{
					Point x = new Point(-1, -1);
					Point location = this.Location;
					x.X = location.X + this.Max * Math.Sin(this.AngleRads);
					location = this.Location;
					x.Y = location.Y - this.Max * Math.Cos(this.AngleRads);
					double num = x.DistanceTo(this.Location);
					LineSegment lineSegment = this.LineSegment;
                    if (ext.limits == null)
                        return new Point(0, 0);
					foreach (Limit limit in ext.limits)
					{
						Point point1 = lineSegment.IntersectionWith(limit);
						if ((point1.X != -1 ? true : point1.Y != -1))
						{
							if (num > point1.DistanceTo(this.Location))
							{
								num = point1.DistanceTo(this.Location);
								x = point1;
							}
						}
					}
					this.stChanged = false;
					Point point2 = x;
					location = point2;
					this.hit = point2;
					point = location;
				}
				else
				{
					point = this.hit;
				}
				return point;
			}
		}

		public LineSegment LineSegment
		{
			get
			{
				LineSegment lineSegment = new LineSegment()
				{
					P1 = this.Location
				};
				Point location = this.Location;
				double x = location.X + this.Max * Math.Sin(this.AngleRads);
				location = this.Location;
				lineSegment.P2 = new Point(x, location.Y - this.Max * Math.Cos(this.AngleRads));
				return lineSegment;
			}
		}

		public Point Location
		{
			get;
			set;
		}
        double _max, _min;
		public double Max
		{
            get { return funcs.cmsToWpfPixels( _max); }
            set { _max = value; }
		}

		public double Min
		{
            get { return funcs.cmsToWpfPixels(_min); }
            set { _min = value; }
		}

		public string Name
		{
			get;
			set;
		}

		public Line Ray
		{
			get;
			set;
		}

		public DistanceSensor()
		{
			this.angle = 0;
			this.angleDiff = 0;
			this.Location = new Point(0, 0);
			this.Name = "no-named";
			this.Max = 500;
			this.Min = 15;
			this.stChanged = true;
		}

		public void RefreshVisual()
		{
            try
            {
                Ellipse start = this.Start;
                Point location = this.Location;
                double x = location.X - this.Start.ActualWidth / 2;
                location = this.Location;
                start.Margin = new Thickness(x, location.Y - this.Start.ActualHeight / 2, 0, 0);
                this.Ray.X1 = this.Location.X;
                this.Ray.Y1 = this.Location.Y;
                Point hit = this.Hit;
                this.Stop.Margin = new Thickness(hit.X - this.Stop.ActualWidth / 2, hit.Y - this.Stop.ActualHeight / 2, 0, 0);
                this.Ray.X2 = hit.X;
                this.Ray.Y2 = hit.Y;
                double distance = this.Distance;
                if ((distance < this.Min ? false : distance + 2 <= this.Max))
                {
                    this.Ray.Stroke = Brushes.Blue;
                }
                else
                {
                    this.Ray.Stroke = Brushes.Red;
                }
            }
            catch { }
		}

		public void setControls(Ellipse _start, Ellipse _stop, Line _ray)
		{
			this.Start = _start;
			this.Stop = _stop;
			this.Ray = _ray;
		}
	}
}