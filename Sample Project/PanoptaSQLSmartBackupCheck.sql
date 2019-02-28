DECLARE @TimeBeforeExpiration AS DATETIME2(3) = DATEADD(HOUR, -24, GETDATE())

SELECT
	databases.name AS 'DatabaseName'
	, CASE WHEN oaBackupExist.Exist = 1 THEN 1 ELSE 0 END AS 'UseableBackupExist'

FROM
	sys.databases

OUTER APPLY (

	SELECT TOP 1
		1 AS 'Exist'
		, fab.backup_finish_date

	FROM
		msdb.smart_admin.fn_available_backups(databases.name) fab

	WHERE
		fab.backup_finish_date >= @TimeBeforeExpiration

	ORDER BY
		fab.backup_finish_date DESC

) oaGetBackupStatus

OUTER APPLY (

	SELECT TOP 1
		1 AS 'Exist'
		, smart_backup_files.backup_finish_date

	FROM
		msdb.dbo.smart_backup_files

	WHERE
		smart_backup_files.[status] = 'A'
		AND smart_backup_files.database_name = databases.name
		AND smart_backup_files.backup_finish_date >= @TimeBeforeExpiration

	ORDER BY
		smart_backup_files.last_modified_utc DESC

) oaBackupExist

WHERE
	databases.state_desc = 'ONLINE'
	AND (
		databases.[name] LIKE '%foundation' OR
		databases.[name] LIKE '%marketing' OR
		databases.[name] LIKE '%membership' OR
		databases.[name] LIKE '%messaging' OR
		databases.[name] LIKE '%order' OR
		databases.[name] LIKE '%pickandpack' OR
		databases.[name] LIKE '%pnp' OR
		databases.[name] LIKE '%product' OR
		databases.[name] LIKE '%profile' OR
		databases.[name] LIKE '%Archive' OR
		databases.[name] LIKE '%Reporting' OR
		databases.[name] LIKE '%DataWarehouse'
	)

ORDER BY
	databases.[name]