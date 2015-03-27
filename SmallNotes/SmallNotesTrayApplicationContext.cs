using Common.TrayApplication;
using SmallNotes.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmallNotes
{
	public class SmallNotesTrayApplicationContext : TrayApplicationContext
	{
		/// <summary>
		/// Cached feed list
		/// </summary>
		//public Dictionary<int, Feed> Feeds { get; private set; }

		/// <summary>
		/// Config database
		/// </summary>
		//public Database ConfigDatabase { get; private set; }

		public SmallNotesTrayApplicationContext() : base()
		{
			// TODO Initialize variables
		}

		#region TrayApplicationContext implementation

		protected override void OnInitializeContext()
		{
			// TODO Connect databases, load notes, and display note forms, etc...
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

		#endregion

		#region Event handlers

		private void showOptionsMenuItem_Click(object sender, EventArgs e)
		{
			CreateOptionsForm();
		}

		private void newNoteMenuItem_Click(object sender, EventArgs e)
		{
			// TODO Create a new note, show it's form, etc...
		}

		private void exitMenuItem_Click(object sender, EventArgs e)
		{
			ExitThread();
		}

		#endregion
	}
}
