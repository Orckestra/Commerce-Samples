SET NOCOUNT ON

-- Backup all databases already backuped in Azure.
-- Script is mirroring aware
-- Do not run the script within the same second otherwise backupschain could be mixed up and unuseable


-- The script won't work if any of the following is true:
-- - The database has never been backup yet to Azure using smart backups
-- - The database had a guid fork and no backup was yet done to Azure using smart backups
-- - Smart backup configuration is not set properly
-- - Azure Data storage does not exist
-- - Database recovery model is not set to FULL


------------------------------------------------------
-- Start configuration section

DECLARE @IsFullBackup AS BIT = 0 -- 0 = Log backup, 1 = Full backup

-- End configuration section
------------------------------------------------------


-- Validation

IF (@IsFullBackup IS NULL)
BEGIN
	RAISERROR ('Configuration @IsFullBackup cannot be NULL. Allowed configuration: 0 = Log backup, 1 = Full backup.', 16, 1)
	RETURN
END



-- Do not change the remainder configuration

DECLARE @DebugMode AS BIT = 0

DECLARE @StorageURLPrefix AS NVARCHAR(MAX) = 'https://'
DECLARE @StorageURLSuffixe AS NVARCHAR(MAX) = '.blob.core.windows.net/'

DECLARE @BackupFileExtension AS NVARCHAR(3) = 'log'
DECLARE @BackupCommandType AS NVARCHAR(8) = 'LOG'
DECLARE @BackupHeaderNameBackupType AS NVARCHAR(4) = 'LOG'

DECLARE @QueryToExecuteBackup AS NVARCHAR(MAX)
DECLARE @CurrentDateForBackup AS DATETIME2(0) = GETDATE()
DECLARE @FormattedCurrentDateForBackup AS NVARCHAR(25) = FORMAT(@CurrentDateForBackup, 'yyyyMMddHHmmss')

DECLARE @GuidLength AS INT = 32



IF (@IsFullBackup = 1)
BEGIN
	SET @BackupFileExtension = 'bak'
	SET @BackupCommandType = 'DATABASE'
	SET @BackupHeaderNameBackupType = 'FULL'
END

SELECT
	@QueryToExecuteBackup = CONCAT(@QueryToExecuteBackup, 
'
BACKUP ', @BackupCommandType, ' [', frmGetDBUrl.[name], '] TO  URL = N''', frmGetDBUrl.FullFileNameToUrl ,''' WITH  CREDENTIAL = N''AutoBackup_Credential'' , NOFORMAT, NOINIT,  NAME = N''', frmGetDBUrl.[name], '-', @BackupHeaderNameBackupType, ' Database Backup'', NOSKIP, NOREWIND, NOUNLOAD, COMPRESSION,  STATS = 10, CHECKSUM')

FROM (

	SELECT
		databases.[name]
		, CONCAT(@StorageURLPrefix, caGetLastestBackupInfo.AzureStorageKey, @StorageURLSuffixe, LOWER(@@SERVERNAME), '-mssqlserver/', databases.[name], '_', caGetLastestBackupInfo.LastestBackupGUID, '_', @FormattedCurrentDateForBackup, LEFT(RIGHT(SYSDATETIMEOFFSET(), 6), 3), '.', @BackupFileExtension) AS 'FullFileNameToUrl'

	FROM
		sys.databases

	CROSS APPLY (

		SELECT TOP 1
			SUBSTRING(backupmediafamily.physical_device_name, PATINDEX(CONCAT('%', backupset.database_name, '_%'), backupmediafamily.physical_device_name) + LEN(backupset.database_name) + 1, 32) AS 'LastestBackupGUID'
			, SUBSTRING(backupmediafamily.physical_device_name, LEN(@StorageURLPrefix) + 1, PATINDEX(CONCAT('%', @StorageURLSuffixe, '%'), backupmediafamily.physical_device_name) - LEN(@StorageURLPrefix) - 1) AS 'AzureStorageKey'

		FROM
			msdb.dbo.backupmediafamily

		INNER JOIN msdb.dbo.backupset
			ON msdb.dbo.backupmediafamily.media_set_id = msdb.dbo.backupset.media_set_id  

		WHERE
			backupset.database_name = databases.[name]
			AND backupmediafamily.physical_device_name LIKE CONCAT(@StorageURLPrefix, '%')
			AND backupset.is_copy_only = 0

		ORDER BY
			backupset.backup_finish_date DESC

	) caGetLastestBackupInfo
	
	WHERE
		databases.state_desc = 'ONLINE'
		AND databases.recovery_model_desc = 'FULL'

) frmGetDBUrl
OPTION (RECOMPILE)


PRINT @QueryToExecuteBackup
PRINT ''
IF (@DebugMode = 0) EXEC sp_executeSQL @QueryToExecuteBackup

