using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PSim
{
    /// <summary>
    /// Interaction logic for Car.xaml
    /// </summary>
    public partial class Car : UserControl
    {

        public Car()
        {
            this.InitializeComponent();
            this.SizeChanged += Car_SizeChanged;
        }

        void Car_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Polygon pointCollections = this.thePolygon;
            Point[] point = new Point[] { new Point(base.Width / 2, base.Height / 5), new Point(base.Width / 4, base.Height * 3 / 4), new Point(base.Width * 3 / 4, base.Height * 3 / 4) };
            pointCollections.Points = new PointCollection(point);
        }

        public double Left
        {
            get
            {
                return base.Margin.Left;
            }
            set
            {
                double top = base.Margin.Top;
                double right = base.Margin.Right;
                Thickness margin = base.Margin;
                Thickness thickness = new Thickness(value, top, right, margin.Bottom);
                try
                {
                    base.Margin = thickness;
                }
                catch { }
            }
        }
        private double rotationAngle = 0;
        public double MidLeft
        {
            get
            {
                return base.Margin.Left + base.Width / 2;
            }
        }

        public double MidTop
        {
            get
            {
                return base.Margin.Top + base.Height / 2;
            }
        }

        public double RotationAngle
        {
            get
            {
                return rotationAngle;
            }
            set
            {
                rotationAngle = value % 360;
                RotateTransform rotateTransform = new RotateTransform(rotationAngle, base.ActualWidth / 2, base.ActualHeight / 2);
                base.RenderTransform = rotateTransform;
            }
        }

        public double RotationAngleRads
        {
            get
            {
                return this.RotationAngle * 3.14159265358979 / 180;
            }
        }

        public double Top
        {
            get
            {
                return base.Margin.Top;
            }
            set
            {
                double left = base.Margin.Left;
                double right = base.Margin.Right;
                Thickness margin = base.Margin;
                Thickness thickness = new Thickness(left, value, right, margin.Bottom);
                try
                {
                    base.Margin = thickness;
                }
                catch { }
            }
        }
        double _leftEnginesForce, _rightEnginesForce;
        int _leftEnginesSense, _rightEnginesSense;
        public double LeftEnginesForce
        {
            get { return _leftEnginesForce; }
            set { _leftEnginesForce = value; propChanged(); }
        }
        public int LeftEnginesSense
        {
            get { return _leftEnginesSense; }
            set { _leftEnginesSense = value; propChanged(); }
        }
        public double LeftEnginesResult
        {
            get
            {
                return LeftEnginesForce * LeftEnginesSense * MaxEngineForce * this.ActualHeight * 150 / 2 / 100;
            }
        }
        public double RightEnginesForce
        {
            get { return _rightEnginesForce; }
            set { _rightEnginesForce = value; propChanged(); }
        }
        public int RightEnginesSense
        {
            get { return _rightEnginesSense; }
            set { _rightEnginesSense = value; propChanged(); }
        }
        public double RightEnginesResult
        {
            get { return RightEnginesForce * RightEnginesSense * MaxEngineForce * this.ActualHeight * 150 / 2 / 100; ; }
        }
        double _maxEngineForce;
        public double MaxEngineForce
        {
            get { return _maxEngineForce; }
            set { _maxEngineForce = value; propChanged(); }
        }
        public double EngineRatio { get; set; }

        void propChanged()
        {
            try
            {
                if (ext.CarSettingsWindow == null || ext.MapWindow == null)
                    return;
                ext.CarSettingsWindow.reloadCarProps();
                ext.MapWindow.refreshMechanicCouple();
            }
            catch { }
        }
        
        public void autoTranslateRotate(double distance, Direction dir, bool random = true)
        {
            if (random)
            {
                double oldRatio = ext.EngineRatio;
                do
                {
                    ext.EngineRatio = oldRatio + RandomMersene.genrand_real1() * ext.deviationPercent * 50 / 100 * (RandomMersene.genrand_bool() ? -1.0 : 1.0);
                } while (ext.EngineRatio <= 1);
                this.autoTranslateRotate(distance, dir, false);

                //funcs.Log("r r w: " + (ext.EngineRatio - oldRatio).ToString());

                ext.EngineRatio = oldRatio;
                return;
            }

            if (ext.EngineRatio == 1)
            {
                this.Move(distance, false);
                return;
            }
            double actualWidth = ext.MapWindow.theCar.ActualWidth;
            double engineRatio = actualWidth / (ext.EngineRatio - 1);
            double num = engineRatio + actualWidth / 2;
            double num1 = 360 * distance / (6.28318530717959 * num);
            double num2 = 2 * Math.Pow(num, 2);
            double num3 = Math.Sqrt(num2 - num2 * Math.Cos(num1 * 3.14159265358979 / 180));
            double num4 = (180 - num1) / 2;
            double num5 = num3 * Math.Cos(num4 * 3.14159265358979 / 180);
            double num6 = num3 * Math.Sin(num4 * 3.14159265358979 / 180);
            if (dir == Direction.Left)
            {
                num1 = -num1;
                num5 = -num5;
            }
            if (distance < 0)
            {
                num5 = -num5;
                num6 = -num6;
            }
            double num7 = Math.Sin(this.RotationAngleRads) * num6;
            num7 = num7 + Math.Cos(this.RotationAngleRads) * num5;
            double num8 = Math.Cos(this.RotationAngleRads) * num6;
            num8 = num8 - Math.Sin(this.RotationAngleRads) * num5;
            Car top = this;
            top.Top = top.Top - num8;
            Car left = this;
            left.Left = left.Left + num7;
            Car rotationAngle = this;
            rotationAngle.RotationAngle = rotationAngle.RotationAngle + num1;
        }

        public void Move(double dist, bool random = true)
        {
            if (random)
            {
                double oldRatio = ext.EngineRatio;
                ext.EngineRatio = RandomMersene.genrand_real1() * ext.deviationPercent * 40 / 100 + 1;
                this.autoTranslateRotate(dist, RandomMersene.genrand_bool() ? Direction.Left : Direction.Right, false);
                //funcs.Log("m r w:" + (ext.EngineRatio - 1).ToString());
                ext.EngineRatio = oldRatio;
                return;
            }

            Car top = this;
            top.Top = top.Top - dist * Math.Cos(this.RotationAngleRads);
            Car left = this;
            left.Left = left.Left + dist * Math.Sin(this.RotationAngleRads);
        }

        public void translateRotate(double front, double right, double angle, bool random = true)
        {
            double num = Math.Sin(this.RotationAngleRads) * front;
            num = num + Math.Cos(this.RotationAngleRads) * right;
            double num1 = Math.Cos(this.RotationAngleRads) * front;
            num1 = num1 - Math.Sin(this.RotationAngleRads) * right;
            Car top = this;
            top.Top = top.Top - num1;
            Car left = this;
            left.Left = left.Left + num;
            Car rotationAngle = this;
            rotationAngle.RotationAngle = rotationAngle.RotationAngle + angle;
        }
        public double MomentRotatie
        {
            get
            {
                return - this.LeftEnginesForce * this.MaxEngineForce * this.LeftEnginesSense + this.RightEnginesForce * this.MaxEngineForce * this.RightEnginesSense;
            }
        }
        public double RezultantaForte
        {
            get
            {
                return this.LeftEnginesSense * this.LeftEnginesForce * this.MaxEngineForce + this.RightEnginesSense * this.RightEnginesForce * this.MaxEngineForce;
            }
        }
        public double CuantaRezultantaForte
        {
            get
            {
                return 5 * this.Width / funcs.GetCarRW;
            }
        }
        public void translateRotateCuplu()
        {
            if (RezultantaForte == 0)
            {
                double angle;
                angle = -MomentRotatie / (3 * this.MaxEngineForce);
                this.RotationAngle += angle;

            }
            else if (LeftEnginesForce == 1 && RightEnginesForce == 1)
            {
                Move(this.RezultantaForte * this.CuantaRezultantaForte, false);
                //funcs.Log("d: " + (this.RezultantaForte * this.CuantaRezultantaForte).Round());
            }
            else
            {
                double rpr = Math.Min(LeftEnginesForce, RightEnginesForce);
                double r1, r2, r;
                r1 = (rpr + 1) / funcs.GetCarRW;
                r2 = funcs.GetCarRW / (rpr - 1);
                r = ((r1 + r2) / 2);
                double alpha = this.RezultantaForte * this.CuantaRezultantaForte / r / 2;
                double directLength = Math.Sqrt(2 * r * r * (1 - Math.Cos(alpha))) * 2.2 * Math.Sign(this.RezultantaForte);

                double angle;
                angle = -MomentRotatie / (3 * this.MaxEngineForce);

                Move(directLength, false);

                this.RotationAngle += angle*2;

                //funcs.Log("d: " + (this.RezultantaForte * this.CuantaRezultantaForte).Round() + "  dl: "+directLength.Round()+"  angl: " + angle.Round());
            }
        }

    }
}
