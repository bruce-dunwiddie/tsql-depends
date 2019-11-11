SELECT
	c.[object_id],
	c.[name] AS column_name,
	c.column_id,
	c.collation_name
FROM
	sys.columns AS c
WHERE
	NOT EXISTS
	(
		SELECT *
		FROM
			sys.objects AS o
		WHERE
			o.[object_id] = c.[object_id] AND
			o.is_ms_shipped = 1
	) AND
	NOT EXISTS
	(
		SELECT *
		FROM
			sys.extended_properties AS ep
		WHERE
			ep.major_id = c.[object_id] AND
			ep.minor_id = 0 AND
			ep.class = 1 AND
			ep.[name] = 'microsoft_database_tools_support'
	)
ORDER BY
	c.[object_id], 
	c.column_id;