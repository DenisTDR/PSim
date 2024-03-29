using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Media.Imaging;

namespace PSim
{
	public partial class MapWindow : Window
	{
		private CarAction keyboardAction;

		private double lastBigGridWidth = -1;

		private bool resizing = false;

		private Timer theTimer;

		private List<Line> lines = new List<Line>();


        int tmpRefreshShitsTimes = 0;

        public Car theCar;

		public MapWindow()
		{
            this.InitializeComponent();
            (ext.LogsWindow = new LogsWindow()).Show();
            ext.TheCar = theCar = new Car();
            theCar.HorizontalAlignment = HorizontalAlignment.Left;
            theCar.VerticalAlignment = VerticalAlignment.Top;
            theCar.LeftEnginesForce = theCar.RightEnginesForce = 1;
            theCar.MaxEngineForce = 1;
            theCar.EngineRatio = 1;
            this.bigGrid.Children.Add(theCar);

            ext.ParkingType = ParkingType.BigParking;
			ext.ActionsList = new List<CarAction>();
			this.theTimer = new Timer()
			{
				Interval = 10
			};
			this.theTimer.Elapsed += new ElapsedEventHandler(this.theTimer_Elapsed);
			this.theTimer.Start();
			CarStatus carStatu = new CarStatus();
			ext.CarStatusWindow = carStatu;
			carStatu.Show();
            CarSettingsWindow newActionWindow = new CarSettingsWindow();
            ext.CarSettingsWindow = newActionWindow;
			newActionWindow.Show();
			SensorsWindow sensorsWindow = new SensorsWindow();
			ext.SensorsWindow = sensorsWindow;
			sensorsWindow.Show();
            (ext.GraphicForm = new GraphicForm()).Show();
			base.Closed += new EventHandler(this.MapWindow_Closed);
			base.SizeChanged += new SizeChangedEventHandler(this.MapWindow_SizeChanged);
			base.KeyDown += new KeyEventHandler(this.MapWindow_KeyDown);
			base.KeyUp += new KeyEventHandler(this.MapWindow_KeyUp);
            base.Loaded += new RoutedEventHandler(this.MapWindow_Loaded);

            lLeft = new Line() { Stroke = Brushes.Gray, StrokeThickness = 5 };
            bigGrid.Children.Add(lLeft);
            lRight = new Line() { Stroke = Brushes.Gray, StrokeThickness = 5 };
            bigGrid.Children.Add(lRight);
            lResultForce = new Line() { Stroke = Brushes.Gray, StrokeThickness = 5 };
            bigGrid.Children.Add(lResultForce);
            funcs.initTimers();
			this.createSensors();
		}
        Line lLeft, lRight, lResultForce;

		private Point calculateShits(LineSegment sensorLine)
		{
			Point point = new Point(-1, -1);
			double num = 10000000000;
			foreach (LineSegment limit in ext.limits)
			{
				Point point1 = sensorLine.IntersectionWith(limit);
				if ((point1.X != -1 ? true : point1.Y != -1))
				{
					if (num > sensorLine.P1.DistanceTo(point1))
					{
						num = sensorLine.P1.DistanceTo(point1);
						point = point1;
					}
				}
			}
			return point;
		}

		private void createSensors()
		{
			if (ext.distanceSensors == null)
			{
				ext.distanceSensors = new List<DistanceSensor>();
			}
			else
			{
				ext.distanceSensors.Clear();
            }
			List<DistanceSensor> distanceSensors = ext.distanceSensors;
			DistanceSensor distanceSensor = new DistanceSensor()
			{
                Name = "FrontRight",
				Angle = 45,
                Min = 100,
                Max = 800
            };
			distanceSensors.Add(distanceSensor);
			List<DistanceSensor> distanceSensors1 = ext.distanceSensors;
			DistanceSensor distanceSensor1 = new DistanceSensor()
			{
                Name = "FrontLeft",
				Angle = -45,
                Min = 100,
                Max = 800
			};
			distanceSensors1.Add(distanceSensor1);
			List<DistanceSensor> distanceSensors2 = ext.distanceSensors;
			DistanceSensor distanceSensor2 = new DistanceSensor()
			{
				Name = "SideRight",
				Angle = 90,
                Min = 40,
                Max = 400
			};
			distanceSensors2.Add(distanceSensor2);
			List<DistanceSensor> distanceSensors3 = ext.distanceSensors;
			DistanceSensor distanceSensor3 = new DistanceSensor()
			{
				Name = "SideLeft",
                Angle = -90,
                Min = 40,
                Max = 400
			};
			distanceSensors3.Add(distanceSensor3);
			foreach (DistanceSensor distanceSensor4 in ext.distanceSensors)
			{
				Ellipse ellipse = new Ellipse();
				Ellipse ellipse1 = new Ellipse();
				double num = 8;
				double num1 = num;
				ellipse1.Height = num;
				double num2 = num1;
				num1 = num2;
				ellipse1.Width = num2;
				double num3 = num1;
				num1 = num3;
				ellipse.Height = num3;
				ellipse.Width = num1;
				int num4 = 0;
				System.Windows.HorizontalAlignment horizontalAlignment = (System.Windows.HorizontalAlignment)num4;
				ellipse.HorizontalAlignment = (System.Windows.HorizontalAlignment)num4;
				ellipse1.HorizontalAlignment = horizontalAlignment;
				int num5 = 0;
				System.Windows.VerticalAlignment verticalAlignment = (System.Windows.VerticalAlignment)num5;
				ellipse.VerticalAlignment = (System.Windows.VerticalAlignment)num5;
				ellipse1.VerticalAlignment = verticalAlignment;
				SolidColorBrush black = Brushes.Black;
				Brush brush = black;
				ellipse.Stroke = black;
				ellipse1.Stroke = brush;
				SolidColorBrush yellow = Brushes.Yellow;
				brush = yellow;
				ellipse.Fill = yellow;
				ellipse1.Fill = brush;
				double num6 = 1;
				num1 = num6;
				ellipse.StrokeThickness = num6;
				ellipse1.StrokeThickness = num1;
				Line line = new Line()
				{
					StrokeThickness = 3,
					Stroke = Brushes.Blue
				};
				distanceSensor4.setControls(ellipse, ellipse1, line);
				this.bigGrid.Children.Add(line);
				this.bigGrid.Children.Add(ellipse);
				this.bigGrid.Children.Add(ellipse1);
				Panel.SetZIndex(line, 10);
				Panel.SetZIndex(ellipse, 11);
				Panel.SetZIndex(ellipse1, 11);
			}
		}

        public void switchParking(ParkingType _parkingType)
        {
            ext.ParkingType = _parkingType;
            if (ext.ParkingType == ParkingType.LateralParking)
            {
                bigGrid.Background = new ImageBrush(ext.laterallParkingImage);                
                theCar.RotationAngle = 180.01;                
            }
            else
            {
                bigGrid.Background = new ImageBrush(ext.bigParkingImage);
                theCar.RotationAngle = 0.01;
            }
            foreach (Line line in this.lines)
                this.bigGrid.Children.Remove(line);
            this.lines.Clear();

            MapWindow_SizeChanged(null, null);
            if (ext.ParkingType == ParkingType.LateralParking)
            {
                theCar.Left = 1160 * bigGrid.ActualHeight / funcs.getRH();
                theCar.Top = 27 * bigGrid.ActualHeight / funcs.getRH();
            }
            else
            {
                
                theCar.Left = 2140 * bigGrid.ActualHeight / funcs.getRH();
                theCar.Top = 4500 * bigGrid.ActualHeight / funcs.getRH(); // 4553  4500
            } 
            lastBigGridWidth = bigGrid.ActualWidth;
            
            tmpRefreshShitsTimes = 1;

            if (ext.MapSettingsWindow != null)
                ext.MapSettingsWindow.RefreshShits();
        }
        public void RefreshCozBusyParkingPlacesChanged()
        {
            foreach (Line line in this.lines)
                this.bigGrid.Children.Remove(line);
            this.lines.Clear();
            drawLimits();
            RefreshShits();
        }
		private void drawLimits()
		{
			Limit item;
            ext.limits = funcs.getParkingAreaLimits(this.bigGrid.ActualWidth);
			if (this.lines.Count != 0)
			{
				for (int i = 0; i < ext.limits.Count; i++)
				{
					item = ext.limits[i];
					this.lines[i].X1 = item.P1.X;
					this.lines[i].Y1 = item.P1.Y;
					this.lines[i].X2 = item.P2.X;
					this.lines[i].Y2 = item.P2.Y;
				}
			}
			else
			{
				foreach (Limit lim in ext.limits)
				{
					Line line = new Line()
					{
                        X1 = lim.P1.X,
                        Y1 = lim.P1.Y,
                        X2 = lim.P2.X,
                        Y2 = lim.P2.Y,
						Stroke = lim.LineColor,
						StrokeThickness = 5
					};
					this.bigGrid.Children.Add(line);
					this.lines.Add(line);
				}
			}
		}

		private void MapWindow_Closed(object sender, EventArgs e)
		{
			Application.Current.Shutdown();
		}
        bool upPressed = false, downPressed = false, leftPressed = false, rightPressed = false;
		private void MapWindow_KeyDown(object sender, KeyEventArgs e)
		{
			if (this.keyboardAction == null)
			{
				this.keyboardAction = new CarAction()
				{
					Duration = 999
				};
			}
			if (e.Key == Key.Up || e.Key==Key.W)
                upPressed = true;
				//this.keyboardAction.MoveAction = MoveAction.Forward;
            else if (e.Key == Key.Down || e.Key == Key.S)
                downPressed = true;
				//this.keyboardAction.MoveAction = MoveAction.Backward
            else if (e.Key == Key.Left || e.Key == Key.A)
                leftPressed = true;
                //this.keyboardAction.Direction = Direction.Left;
            else if (e.Key == Key.Right || e.Key == Key.D)
                rightPressed = true;
                //this.keyboardAction.Direction = Direction.Right;

            if ((e.Key == Key.Up || e.Key == Key.W || e.Key == Key.Down || e.Key == Key.S) 
                ||(e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.A || e.Key == Key.D) )
			{
                refreshForcesCozKeyboard();
                if (this.keyboardAction.Duration < 50)
				{
					//this.keyboardAction.Duration = 1000;
				}
				//ext.ActionsList.Add(this.keyboardAction);
			}
		}

		private void MapWindow_KeyUp(object sender, KeyEventArgs e)
		{
            if (e.Key == Key.Up || e.Key == Key.W)
                upPressed = false;
            if (e.Key == Key.Down || e.Key == Key.S)
                downPressed = false;
            if (e.Key == Key.Left || e.Key == Key.A)
                leftPressed = false;
            if (e.Key == Key.Right || e.Key == Key.D)
                rightPressed = false;
            
            if ((e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.W || e.Key == Key.S))
			{
                refreshForcesCozKeyboard();
			}
            if ((e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.D || e.Key == Key.A))
			{
                refreshForcesCozKeyboard();
			}
		}
        void refreshForcesCozKeyboard()
        {
            if (upPressed)
            {
                ext.TheCar.LeftEnginesSense = 1;
                ext.TheCar.RightEnginesSense = 1;
                if (rightPressed)
                {
                    ext.TheCar.LeftEnginesForce = 1;
                    ext.TheCar.RightEnginesForce = 0;
                }
                else if (leftPressed)
                {
                    ext.TheCar.LeftEnginesForce = 0;
                    ext.TheCar.RightEnginesForce = 1;
                }
                else
                {
                    ext.TheCar.LeftEnginesForce = 1;
                    ext.TheCar.RightEnginesForce = 1;
                }
            }
            else if (downPressed)
            {
                ext.TheCar.LeftEnginesSense = -1;
                ext.TheCar.RightEnginesSense = -1;
                if (rightPressed)
                {
                    ext.TheCar.LeftEnginesForce = 1;
                    ext.TheCar.RightEnginesForce = 0;
                }
                else if (leftPressed)
                {
                    ext.TheCar.LeftEnginesForce = 0;
                    ext.TheCar.RightEnginesForce = 1;
                }
                else
                {
                    ext.TheCar.LeftEnginesForce = 1;
                    ext.TheCar.RightEnginesForce = 1;
                }
            }
            else if (leftPressed)
            {
                ext.TheCar.LeftEnginesSense = -1;
                ext.TheCar.RightEnginesSense = 1;
                ext.TheCar.LeftEnginesForce = 1;
                ext.TheCar.RightEnginesForce = 1;
            }
            else if (rightPressed)
            {
                ext.TheCar.LeftEnginesSense = 1;
                ext.TheCar.RightEnginesSense = -1;
                ext.TheCar.LeftEnginesForce = 1;
                ext.TheCar.RightEnginesForce = 1;
            }
            else
            {
                ext.TheCar.LeftEnginesForce = 0;
                ext.TheCar.RightEnginesForce = 0;
                ext.ActionsList.Clear();
                return;
            }
            ext.ActionsList.Clear();
            ext.ActionsList.Add(new CarAction() { MoveAction = MoveAction.SmartMovement, Duration = 25 /*double.Parse(this.timeTxt.Text)*/});

        }
		private void MapWindow_Loaded(object sender, RoutedEventArgs e)
		{
            switchParking(ext.ParkingType);
            System.Windows.Forms.Timer tmr = new System.Windows.Forms.Timer();
            tmr.Interval = 1000;
            tmr.Tick += (object sender2, EventArgs e2) =>
            {
                tmr.Stop();
                tmr.Dispose();
                //RealMeta.initMersConturInchis();
            };
            tmr.Start();
		}

		private void MapWindow_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (!this.resizing)
			{
				this.resizing = true;

				double actualWidth = this.bigGrid.ActualWidth;
				double actualHeight = this.bigGrid.ActualHeight;
				double width = base.Width - actualWidth;
				double height = base.Height - actualHeight;
				actualWidth = Math.Min(actualWidth, actualHeight);
                base.Width = actualHeight / funcs.getHWRatio() + width;
                base.Height = actualHeight + height;

                this.theCar.Width = funcs.imagePixelsToWPFPixels(funcs.GetCarRW);// 401.0 *actualHeight / funcs.getRH(); 
                this.theCar.Height = funcs.imagePixelsToWPFPixels(funcs.GetCarRH); ;// 471.0 * actualHeight / funcs.getRH();

				this.drawLimits();
				if (this.lastBigGridWidth != -1)
				{
					this.theCar.Left = this.theCar.Left * actualWidth / this.lastBigGridWidth;
					this.theCar.Top = this.theCar.Top * actualWidth / this.lastBigGridWidth;
				}
				this.lastBigGridWidth = actualWidth;
                this.theCar.RotationAngle = this.theCar.RotationAngle;
                this.RefreshShits();
                this.resizing = false;

                funcs.MapActualHeight = bigGrid.ActualHeight;
                funcs.MapActualWidth = bigGrid.ActualWidth;
                
			}
		}

        public void RefreshShits()
        {
            double midLeft = this.theCar.MidLeft + this.theCar.ActualHeight / 2 * Math.Sin(this.theCar.RotationAngleRads) + this.theCar.ActualWidth / 4 * Math.Sin(this.theCar.RotationAngleRads - 1.5707963267949);
            double midTop = this.theCar.MidTop - this.theCar.ActualHeight / 2 * Math.Cos(this.theCar.RotationAngleRads) - this.theCar.ActualWidth / 4 * Math.Cos(this.theCar.RotationAngleRads - 1.5707963267949);
            ext.distanceSensors[0].Location = new Point(midLeft, midTop);
            ext.distanceSensors[0].AngleDiff = this.theCar.RotationAngle;
            ext.distanceSensors[0].RefreshVisual();
            midLeft = this.theCar.MidLeft + this.theCar.ActualHeight / 2 * Math.Sin(this.theCar.RotationAngleRads) - this.theCar.ActualWidth / 4 * Math.Sin(this.theCar.RotationAngleRads - 1.5707963267949);
            midTop = this.theCar.MidTop - this.theCar.ActualHeight / 2 * Math.Cos(this.theCar.RotationAngleRads) + this.theCar.ActualWidth / 4 * Math.Cos(this.theCar.RotationAngleRads - 1.5707963267949);
            ext.distanceSensors[1].Location = new Point(midLeft, midTop);
            ext.distanceSensors[1].AngleDiff = this.theCar.RotationAngle;
            ext.distanceSensors[1].RefreshVisual();
            midLeft = this.theCar.MidLeft + this.theCar.ActualWidth / 2 * Math.Cos(this.theCar.RotationAngleRads);
            midTop = this.theCar.MidTop + this.theCar.ActualWidth / 2 * Math.Sin(this.theCar.RotationAngleRads);
            ext.distanceSensors[2].Location = new Point(midLeft, midTop);
            ext.distanceSensors[2].AngleDiff = this.theCar.RotationAngle;
            ext.distanceSensors[2].RefreshVisual();
            midLeft = this.theCar.MidLeft - this.theCar.ActualWidth / 2 * Math.Cos(this.theCar.RotationAngleRads);
            midTop = this.theCar.MidTop - this.theCar.ActualWidth / 2 * Math.Sin(this.theCar.RotationAngleRads);
            ext.distanceSensors[3].Location = new Point(midLeft, midTop);
            ext.distanceSensors[3].AngleDiff = this.theCar.RotationAngle;
            ext.distanceSensors[3].RefreshVisual();
            ext.SensorsWindow.Refresh();

            refreshMechanicCouple();
        }

        public void refreshMechanicCouple()
        {

            lLeft.Y1 = theCar.MidTop
              - Math.Sin(theCar.RotationAngleRads) * theCar.Width / 2;
            lLeft.X1 = theCar.MidLeft
                - Math.Cos(theCar.RotationAngleRads) * theCar.Width / 2;


            lLeft.Y2 = theCar.MidTop + Math.Cos(theCar.RotationAngleRads) * -theCar.LeftEnginesResult
                - Math.Sin(theCar.RotationAngleRads) * theCar.Width / 2;
            lLeft.X2 = theCar.MidLeft - Math.Sin(theCar.RotationAngleRads) * -theCar.LeftEnginesResult
               - Math.Cos(theCar.RotationAngleRads) * theCar.Width / 2;


            lRight.Y1 = theCar.MidTop +
              +Math.Sin(theCar.RotationAngleRads) * theCar.Width / 2;
            lRight.X1 = theCar.MidLeft
                + Math.Cos(theCar.RotationAngleRads) * theCar.Width / 2;

            
            lRight.Y2 = theCar.MidTop - Math.Cos(theCar.RotationAngleRads) * theCar.RightEnginesResult
                + Math.Sin(theCar.RotationAngleRads) * theCar.Width / 2;
            lRight.X2 = theCar.MidLeft + Math.Sin(theCar.RotationAngleRads) * theCar.RightEnginesResult
               + Math.Cos(theCar.RotationAngleRads) * theCar.Width / 2;

            lResultForce.X1 = theCar.MidLeft;
            lResultForce.Y1 = theCar.MidTop;

            double resultForce = theCar.RezultantaForte * theCar.Height/2 * 150 / 100;
            lResultForce.Y2 = theCar.MidTop - resultForce * Math.Cos(theCar.RotationAngleRads);
            lResultForce.X2 = theCar.MidLeft + resultForce * Math.Sin(theCar.RotationAngleRads);

            ext.CarStatusWindow.reLoadProps();
        }

		private void theTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
            
			double movingStep;
			if (ext.ActionsList.Count != 0)
			{
                CarAction carAction = ext.ActionsList[0];
                if (carAction.Duration > 0)
				{
					Action action = null;
					double num = RandomMersene.genrand_real1() * ((RandomMersene.genrand_bool() ? -1.0 : 1.0));
                    if (carAction.MoveAction == MoveAction.SmartMovement)
                    {
                        action = () => this.theCar.translateRotateCuplu();
                        carAction.Duration -= this.theTimer.Interval / 1000;
                    }
                    else if (carAction.Direction != Direction.Straight)
					{
                        movingStep = (double)((carAction.MoveAction == MoveAction.Forward ? 1 : -1)) * ext.MovingStep + num * ext.deviationPercent / 100 * ext.MovingStep;
                        action = () => this.theCar.autoTranslateRotate(movingStep, carAction.Direction);
                        carAction.Duration = carAction.Duration - this.theTimer.Interval / 1000;
					}
					else
					{
                        movingStep = (double)((carAction.MoveAction == MoveAction.Forward ? 1 : -1)) * ext.MovingStep + num * ext.deviationPercent / 100 * ext.MovingStep;
						action = () => this.theCar.Move(movingStep);
						carAction.Duration = carAction.Duration - this.theTimer.Interval / 1000;
					}
					System.Windows.Threading.Dispatcher dispatcher = base.Dispatcher;
					if (action != null)
                    {
                        Action action1 = () =>
                        {
							if (action != null)
                            {
                                action();
							}
							this.RefreshShits();
						};
						if (!dispatcher.CheckAccess())
						{
							try
							{
								dispatcher.Invoke(action1);
							}
							catch
							{
							}
						}
						else
						{
							action1();
						}
					}
				}
				else
				{
                    ext.ActionsList.Remove(carAction);
				}
			}
            if (tmpRefreshShitsTimes > 0)
            {
                Action action1 = () =>
                {
                    this.theCar.autoTranslateRotate(0, Direction.Left);
                    this.RefreshShits();
                };
                System.Windows.Threading.Dispatcher dispatcher = base.Dispatcher;
                if (!dispatcher.CheckAccess())
                {
                    try
                    {
                        dispatcher.Invoke(action1);
                    }
                    catch
                    {
                    }
                }
                else
                {
                    action1();
                }
                tmpRefreshShitsTimes--;
            }
		}
	}
}