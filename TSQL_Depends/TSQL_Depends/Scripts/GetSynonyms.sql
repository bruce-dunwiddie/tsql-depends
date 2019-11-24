SELECT 
    s.[name] AS [schema_name],
	sy.[name] AS [synonym_name],
	sy.[object_id],
    sy.base_object_name
FROM 
    sys.synonyms sy
		INNER JOIN sys.schemas AS s ON
			s.[schema_id] = sy.[schema_id]
ORDER BY 
    s.[name],
	sy.[name];