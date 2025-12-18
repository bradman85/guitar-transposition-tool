using System;
using System.Windows.Forms;


namespace GuitarTranspositionTool
{
	internal static class Program
	{
		[STAThread]
		static void Main()
		{
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			// Create and run the clean form
			Application.Run(new MainForm());
		}
	}
}
