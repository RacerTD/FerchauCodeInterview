using System.Configuration;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows;

namespace CarRental
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private bool _useConsole = true;

		[DllImport("kernel32.dll")]
		public static extern bool AllocConsole();

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			if (_useConsole)
			{
				AllocConsole();
				Console.WriteLine("Console initialized. Logs will appear here.");
			}
		}
	}

}
