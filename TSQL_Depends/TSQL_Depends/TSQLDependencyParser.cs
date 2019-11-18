using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TSQL.Depends.Models;

namespace TSQL.Depends
{
	public class TSQLDependencyParser
	{
		public TSQLDependencyParser(
			string connectionString)
		{
			// https://eng.uber.com/queryparser/ explains possibilities out of a similar effort

			// also https://github.com/uber/queryparser/blob/master/FUTURE.md
			// invalid references
			// type checking

			ModelBuilder builder = new ModelBuilder(
				connectionString);

			TSQLReferenceResolver resolver = new TSQLReferenceResolver(
				builder.GetServerProperties(),
				builder.GetSessionProperties(),
				builder.GetServers(),
				builder.GetDatabases());
		}
	}
}
