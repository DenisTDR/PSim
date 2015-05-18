using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace PSim
{
	public partial class SensorsWindow : Window
	{
		public SensorsWindow()
		{
			this.InitializeComponent();
			base.Left = 5;
            base.Top = ext.CarSettingsWindow.Top + ext.CarSettingsWindow.Height + 5;
			if (ext.distanceSensors != null)
			{
				base.Height = (double)(70 + 22 * ext.distanceSensors.Count);
			}
		}
		public void Refresh()
		{
			this.namesListBox.Items.Clear();
			this.valuesListBox.Items.Clear();
			foreach (DistanceSensor distanceSensor in ext.distanceSensors)
			{
				this.namesListBox.Items.Add(distanceSensor.Name);
				ItemCollection items = this.valuesListBox.Items;
				double num = Math.Round(distanceSensor.Distance, 2);
				items.Add(num.ToString());
			}
            ext.GraphicForm.tPanel1.AddTPoint(ext.distanceSensors[2].Distance);
			base.Height = (double)(80 + 25 * ext.distanceSensors.Count);
		}
	}
}