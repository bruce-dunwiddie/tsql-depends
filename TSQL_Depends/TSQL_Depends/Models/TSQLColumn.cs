using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSQL.Depends.Models
{
	internal class TSQLColumn
	{
		public string DatabaseName { get; set; }

		public int ObjectID { get; set; }

		public string Name { get; set; }

		public int ColumnID { get; set; }
	}
}
