using Common.TrayApplication;
using log4net.Config;
using Mono.Options;
using SmallNotes.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmallNotes
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static void Main(string[] args)
		{
			try
			{
				// Check if this is the only instance of this application
				if (!SingleInstance.Start())
				{
					// This is not the only instance, exit
					Console.WriteLine(Resources.AnotherInstance);
					return;
				}

				// Configure logging
				BasicConfigurator.Configure();

				// Create the TrayApplicationContext - the controller of the entire application
				SmallNotesTrayApplicationContext ctxt = new SmallNotesTrayApplicationContext();
				ctxt.Logger.Info("Initializing...");

				// Options
				OptionSet options = new OptionSet() {
					{ "startup", "Do not show the options form.", v => ctxt.ShowOptionsForm = v == null }
				};
				options.Parse(args);

				// Initialize the application
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				ctxt.InitializeContext();

				// Run the application
				ctxt.Logger.Info("Initialized. Starting main loop...");
				try
				{
					Application.Run(ctxt);
				}
				catch (Exception ex)
				{
					ctxt.Logger.Error("Unexpected exception:", ex);
					MessageBox.Show(ex.Message, Resources.ProgramTerminated, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

				// Log the shutdown
				ctxt.Logger.Info("Exiting...");
			}
			finally
			{
				// all finished so release the mutex
				SingleInstance.Stop();
			}	
		}
	}
}
