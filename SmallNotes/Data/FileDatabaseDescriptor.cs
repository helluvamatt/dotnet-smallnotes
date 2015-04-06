using SmallNotes.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallNotes.Data
{
	public class FileDatabaseDescriptor : IDatabaseDescriptor
	{
		// TODO Attributes [LocalizedDisplayName(), LocalizedDescription(), LocalizedCategory()] ...
		public string DbFile { get; set; }

		public IDatabase InitializeDatabase(string userDataFolder)
		{
			if (string.IsNullOrEmpty(DbFile))
			{
				DbFile = userDataFolder;
			}
			return new FileDatabase(this);
		}

		public string DisplayName
		{
			get { return Resources.FileDatabaseDescriptor_DisplayName; }
		}

		public string Description
		{
			get { return Resources.FileDatabaseDescriptor_Description; }
		}
	}
}
