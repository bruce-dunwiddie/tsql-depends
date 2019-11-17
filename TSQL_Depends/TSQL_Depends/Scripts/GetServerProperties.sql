SELECT
	s.collation_name AS collation_name,
	-- https://docs.microsoft.com/en-us/sql/t-sql/functions/collation-functions-collationproperty-transact-sql?view=sql-server-ver15
	CAST(COLLATIONPROPERTY(s.collation_name, 'CodePage') AS int) AS collation_code_page,
	CAST(COLLATIONPROPERTY(s.collation_name, 'ComparisonStyle') AS int) AS collation_comparison_style,
	--Ignore case : 1
	--Ignore accent : 2
	--Ignore Kana : 65536
	--Ignore width : 131072
	CAST(CAST(COLLATIONPROPERTY(s.collation_name, 'ComparisonStyle') AS int) & 1 AS bit) AS collation_ignore_case
FROM
	(
		-- https://docs.microsoft.com/en-us/sql/t-sql/functions/serverproperty-transact-sql?view=sql-server-ver15
		SELECT CAST(SERVERPROPERTY('Collation') AS nvarchar(max)) AS collation_name
	) s;