--** Script Fondation_Anonymized

SET XACT_ABORT ON
SET NOCOUNT ON



IF(DB_NAME() NOT LIKE '%Foundation')
BEGIN
	RAISERROR ('This is not a foundation database.', 16, 1)
	RETURN
END



BEGIN TRANSACTION


UPDATE dbo.[Provider]
	SET ProviderDetails.modify('replace value of (/Provider/Values/UserName/text())[1] with "azure_44af00f85254c176fe9605819c2ab3e8@azure.com"')
WHERE
	[Provider].[name] = 'SmtpMailProvider' and [Provider].[ImplementationTypeName] = 'SmtpMailProvider'   

UPDATE dbo.[Provider]
	SET ProviderDetails.modify('replace value of (/Provider/Values/Password/text())[1] with "3ye3bSOqMcVi8mr7"')
WHERE
	[Provider].[name] = 'SmtpMailProvider' and [Provider].[ImplementationTypeName] = 'SmtpMailProvider' 

UPDATE dbo.[Provider]
	SET ProviderDetails.modify('replace value of (/Provider/Values/SenderAddress/text())[1] with "Customer COmpany Name Test &lt;no-reply-test@Company.ca&gt;"')
WHERE
	[Provider].[name] = 'SmtpMailProvider' and [Provider].[ImplementationTypeName] = 'SmtpMailProvider' 

UPDATE dbo.[Provider]
	SET ProviderDetails.modify('replace value of (/Provider/Values/TemporaryPassword/text())[1] with "Qwerty123!"')
WHERE
	[Provider].[name] = 'AzureDirectoryProvider' and [Provider].[ImplementationTypeName] = 'AzureDirectoryProvider'   

COMMIT
GO

