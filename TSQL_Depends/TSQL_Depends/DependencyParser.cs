using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSQL.Depends
{
	public class DependencyParser
	{
		public DependencyParser(string connectionString)
		{
			// https://eng.uber.com/queryparser/ explains possibilities out of a similar effort

			// also https://github.com/uber/queryparser/blob/master/FUTURE.md
			// invalid references
			// type checking

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
			}
		}
	}
}
