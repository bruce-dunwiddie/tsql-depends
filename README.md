# tsql-depends
.Net Library for Parsing Database Object Dependencies from T-SQL Scripts

Available on Nuget, [TSQL.Depends](https://www.nuget.org/packages/TSQL.Depends/).

    Install-Package TSQL.Depends
    
[![NuGet](https://img.shields.io/nuget/dt/TSQL.Depends.svg)](https://www.nuget.org/packages/TSQL.Depends/)

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)

## Current Features
- Full T-SQL token implementation.
- Ignores comments.
- Only returns resolved objects.
- Resolves objects using database, server, and connection settings for case sensitivity and default schema.
- Resolves using 1, 2, or 3 part names.
- 4 part names are assumed valid if matching an existing linked server.
- .Net Framework 4.0+ and .Net Core 2.0+ compatible.
    
## Example Dependency Parsing

dbo.uspGetBillOfMaterials:
```sql
CREATE PROCEDURE [dbo].[uspGetBillOfMaterials]
    @StartProductID [int],
    @CheckDate [datetime]
AS
BEGIN
    SET NOCOUNT ON;

    -- Use recursive query to generate a multi-level Bill of Material (i.e. all level 1 
    -- components of a level 0 assembly, all level 2 components of a level 1 assembly)
    -- The CheckDate eliminates any components that are no longer used in the product on this date.
    WITH [BOM_cte]([ProductAssemblyID], [ComponentID], [ComponentDesc], [PerAssemblyQty], [StandardCost], [ListPrice], [BOMLevel], [RecursionLevel]) -- CTE name and columns
    AS (
        SELECT b.[ProductAssemblyID], b.[ComponentID], p.[Name], b.[PerAssemblyQty], p.[StandardCost], p.[ListPrice], b.[BOMLevel], 0 -- Get the initial list of components for the bike assembly
        FROM [Production].[BillOfMaterials] b
            INNER JOIN [Production].[Product] p 
            ON b.[ComponentID] = p.[ProductID] 
        WHERE b.[ProductAssemblyID] = @StartProductID 
            AND @CheckDate >= b.[StartDate] 
            AND @CheckDate <= ISNULL(b.[EndDate], @CheckDate)
        UNION ALL
        SELECT b.[ProductAssemblyID], b.[ComponentID], p.[Name], b.[PerAssemblyQty], p.[StandardCost], p.[ListPrice], b.[BOMLevel], [RecursionLevel] + 1 -- Join recursive member to anchor
        FROM [BOM_cte] cte
            INNER JOIN [Production].[BillOfMaterials] b 
            ON b.[ProductAssemblyID] = cte.[ComponentID]
            INNER JOIN [Production].[Product] p 
            ON b.[ComponentID] = p.[ProductID] 
        WHERE @CheckDate >= b.[StartDate] 
            AND @CheckDate <= ISNULL(b.[EndDate], @CheckDate)
        )
    -- Outer select from the CTE
    SELECT b.[ProductAssemblyID], b.[ComponentID], b.[ComponentDesc], SUM(b.[PerAssemblyQty]) AS [TotalQuantity] , b.[StandardCost], b.[ListPrice], b.[BOMLevel], b.[RecursionLevel]
    FROM [BOM_cte] b
    GROUP BY b.[ComponentID], b.[ComponentDesc], b.[ProductAssemblyID], b.[BOMLevel], b.[RecursionLevel], b.[StandardCost], b.[ListPrice]
    ORDER BY b.[BOMLevel], b.[ProductAssemblyID], b.[ComponentID]
    OPTION (MAXRECURSION 25) 
END;
```

Code:
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

		// for this example, we'll get the definition of the stored procedure dbo.uspGetBillOfMaterials
		// directly from the AdventureWorks database
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

		// pass the connection string into the parser to enable dependency resolution
		TSQLDependencyParser parser = new TSQLDependencyParser(AdventureWorks);

		// pass the script in as a string to be parsed for dependencies
		List<TSQLObjectDependency> dependencies = parser.GetDependencies(procDefinition);

		// write out all the object references found within the script
		foreach (var dependency in dependencies)
		{
			// each dependency returns a Type and a fully qualified 3 part name
			Console.WriteLine($"{dependency.Type} {dependency.DatabaseName}.{dependency.SchemaName}.{dependency.Name}");
		}
	}
}
```

Result:
```
StoredProcedure AdventureWorks2017.dbo.uspGetBillOfMaterials
Table AdventureWorks2017.Production.BillOfMaterials
Table AdventureWorks2017.Production.Product
```

## Future Additions
- Resolve dependencies down to column level.
