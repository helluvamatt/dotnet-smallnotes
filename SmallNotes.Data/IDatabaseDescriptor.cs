using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallNotes.Data
{
	public interface IDatabaseDescriptor
	{
		IDatabase InitializeDatabase(string userDataFolder);

		string DisplayName { get; }

		string Description { get; }
	}
}
