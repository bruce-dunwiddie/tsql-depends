using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSQL.Depends
{
	internal class TSQLMultiPartIdentifier
	{
		public string ServerName { get; set; }

		public string DatabaseName { get; set; }

		public string SchemaName { get; set; }

		public string ObjectName { get; set; }
	}
}
