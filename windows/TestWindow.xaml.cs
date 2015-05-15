using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace PSim
{
	public partial class TestWindow : Window
	{
		public TestWindow()
		{
			this.InitializeComponent();
            this.Loaded += TestWindow_Loaded;
		}

        void TestWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RandomMersene.init_genrand((uint)DateTime.Now.Ticks);
            for (int i = 0; i < 200; i++)
            {
                lb.Items.Add(RandomMersene.genrand_real1());
            }
        }

	}
}