using Common.Data.Async;
using Common.TrayApplication;
using SmallNotes.Data;
using SmallNotes.Properties;
using SmallNotes.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SmallNotes
{
	public class SmallNotesTrayApplicationContext : TrayApplicationContext
	{
		private const string APP_DATA_PATH = "SmallNotes";

		/// <summary>
		/// Config database
		/// </summary>
		public Database ConfigDatabase { get; private set; }

		private AsyncCallback<Database.LoadNotesResult> LoadCallback { get; set; }

		public SmallNotesTrayApplicationContext() : base()
		{
			// Initialize variables
			// TODO Enable multiple database backends (Filesystem, cloud, etc..)
			ConfigDatabase = new FileDatabase();
			LoadCallback = new AsyncCallback<Database.LoadNotesResult>();
		}

		#region TrayApplicationContext implementation

		protected override void OnInitializeContext()
		{
			// Make sure the AppDataPath folder is available
			Directory.CreateDirectory(GetAppDataPath());

			// Initialize the load event handler
			LoadCallback.AsyncFinished += LoadCallback_AsyncFinished;

			// TODO Load application configuration
			Dictionary<string, string> properties = new Dictionary<string, string>();

			// Initialize the database
			ConfigDatabase.Initialize(GetAppDataPath(), properties);

			// Do the load
			ConfigDatabase.LoadNotesAsync(LoadCallback);
		}

		protected override OptionsForm BuildOptionsForm()
		{
			SmallNotesOptionsForm form = new SmallNotesOptionsForm();
			// TODO Bind events, etc...
			return form;
		}

		protected override void BuildContextMenu()
		{
			// Build the context menu: showOptionsMenuItem
			ToolStripMenuItem showOptionsMenuItem = new ToolStripMenuItem(Resources.MenuItemManage);
			showOptionsMenuItem.Click += showOptionsMenuItem_Click;

			// Build the context menu: feedsMenuItem
			ToolStripMenuItem newNoteMenuItem = new ToolStripMenuItem(Resources.MenuItemNewNote);
			newNoteMenuItem.Click += newNoteMenuItem_Click;

			// Build the context menu: exitMenuItem
			ToolStripMenuItem exitMenuItem = new ToolStripMenuItem(Resources.MenuItemExit);
			exitMenuItem.Click += exitMenuItem_Click;

			// Build contextMenu
			notifyIcon.ContextMenuStrip.Items.Add(showOptionsMenuItem);
			notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
			notifyIcon.ContextMenuStrip.Items.Add(newNoteMenuItem);
			notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
			notifyIcon.ContextMenuStrip.Items.Add(exitMenuItem);
		}

		protected override string GetApplicationName()
		{
			return Resources.AppTitle;
		}

		protected override Icon GetApplicationIcon()
		{
			return new Icon(GetType(), "appicon.ico");
		}

		protected override string GetAppDataPath()
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), APP_DATA_PATH);
		}

		#endregion

		#region Event handlers

		private void showOptionsMenuItem_Click(object sender, EventArgs e)
		{
			CreateOptionsForm();
		}

		private void newNoteMenuItem_Click(object sender, EventArgs e)
		{
			// TODO Create a new note, show it's form, etc...
			Logger.Info("Creating new note...");
		}

		private void exitMenuItem_Click(object sender, EventArgs e)
		{
			ExitThread();
		}

		private void LoadCallback_AsyncFinished(Database.LoadNotesResult result)
		{
			// TODO Redraw windows, possibly adding new ones
		}

		#endregion
	}
}
