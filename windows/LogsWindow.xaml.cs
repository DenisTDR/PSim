using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Markup;

namespace PSim
{
	public partial class LogsWindow : Window
	{
		public LogsWindow()
		{
			this.InitializeComponent();
			try
			{
				base.Left = SystemParameters.PrimaryScreenWidth - 5 - base.Width;
				base.Top = 50;
				base.Height = SystemParameters.PrimaryScreenHeight - 100;
			}
			catch
			{

			}
		}

		public void AddLog(string log)
		{
			this.theListBox.Items.Add(log);
			ListBoxAutomationPeer listBoxAutomationPeer = (ListBoxAutomationPeer)UIElementAutomationPeer.CreatePeerForElement(this.theListBox);
			IScrollProvider pattern = (IScrollProvider)listBoxAutomationPeer.GetPattern(PatternInterface.Scroll);
			ScrollAmount scrollAmount = ScrollAmount.LargeIncrement;
			ScrollAmount scrollAmount1 = ScrollAmount.NoAmount;
			if (pattern.VerticallyScrollable)
			{
				pattern.Scroll(scrollAmount1, scrollAmount);
			}
		}
	}
}