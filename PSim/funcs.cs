using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Timers;

namespace PSim
{
	public static class funcs
	{
		public static double DistanceTo(this Point p1, Point p2)
		{
			double num = Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
			return num;
		}

        public static List<Limit> getParkingAreaLimits(double wSize)
        {
            List<Limit> limits = new List<Limit>();
            if (ext.ParkingType == ParkingType.BigParking)
            {

                Point[] point = new Point[] { new Point(1994, 4771), new Point(1994, 4411), new Point(369, 4411), new Point(369, 2736), new Point(0, 2729) };
                limits.AddRange(funcs.limitsFromPoints(point));
                point = new Point[] { new Point(0, 2043), new Point(369, 2043), new Point(369, 369), new Point(1994, 369), new Point(1994, 0) };
                limits.AddRange(funcs.limitsFromPoints(point));
                point = new Point[] { new Point(2670, 0), new Point(2672, 369), new Point(4290, 369), new Point(4290, 2043), new Point(4659, 2043) };
                limits.AddRange(funcs.limitsFromPoints(point));
                point = new Point[] { new Point(4659, 2736), new Point(4290, 2736), new Point(4290, 4411), new Point(2672, 4411), new Point(2672, 4771) };
                limits.AddRange(funcs.limitsFromPoints(point));
                Limit[] limit = new Limit[] { new Limit(1313, 1313, 3345, 1313), new Limit(1313, 2028, 3345, 2028), new Limit(1313, 2737, 3345, 2737), new Limit(1313, 3459, 3345, 3459), new Limit(2329, 1313, 2329, 3459) };
                limits.AddRange(limit);

                for (int i = 0; i < 6; i++)
                    if (ext.busyParkingPlaces[0, i])
                        foreach (Limit lim in ext.ParkingPlaces[0, i])
                            limits.Add(lim.GetCopy());

                for (int i = 0; i < limits.Count; i++)
                    limits[i].Scalare(wSize / 4659);

            }
            else
            {
                limits.Add(new Limit(500, 190, 500, 6088) { LineColor = Brushes.Blue });
                limits.Add(new Limit(2219, 189, 2219, 6088) { LineColor = Brushes.Blue });


                for (int i = 0; i < 4; i++)
                    if (ext.busyParkingPlaces[1, i])
                        foreach (Limit lim in ext.ParkingPlaces[1, i])
                            limits.Add(lim.GetCopy());

                for (int i = 0; i < limits.Count; i++)
                    limits[i].Scalare(wSize / 2537);

            }
            return limits;
        }

		public static List<Limit> limitsFromPoints(IEnumerable<Point> points)
		{
			List<Limit> limits = new List<Limit>();
			Point point = new Point();
			bool flag = true;
			foreach (Point point1 in points)
			{
				if (!flag)
				{
					limits.Add(new Limit(point, point1));
					point = point1;
				}
				else
				{
					point = point1;
					flag = false;
				}
			}
			return limits;
		}

        public static double getHWRatio()
        {
            return ext.ParkingType == ParkingType.BigParking ? 1.024 : 2.401655;
        }
        public static int getRH()
        {
            return ext.ParkingType == ParkingType.BigParking ? 4771 : 6093;
        }
        public static int GetCarRW { get { return 402; } }
        public static int GetCarRH { get { return 470; } }

        public static void TranslateLimitList(List<Limit> list, Point p, Brush Color)
        {
            foreach (Limit lim in list)
                lim.Translatare(p).LineColor = Color;
        }
		public static void Log(string log)
		{
			if (ext.LogsWindow != null)
			{
				ext.LogsWindow.AddLog(log);
			}
		}

		public static Point Round(this Point p, int dec = 0)
		{
			Point point = new Point(Math.Round(p.X, dec), Math.Round(p.Y, dec));
			return point;
		}

		public static double Round(this double d, int dec = 2)
		{
			return Math.Round(d, dec);
		}
        public static double Length(this System.Windows.Shapes.Line line)
        {
            return Math.Sqrt(Math.Pow(line.X1 - line.X2, 2) + Math.Pow(line.Y1 - line.Y2, 2));
        }
        public static double getSensorValue(Sensor sensor)
        {
            return ext.distanceSensors[(int)sensor].Distance;
        }

        public static Timer timmingTimer;

        public static void initTimers()
        {
            timmingTimer = new Timer();
            timmingTimer.Elapsed += timmingTimer_Elapsed;
            timmingTimer.Interval = 100;
            timmingTimer.Start();
            funcs.Log("timming inited");
        }

        static void timmingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Action action = () =>
            {
                int i;
                for (i = 0; i < ext.cmdQueue.Count; i++)
                {
                    ext.cmdQueue[i].Period -= (int)timmingTimer.Interval;
                    if (ext.cmdQueue[i].Period <= 0)
                    {
                        if (ext.cmdQueue[i].Repeat)
                        {
                            ext.cmdQueue[i].Period = ext.cmdQueue[i].BackUpPeriod;
                        }
                        if (ext.cmdQueue[i].CheckIfDone())
                        {
                            ext.cmdQueue.RemoveAt(i);
                            i--;
                        }
                    }
                }
            };
            if (ext.MapWindow.Dispatcher.CheckAccess())
                action();
            else
                ext.MapWindow.Dispatcher.Invoke(action);
        }
	}
}