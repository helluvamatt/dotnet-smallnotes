using Common.UI.LocalizedDesignAttributes;
using SmallNotes.Properties;
using SmallNotes.UI.Editors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallNotes.Data.FileDatabaseImpl
{
	public class FileDatabaseDescriptor : IDatabaseDescriptor
	{
		[
		LocalizedDisplayName(typeof(Resources), "FileDatabaseDescriptor_DbFile_DisplayName"),
		LocalizedDescription(typeof(Resources), "FileDatabaseDescriptor_DbFile_Description"),
		LocalizedCategory(typeof(Resources), "FileDatabaseDescriptor_Category_Parameters"),
		Editor(typeof(FileUIEditor), typeof(UITypeEditor))
		]
		public string DbFile { get; set; }

		public IDatabase InitializeDatabase(string userDataFolder)
		{
			if (string.IsNullOrEmpty(DbFile))
			{
				DbFile = userDataFolder;
			}
			return new FileDatabase(this);
		}

		[Browsable(false)]
		public string DisplayName
		{
			get { return Resources.FileDatabaseDescriptor_DisplayName; }
		}

		[Browsable(false)]
		public string Description
		{
			get { return Resources.FileDatabaseDescriptor_Description; }
		}
	}
}
