using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSQL.Depends.Models
{
	internal class TSQLSynonym
	{
		public string DatabaseName { get; set; }

		public string SchemaName { get; set; }

		public string Name { get; set; }

		public int ObjectID { get; set; }

		public string BaseServerName { get; set; }

		public string BaseDatabaseName { get; set; }

		public string BaseSchemaName { get; set; }

		public string BaseObjectName { get; set; }
	}
}
