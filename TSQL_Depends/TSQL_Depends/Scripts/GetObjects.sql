SELECT
	s.[name] AS [schema_name],
	o.[name] AS [object_name],
	o.[object_id],
	o.[type] AS object_type
FROM
	sys.objects AS o
		INNER JOIN sys.schemas AS s ON
			s.[schema_id] = o.[schema_id]
WHERE
	o.is_ms_shipped = 0 AND
	-- exclude keys and constraints for now
	o.parent_object_id = 0 AND
	NOT EXISTS
	(
		SELECT *
		FROM
			sys.extended_properties AS ep
		WHERE
			ep.major_id = o.[object_id] AND
			ep.minor_id = 0 AND
			ep.class = 1 AND
			ep.[name] = 'microsoft_database_tools_support'
	)
ORDER BY
	s.[name],
	o.[name];