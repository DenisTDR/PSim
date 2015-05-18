using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace PSim
{
    public partial class CarSettingsWindow : Window
    {
        public CarSettingsWindow()
        {
            this.InitializeComponent();
            base.Left = 5;
            base.Top = ext.CarStatusWindow.Top + ext.CarStatusWindow.Height + 5;
            ext.EngineRatio = 2;
        }

        private void moveBackwardBtn_Click(object sender, RoutedEventArgs e)
        {
            Direction direction;
            List<CarAction> actionsList = ext.ActionsList;
            CarAction carAction = new CarAction()
            {
                MoveAction = MoveAction.Backward
            };
            CarAction carAction1 = carAction;
            if (this.turnLeftCheckBox.IsChecked.Value)
            {
                direction = Direction.Left;
            }
            else
            {
                direction = (this.turnRightCheckBox.IsChecked.Value ? Direction.Right : Direction.Straight);
            }
            carAction1.Direction = direction;
            carAction.Duration = double.Parse(this.timeTxt.Text);
            actionsList.Add(carAction);
        }

        private void moveForwardBtn_Click(object sender, RoutedEventArgs e)
        {
            Direction direction;
            List<CarAction> actionsList = ext.ActionsList;
            CarAction carAction = new CarAction()
            {
                MoveAction = MoveAction.Forward
            };
            CarAction carAction1 = carAction;
            if (this.turnLeftCheckBox.IsChecked.Value)
            {
                direction = Direction.Left;
            }
            else
            {
                direction = (this.turnRightCheckBox.IsChecked.Value ? Direction.Right : Direction.Straight);
            }
            carAction1.Direction = direction;
            carAction.Duration = double.Parse(this.timeTxt.Text);
            actionsList.Add(carAction);
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.deviationPercentTxt.Text = e.NewValue.ToString();
            ext.deviationPercent = e.NewValue;
        }

        private void Slider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.frontSensorsAngleTxt.Text = e.NewValue.ToString();
            if (ext.distanceSensors != null)
            {
                ext.distanceSensors[0].Angle = -(e.NewValue - 90);
                ext.distanceSensors[1].Angle = e.NewValue - 90;
                ext.distanceSensors[0].RefreshVisual();
                ext.distanceSensors[1].RefreshVisual();
                ext.SensorsWindow.Refresh();
            }
        }

        private void Slider_ValueChanged_2(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
        }

        private void bigParkingRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (ext.MapWindow != null)
                ext.MapWindow.switchParking(bigParkingRadioBtn.IsChecked.Value ? ParkingType.BigParking : ParkingType.LateralParking);
        }

        private void mapSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ext.MapSettingsWindow == null)
                (ext.MapSettingsWindow = new MapSettingsWindow()).Show();
            else
                ext.MapSettingsWindow.Activate();
            ext.MapSettingsWindow.Top = ext.SensorsWindow.Top;
            ext.MapSettingsWindow.Left = ext.SensorsWindow.Left + ext.SensorsWindow.Width + 5;
            ext.MapSettingsWindow.RefreshShits();
        }


        private void frontRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (frontRadioBtn.IsChecked.Value)
            {
                ext.TheCar.LeftEnginesSense = ext.TheCar.RightEnginesSense = 1;
                ext.TheCar.LeftEnginesForce = ext.TheCar.RightEnginesForce = 1;
            }
            else if (rotateRightCheckBox.IsChecked.Value)
            {
                ext.TheCar.LeftEnginesSense = 1;
                ext.TheCar.RightEnginesSense = -1;
                ext.TheCar.LeftEnginesForce = ext.TheCar.RightEnginesForce = 1;
            }
            else if (rotateLeftCheckBox.IsChecked.Value)
            {
                ext.TheCar.LeftEnginesSense = -1;
                ext.TheCar.RightEnginesSense = 1;
                ext.TheCar.LeftEnginesForce = ext.TheCar.RightEnginesForce = 1;
            }
            else if (turnLeftCheckBox.IsChecked.Value)
            {
                ext.TheCar.LeftEnginesSense = ext.TheCar.RightEnginesSense = 1;
                ext.TheCar.LeftEnginesForce = 0.5;
                ext.TheCar.RightEnginesForce = 1;
            }
            else if (turnRightCheckBox.IsChecked.Value)
            {
                ext.TheCar.LeftEnginesSense = ext.TheCar.RightEnginesSense = 1;
                ext.TheCar.LeftEnginesForce = 1;
                ext.TheCar.RightEnginesForce = 0.5;
            }
            try
            {
                ext.MapWindow.refreshMechanicCouple();
            }
            catch { }
        }

        private void enginePowerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                ext.TheCar.MaxEngineForce = enginePowerSlider.Value / 255;
                ext.MapWindow.refreshMechanicCouple();
            }
            catch { }
        }

   

        private void leftEnginesSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (leftEnginesTxt != null)
                leftEnginesTxt.Text = e.NewValue.Round().ToString();
            if (reloading) return;
            ext.TheCar.LeftEnginesForce = e.NewValue/100;
        }

        private void rightEnginesSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (rightEnginesTxt != null)
                rightEnginesTxt.Text = e.NewValue.Round().ToString();
            if (reloading) return;
            ext.TheCar.RightEnginesForce = e.NewValue/100;
        }
        bool reloading = false;
        public void reloadCarProps()
        {
            reloading = true;
            leftEnginesSenseSlider.Value = ext.TheCar.LeftEnginesSense;
            rightEnginesSenseSlider.Value = ext.TheCar.RightEnginesSense;
            leftEnginesSlider.Value = (int)(ext.TheCar.LeftEnginesForce * 100);
            rightEnginesSlider.Value = (int)(ext.TheCar.RightEnginesForce * 100);


            reloading = false;
        }

        private void leftEnginesSenseSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (reloading) return;
            if (leftEnginesSenseLbl != null) leftEnginesSenseLbl.Content = e.NewValue > 0 ? "Front" : "Back";
            ext.TheCar.LeftEnginesSense = (int)e.NewValue;
        }

        private void rightEnginesSenseSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (reloading) return;
            if (rightEnginesSenseLbl != null) rightEnginesSenseLbl.Content = e.NewValue > 0 ? "Front" : "Back";
            ext.TheCar.RightEnginesSense = (int)e.NewValue;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            QueueEntry qe = new QueueEntry();
            qe.BackUpPeriod = qe.Period = 100;
            qe.Repeat = true;
            qe.TheFunction += qe_TheFunction;
            ext.cmdQueue.Add(qe);
        }

        bool qe_TheFunction(QueueEntry qe, EventArgs e)
        {
            if (funcs.getSensorValue(Sensor.FrontLeft) < 50)
            {
                RealFuncs.StopEngines();
                return true;
            }
            return false;
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RealFuncs.rotirePeLoc(2, 250, Engines.LeftEngines);
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            RealFuncs.StopEngines();
        }


    }
}