using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSQL.Depends.Models
{
	internal class TSQLDatabase
	{
		public List<TSQLObject> Objects;
		public List<TSQLColumn> Columns;
	}
}
