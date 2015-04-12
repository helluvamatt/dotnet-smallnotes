using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallNotes.Data.TestDatabaseImpl
{
	public class TestDatabaseDescriptor : IDatabaseDescriptor
	{
		public IDatabase InitializeDatabase(string userDataFolder)
		{
			return new TestDatabase(this);
		}

		[Browsable(false)]
		public string DisplayName
		{
			get { return "Test Database"; }
		}

		[Browsable(false)]
		public string Description
		{
			get { return "In-memory database with test data."; }
		}
	}
}
