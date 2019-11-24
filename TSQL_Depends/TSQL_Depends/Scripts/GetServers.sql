SELECT
	s.[name] AS [server_name],
	s.is_linked
FROM
	sys.servers AS s
WHERE
	s.is_system = 0 AND
	s.is_subscriber = 0
ORDER BY
	s.[name];