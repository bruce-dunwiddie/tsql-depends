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
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();

				//SELECT
				//	d.database_id,
				//	d.name
				//FROM
				//	sys.databases d
				//WHERE
				//	d.state_desc = 'ONLINE' AND
				//	d.name NOT IN ('master', 'model', 'msdb', 'tempdb', 'Distribution', 'Publication', 'Subscription') AND
				//	d.name NOT LIKE 'ReportServer%'
				//ORDER BY
				//	d.name;

				//SELECT
				//	o.[name],
				//	o.[object_id],
				//	o.principal_id,
				//	o.[schema_id],
				//	o.parent_object_id,
				//	o.[type]
				//FROM
				//	sys.objects AS o
				//WHERE
				//	o.is_ms_shipped = 0 AND
				//	NOT EXISTS
				//	(
				//		SELECT *
				//		FROM
				//			sys.extended_properties AS ep
				//		WHERE
				//			ep.major_id = o.[object_id] AND
				//			ep.minor_id = 0 AND
				//			ep.class = 1 AND
				//			ep.[name] = 'microsoft_database_tools_support'
				//	);

				//SELECT c.[object_id], c.[name]
				//FROM            sys.columns AS c
				//WHERE(NOT EXISTS
				//(SELECT*
				//FROM            sys.objects AS o
				//WHERE        (o.[object_id] = c.[object_id]) AND(o.is_ms_shipped = 1))) AND
				//(NOT EXISTS
				//(SELECT*
				//FROM            sys.extended_properties AS ep
				//WHERE        (ep.major_id = c.[object_id]) AND(ep.minor_id = 0) AND(ep.class = 1) AND([name] = 'microsoft_database_tools_support')))
				//ORDER BY c.[object_id], column_id;
			}
		}
	}
}
