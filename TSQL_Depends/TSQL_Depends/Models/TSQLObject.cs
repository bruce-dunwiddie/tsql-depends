﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSQL.Depends.Models
{
	internal class TSQLObject
	{
		public string ServerName { get; set; }

		public string DatabaseName { get; set; }

		public string SchemaName { get; set; }

		public string Name { get; set; }

		public int ObjectID { get; set; }

		public TSQLOfficialObjectType Type { get; set; }
	}
}
