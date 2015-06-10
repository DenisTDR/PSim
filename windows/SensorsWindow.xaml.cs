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
            for(int i=0;i<ext.distanceSensors.Count;i++){
				this.namesListBox.Items.Add(ext.distanceSensors[i].Name);
                double num = RealFuncs.getSensorValue((Sensor)i).Round();
				this.valuesListBox.Items.Add(num.ToString());
			}
            if (ext.GraphicForm != null)
                ext.GraphicForm.tPanel1.AddTPoint(ext.distanceSensors[2].Distance);
			base.Height = (double)(80 + 25 * ext.distanceSensors.Count);
		}
	}
}