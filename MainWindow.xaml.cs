using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace PSim
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
            RandomMersene.init_genrand(DateTime.Now.Ticks);
			this.InitializeComponent();
			MapWindow mapWindow = new MapWindow();
			ext.MapWindow = mapWindow;
			mapWindow.Show();
			base.Close();
		}
	}
}