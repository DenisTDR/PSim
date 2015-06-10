using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Threading;
using System.Windows.Threading;

namespace PSim
{
	public class App : Application
	{
		public App()
		{
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
		}

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Process unhandled exception
            //MessageBox.Show("an error:\n" + e.Exception.Message);
            // Prevent default unhandled exception processing
            e.Handled = false;
        }

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			base.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[STAThread]
		public static void Main()
		{
			App app = new App();
			app.InitializeComponent();
			app.Run();
		}
	}
}