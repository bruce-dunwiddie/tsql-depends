SELECT
	DB_NAME() AS [database_name],

	/*

	https://docs.microsoft.com/en-us/previous-versions/sql/sql-server-2008-r2/ms190387%28v%3dsql.105%29

	Default Schemas:

	To resolve the names of securables that are not fully qualified names,
	SQL Server 2000 uses name resolution to check the schema owned by the
	calling database user and the schema owned by dbo.

	Beginning with SQL Server 2005, each user has a default schema. The
	default schema can be set and changed by using the DEFAULT_SCHEMA
	option of CREATE USER or ALTER USER. If DEFAULT_SCHEMA is left undefined,
	the database user will have dbo as its default schema. 

	https://docs.microsoft.com/en-us/sql/t-sql/functions/schema-name-transact-sql?view=sql-server-ver15

	Syntax:

		SCHEMA_NAME ( [ schema_id ] )  

	Arguments:

	Term		Definition

	schema_id	The ID of the schema. schema_id is an int. If schema_id is not defined, 
				SCHEMA_NAME will return the name of the default schema of the caller.

	*/

	SCHEMA_NAME() AS default_schema_name;