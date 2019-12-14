# tsql-depends
.Net Library for Parsing Database Object Dependencies from T-SQL Scripts

Available on Nuget, [TSQL.Depends](https://www.nuget.org/packages/TSQL.Depends/).

    Install-Package TSQL.Depends
    
## Example Dependency Parsing

```csharp
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

using TSQL.Depends;

class Program
{
	static void Main(string[] args)
	{
		string AdventureWorks = "Data Source=.;Initial Catalog=AdventureWorks2017;Integrated Security=True";

		// get the definition of the stored proc dbo.uspGetBillOfMaterials from the AdventureWorks database
		string procDefinition = null;

		using (SqlConnection conn = new SqlConnection(AdventureWorks))
		using (SqlCommand getProcDefinition = new SqlCommand(@"
				SELECT
					m.[definition]
				FROM
					sys.sql_modules m
				WHERE
					m.object_id = OBJECT_ID('dbo.uspGetBillOfMaterials');", conn))
		{
			conn.Open();

			procDefinition = getProcDefinition.ExecuteScalar().ToString();
		}

		TSQLDependencyParser parser = new TSQLDependencyParser(AdventureWorks);

		List<TSQLObjectDependency> dependencies = parser.GetDependencies(procDefinition);

		// write out all the object references found within the script
		foreach (var dependency in dependencies)
		{
			Console.WriteLine($"{dependency.Type} {dependency.DatabaseName}.{dependency.SchemaName}.{dependency.Name}");
		}
	}
}
```

```
StoredProcedure AdventureWorks2017.dbo.uspGetBillOfMaterials
Table AdventureWorks2017.Production.BillOfMaterials
Table AdventureWorks2017.Production.Product
```
