using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace PSim
{
	public partial class CarStatus : Window
	{

        public void reLoadProps()
        {
            txtBlock.Text =
                 "X: " + ext.TheCar.Left.Round()
                 + "\nY: " + ext.TheCar.Top.Round()
                 + "\nRotation Angle: " + ext.TheCar.RotationAngle.Round()
                 + "\nRezultanta Forte: " + ext.TheCar.RezultantaForte.Round()
                 + "\nMoment Rotatie: " + ext.TheCar.MomentRotatie.Round()
                 + "\nMax Engine Power: " + (ext.TheCar.MaxEngineForce * 100).Round() + "%"
                 + "\nLeftEngines:   Sens: " + (ext.TheCar.LeftEnginesSense > 0 ? "F" : "B") + "   Power: " + (ext.TheCar.LeftEnginesForce * 100).Round() + "%"
                 + "\nRightEngines: Sens: " + (ext.TheCar.RightEnginesSense > 0 ? "F" : "B") + "   Power: " + (ext.TheCar.RightEnginesForce * 100).Round() + "%"
                 + "\nDistance: " + ext.TheCar.DistantaParcursa.Round().ToString()
                 + "\nRightParalel: " + RealMeta.isRightParalel().ToString();
                 // + "\nSensor Offset: " + funcs.wpfPixelsToCMs((ext.TheCar.ActualWidth * 3 / 4)).Round().ToString();

        }

		public CarStatus()
		{
			this.InitializeComponent();
			base.Top = 50;
			base.Left = 5;
		}
	}
}