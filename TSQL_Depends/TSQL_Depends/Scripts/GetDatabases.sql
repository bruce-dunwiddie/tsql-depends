SELECT
	d.[name] AS [database_name],
	d.collation_name
FROM
	sys.databases d
WHERE
	d.state_desc = 'ONLINE' AND
	d.[name] NOT IN('master', 'model', 'msdb', 'tempdb', 'Distribution', 'Publication', 'Subscription') AND
	d.[name] NOT LIKE 'ReportServer%'
ORDER BY
	d.[name];