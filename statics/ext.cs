using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PSim
{
	public static class ext
	{
        
		public static MapWindow MapWindow;
        public static Car TheCar;

		public static CarStatus CarStatusWindow;

		public static LogsWindow LogsWindow;

        public static CarSettingsWindow CarSettingsWindow;

		public static SensorsWindow SensorsWindow;

		public static TestWindow TestWindow;

        public static MapSettingsWindow MapSettingsWindow;

        public static GraphicForm GraphicForm;

		public static List<CarAction> ActionsList;

		public static List<DistanceSensor> distanceSensors;

		public static List<Limit> limits;

		public static double deviationPercent;

		public static double MovingStep;

        public static double EngineRatio = 2;

        public static List<QueueEntry> cmdQueue;

        public static ParkingType ParkingType = ParkingType.BigParking;

        public static bool[,] busyParkingPlaces;
        public static List<Limit>[,] ParkingPlaces;

        public static BitmapImage bigParkingImage, laterallParkingImage;

        static ext()
        {
            ext.deviationPercent = 0;
            ext.MovingStep = 1;
            ext.EngineRatio = -7;
            busyParkingPlaces = new bool[2, 6] { { true, false, true, false, true, false }, { true, false, false, true, false, false } };
            ParkingPlaces = new List<Limit>[2, 6];
            for (int j = 0; j < 6; j++)
            {
                ParkingPlaces[0, j] = new List<Limit>();
                Point[] point = new Point[] { new Point(0, 0), new Point(470, 0), new Point(470, 402), new Point(0, 402), new Point(0, 0) };
                ParkingPlaces[0, j].AddRange(funcs.limitsFromPoints(point));
            }
            funcs.TranslateLimitList(ParkingPlaces[0, 0], new Point(2587, 2902), Brushes.Red);
            funcs.TranslateLimitList(ParkingPlaces[0, 1], new Point(2587, 2193), Brushes.Red);
            funcs.TranslateLimitList(ParkingPlaces[0, 2], new Point(2587, 1484), Brushes.Red);
            funcs.TranslateLimitList(ParkingPlaces[0, 3], new Point(1595, 1484), Brushes.Red);
            funcs.TranslateLimitList(ParkingPlaces[0, 4], new Point(1595, 2193), Brushes.Red);
            funcs.TranslateLimitList(ParkingPlaces[0, 5], new Point(1595, 2902), Brushes.Red);

            for (int j = 0; j < 4; j++)
            {
                ParkingPlaces[1, j] = new List<Limit>();
                Point[] point = new Point[] { new Point(0, 0), new Point(402, 0), new Point(402, 470), new Point(0, 470), new Point(0, 0) };
                ParkingPlaces[1, j].AddRange(funcs.limitsFromPoints(point));
            }
            funcs.TranslateLimitList(ParkingPlaces[1, 0], new Point(617, 1203), Brushes.Red);
            funcs.TranslateLimitList(ParkingPlaces[1, 1], new Point(617, 2195), Brushes.Red);
            funcs.TranslateLimitList(ParkingPlaces[1, 2], new Point(617, 3187), Brushes.Red);
            funcs.TranslateLimitList(ParkingPlaces[1, 3], new Point(617, 4179), Brushes.Red);
            ext.cmdQueue = new List<QueueEntry>();

            ext.bigParkingImage = new BitmapImage();
            ext.bigParkingImage.BeginInit();
            ext.bigParkingImage.UriSource = new Uri("pack://application:,,,/PSim;component/images/pa_4659.4771.jpg");
            ext.bigParkingImage.EndInit();

            ext.laterallParkingImage = new BitmapImage();
            ext.laterallParkingImage.BeginInit();
            ext.laterallParkingImage.UriSource = new Uri("pack://application:,,,/PSim;component/images/lpa_2537.6093.jpg");
            ext.laterallParkingImage.EndInit();
            funcs.bigParkingHWRatio = (double)ext.bigParkingImage.PixelHeight / ext.bigParkingImage.PixelWidth;
            funcs.lateralParkingHWRatio = (double)ext.laterallParkingImage.PixelHeight / ext.laterallParkingImage.PixelWidth;

            funcs.MapActualHeight = 500;
            funcs.MapActualWidth = 700;
            
            //ext.bigParkingImage.Viewport.Width
        }
	}
    public enum ParkingType
    {
        BigParking,
        LateralParking
    }
}