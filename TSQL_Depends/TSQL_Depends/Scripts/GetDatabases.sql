SELECT
	d.[name] AS [database_name],
	d.collation_name,
	-- https://docs.microsoft.com/en-us/sql/t-sql/functions/collation-functions-collationproperty-transact-sql?view=sql-server-ver15
	CAST(COLLATIONPROPERTY(d.collation_name, 'CodePage') AS int) AS collation_code_page,
	CAST(COLLATIONPROPERTY(d.collation_name, 'ComparisonStyle') AS int) AS collation_comparison_style,
	--Ignore case : 1
	--Ignore accent : 2
	--Ignore Kana : 65536
	--Ignore width : 131072
	CAST(CAST(COLLATIONPROPERTY(d.collation_name, 'ComparisonStyle') AS int) & 1 AS bit) AS collation_ignore_case
FROM
	sys.databases d
WHERE
	d.state_desc = 'ONLINE' AND
	d.[name] NOT IN('master', 'model', 'msdb', 'tempdb', 'Distribution', 'Publication', 'Subscription') AND
	d.[name] NOT LIKE 'ReportServer%'
ORDER BY
	d.[name];