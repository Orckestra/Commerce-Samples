SET NOCOUNT ON

-- Rebind security, logins & databases users if already deployed by the platform for the former
-- The most probable scenario: from prod to staging

-- This script *does not rebuild the security*



DECLARE @LoginList TABLE (
	LoginName SYSNAME NOT NULL
)

INSERT INTO @LoginList
	SELECT
		sql_logins.[name] 
	
	FROM
		sys.sql_logins
	
	WHERE
		sql_logins.type_desc = 'SQL_LOGIN'
		AND sql_logins.[name] LIKE 'Overture%'



DECLARE @CurrentDatabaseName AS SYSNAME
DECLARE @CurrentLoginName AS SYSNAME
DECLARE @CurrentMessageToDisplay AS NVARCHAR(MAX)
DECLARE @QueryToExecute AS NVARCHAR(MAX)


DECLARE CursorIterateOverDatabases CURSOR LOCAL FAST_FORWARD FOR
	SELECT
		databases.[name]

	FROM
		sys.databases

	WHERE
		databases.[name] LIKE '%_DataWarehouse'
		OR databases.[name] LIKE '%_Archive'
		OR databases.[name] LIKE '%_foundation'
		OR databases.[name] LIKE '%_marketing'
		OR databases.[name] LIKE '%_membership'
		OR databases.[name] LIKE '%_messaging'
		OR databases.[name] LIKE '%_order'
		OR databases.[name] LIKE '%_product'
		OR databases.[name] LIKE '%_profile'

	ORDER BY
		databases.[name]


OPEN CursorIterateOverDatabases

FETCH NEXT FROM CursorIterateOverDatabases
INTO @CurrentDatabaseName


WHILE @@FETCH_STATUS = 0
BEGIN
	
	SET @CurrentMessageToDisplay = CONCAT('Start rebind login on: ', @CurrentDatabaseName)
	RAISERROR (@CurrentMessageToDisplay, 0, 1) WITH NOWAIT




	DECLARE CursorIterateOverLogins CURSOR LOCAL FAST_FORWARD FOR
		SELECT LoginList.LoginName FROM @LoginList LoginList

	OPEN CursorIterateOverLogins

	FETCH NEXT FROM CursorIterateOverLogins
	INTO @CurrentLoginName


	WHILE @@FETCH_STATUS = 0
	BEGIN

		SET @CurrentMessageToDisplay = CONCAT('    Start rebind login: ', @CurrentLoginName)
		RAISERROR (@CurrentMessageToDisplay, 0, 1) WITH NOWAIT


		SET @QueryToExecute = CONCAT('EXEC ', QUOTENAME(@CurrentDatabaseName), '..sp_change_users_login ''Auto_Fix'', ''', @CurrentLoginName, '''')
		EXEC sp_executeSQL @QueryToExecute
		--print @QueryToExecute
	
		SET @CurrentMessageToDisplay = CONCAT('    End rebind login: ', @CurrentLoginName) + CHAR(13)
		RAISERROR (@CurrentMessageToDisplay, 0, 1) WITH NOWAIT

		FETCH NEXT FROM CursorIterateOverLogins
		INTO @CurrentLoginName

	END

	CLOSE CursorIterateOverLogins;
	DEALLOCATE CursorIterateOverLogins;




	SET @CurrentMessageToDisplay = CONCAT('End rebind login on: ', @CurrentDatabaseName) + CHAR(13) + CHAR(13) + CHAR(13)
	RAISERROR (@CurrentMessageToDisplay, 0, 1) WITH NOWAIT

    FETCH NEXT FROM CursorIterateOverDatabases
    INTO @CurrentDatabaseName

END

CLOSE CursorIterateOverDatabases;
DEALLOCATE CursorIterateOverDatabases;
