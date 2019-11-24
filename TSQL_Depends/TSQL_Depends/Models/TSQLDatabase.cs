using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSQL.Depends.Models
{
	internal class TSQLDatabase
	{
		public string Name { get; set; }

		public string Collation { get; set; }

		public int CollationCodePage { get; set; }

		public bool CollationIgnoreCase { get; set; }

		public List<TSQLObject> Objects { get; set; }

		public List<TSQLColumn> Columns { get; set; }

		public List<TSQLSynonym> Synonyms { get; set; }
	}
}
