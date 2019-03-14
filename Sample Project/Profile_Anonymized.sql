--** Script Profile_Anonymized


SET XACT_ABORT ON
SET NOCOUNT ON


-- *************************************************************
-- *** Configuration Section  
-- *** Modify the variables  in this section foy your needs
-- *************************************************************



-- START OF CONFIGURATION SECTION

-- ***********************************************************************************************
-- ***********************************************************************************************

-- *************************************************************
-- *** Choose 1 or All customer to be Anonymize ***
-- *** 1 = Specify 1 Customer
-- *** 2 = All Customers
-- *************************************************************

DECLARE @CustomerSelection INT = 1




-- *************************************************************
-- *** Specify the ID of the customer to be Anonymize 
-- *** If you don't have the Customer_ID, you can do select the Customer_ID from the table Customer  
-- *** including the info you have in the  search criteria of the where clause 
-- *** ex: select Customer_Id from Customer where email='   ' 
-- *** or: select Customer_Id from Customer where FirstName='   ' and LastName= '  ' 
-- 
-- If the script is halted because of a deadlock or locking, re-run it
--
-- At least only one row must be needed for a particular customer
-- Make sure to correctly uniquely identify the customer
-- Fields requires to be precise, no wildcard accepted
-- 
-- If you want all customer to be anonymized, you must enter the value 2 for
-- the variable @CustomerSelection just one step up of this scipt
-- The variable @Customer_IdToAnonymize won't be taken into account. 
-- *************************************************************

DECLARE @Customer_IdToAnonymize INT = 




-- END OF CONFIGURATION SECTION

-- ***********************************************************************************************
-- ***********************************************************************************************








-- ****************************************************************
-- Values that will be use for the columns to be anonymized 
-- The following customer fields are anonymized:
-- 'Username', 'FirstName', 'LastName', 'PhoneNumber', 'Email', 
-- 'CellNumber', 'PhoneNumberWork', 'FaxNumber'
-- ****************************************************************
DECLARE @AnonymizedPhoneNumber AS VARCHAR(32) = '555-555-5555'
DECLARE @AnonymizedFakeEmailSuffixe AS NVARCHAR(64) = '@orckestrafake.com'
DECLARE @AnonymizedAuditNewValueSuffixe AS NVARCHAR(128) = 'Anonymized Audit NewValue'
DECLARE @AnonymizedAuditOldValueSuffixe AS NVARCHAR(128) = 'Anonymized Audit OldValue'
DECLARE @AnonymizedCustomerPrefix AS NVARCHAR(16) = @CustomerEntityName


DECLARE @DisableProfileChanges INT = 1



-- ***************************************************
-- *** The name of the entity for Customer
-- ***************************************************
DECLARE @CustomerEntityName AS SYSNAME = 'CUSTOMER'


-- ***************************************************
-- *** The name of the entity for Address
-- ***************************************************
DECLARE @AddressEntityName AS SYSNAME = 'ADDRESS'









-- ********************************
-- *** Start of Program
-- ********************************


--***********************************************************
-- *** If not connected to the right Database, script exits
-- **********************************************************
IF(DB_NAME() NOT LIKE '%Profile')
BEGIN
	RAISERROR ('This is not a profile database.', 16, 1)
	RETURN
END


-- ********************************************************************
-- *** If the value of @CustomerSelection is not valid, exit the script
-- ********************************************************************
IF (@CustomerSelection != 2 AND @CustomerSelection ! = 1)
BEGIN
	RAISERROR ('The value enter for @CustomerSelection is not valid', 16, 1)
	RETURN
END



-- *************************************************************
-- *** Creating the temporary table(##CustomerProfileToAnonymize)  
-- *** to contain the customer_ID(s)  to be Anonymize
-- *** The columns to be anonymized are added to the table
-- *** The table is dropped before creating it 
-- *************************************************************

IF OBJECT_ID('tempdb..##CustomerProfileToAnonymize') IS NOT NULL
	DROP TABLE ##CustomerProfileToAnonymize
	
	
CREATE TABLE ##CustomerProfileToAnonymize (
	Customer_Id INT NOT NULL
	, Id UNIQUEIDENTIFIER NOT NULL
	, UserName NVARCHAR(256) NULL
	, FirstName NVARCHAR(64) NULL
	, LastName NVARCHAR(64) NULL
	, Email NVARCHAR(256) NULL
	, PhoneNumber VARCHAR(32) NULL
	, CellNumber VARCHAR(32) NULL
	, PhoneNumberWork VARCHAR(32) NULL
	, FaxNumber VARCHAR(32) NULL
	, AnonymizedPhoneNumber VARCHAR(32) NOT NULL
	, AnonymizedFakeEmailSuffixe NVARCHAR(64) NOT NULL
	, CustomerEntityName SYSNAME NOT NULL
	, PRIMARY KEY CLUSTERED (Customer_Id)
	, UNIQUE NONCLUSTERED (Id)
)


-- *************************************************************
-- *** Inserting in a temporary table(##CustomerProfileToAnonymize)  
-- *** the customer ID(s)  to be Anonymize
-- *************************************************************

BEGIN
    INSERT INTO ##CustomerProfileToAnonymize
	SELECT
		CUSTOMER.Customer_Id
		, CUSTOMER.Id
		, CUSTOMER.UserName
		, CUSTOMER.FirstName
		, CUSTOMER.LastName
		, CUSTOMER.Email
		, CUSTOMER.PhoneNumber
		, CUSTOMER.CellNumber
		, CUSTOMER.PhoneNumberWork
		, CUSTOMER.FaxNumber
		, @AnonymizedPhoneNumber
		, @AnonymizedFakeEmailSuffixe
		, @CustomerEntityName

	FROM
		dbo.CUSTOMER

	WHERE
		(CUSTOMER.CUSTOMER_Id = @Customer_IdToAnonymize AND @CustomerSelection = 1)
		OR
		@CustomerSelection = 2
	OPTION(RECOMPILE)
END






--***********************************************************
-- *** If no Customer found for the selection, script exists
-- **********************************************************
IF NOT EXISTS (select 1 from ##CustomerProfileToAnonymize)
BEGIN
	RAISERROR ('No Customer selected or database does not contain any customer', 16, 1)
	RETURN
END


DECLARE @QueryToExecute AS NVARCHAR(MAX)
DECLARE @NumberOfRowChanged INT = 1;
DECLARE @CurrentMessageToDisplay AS NVARCHAR(MAX);




--***********************************************************
-- *** Disabling triggers on Customer and Address entities
-- **********************************************************
IF (@DisableProfileChanges = 1)
BEGIN
	SET @QueryToExecute = CONCAT('DISABLE TRIGGER ALL ON dbo.[', @AddressEntityName, ']')
	EXEC sp_executeSQL @QueryToExecute

	SET @QueryToExecute = CONCAT('DISABLE TRIGGER ALL ON dbo.', @CustomerEntityName)
	EXEC sp_executeSQL @QueryToExecute
END





--**********************************************************************************
-- *** Update the Address table to anonymized columns related to customer(s) selected
-- *********************************************************************************
UPDATE [ADDRESS]
	SET [ADDRESS].LastName = SUBSTRING(CONCAT(@AnonymizedCustomerPrefix, [ADDRESS].Address_Id), 0, 64)
	, [ADDRESS].FirstName = SUBSTRING(CONCAT(@AnonymizedCustomerPrefix, [ADDRESS].Address_Id), 0, 64)
	, [ADDRESS].PhoneNumber = CustomerProfileToAnonymize.AnonymizedPhoneNumber
	
FROM
	dbo.[ADDRESS]

INNER JOIN
	dbo.RELATIONSHIP
		ON RELATIONSHIP.Child_Id = [ADDRESS].Address_Id

INNER JOIN
	dbo.ENTITY parentEntity
		ON parentEntity.Entity_Id = RELATIONSHIP.ParentEntity_Id

INNER JOIN
	dbo.ENTITY childEntity
		ON childEntity.Entity_Id = RELATIONSHIP.ChildEntity_Id

INNER JOIN
	##CustomerProfileToAnonymize CustomerProfileToAnonymize
		ON CustomerProfileToAnonymize.Customer_Id = RELATIONSHIP.Parent_Id

WHERE
	parentEntity.EntityName = @CustomerEntityName
	AND childEntity.EntityName = @AddressEntityName
OPTION (RECOMPILE)


SET @NumberOfRowChanged = @@ROWCOUNT;

SET @CurrentMessageToDisplay = CONCAT('Number of sanitanized address rows: ', @NumberOfRowChanged)
RAISERROR (@CurrentMessageToDisplay, 0, 1) WITH NOWAIT





--**********************************************************************************
-- *** Update the Customer table to anonymized colums related to customer(s) selected
-- *********************************************************************************
UPDATE CUSTOMER
	SET CUSTOMER.Email = CONCAT(@AnonymizedCustomerPrefix, CUSTOMER.Customer_Id, CustomerProfileToAnonymize.AnonymizedFakeEmailSuffixe)
	, CUSTOMER.Username = CONCAT(@AnonymizedCustomerPrefix, CUSTOMER.Customer_Id)
	, CUSTOMER.FirstName = CONCAT(@AnonymizedCustomerPrefix, CUSTOMER.Customer_Id)
	, CUSTOMER.LastName = CONCAT(@AnonymizedCustomerPrefix, CUSTOMER.Customer_Id)
	, CUSTOMER.PhoneNumber = CustomerProfileToAnonymize.AnonymizedPhoneNumber
	, CUSTOMER.CellNumber = CustomerProfileToAnonymize.AnonymizedPhoneNumber
	, CUSTOMER.PhoneNumberWork = CustomerProfileToAnonymize.AnonymizedPhoneNumber
	, CUSTOMER.FaxNumber = CustomerProfileToAnonymize.AnonymizedPhoneNumber

FROM
	dbo.CUSTOMER

INNER JOIN
	##CustomerProfileToAnonymize CustomerProfileToAnonymize
		ON CustomerProfileToAnonymize.Customer_Id = CUSTOMER.Customer_Id

OPTION (RECOMPILE)


SET @NumberOfRowChanged = @@ROWCOUNT;
SET @CurrentMessageToDisplay = CONCAT('Number of sanitanized customer rows: ', @NumberOfRowChanged)
RAISERROR (@CurrentMessageToDisplay, 0, 1) WITH NOWAIT





--**********************************************************************************
-- Update the Audit table to anonymized colums related to the customer EntityName
-- audit changes of the selected customer(s)
-- Filter by the columnName :
-- 'Username', 'FirstName', 'LastName', 'PhoneNumber', 'Email', 
-- 'CellNumber', 'PhoneNumberWork', 'FaxNumber'
--**********************************************************************************
UPDATE [AUDIT] SET 
	[AUDIT].OldValue = CASE WHEN [AUDIT].Operation IN ('U', 'D') THEN CONCAT([AUDIT].TableName, ': ', @AnonymizedAuditNewValueSuffixe) ELSE NULL END
	, [AUDIT].NewValue = CASE WHEN [AUDIT].Operation IN ('I', 'U') THEN CONCAT([AUDIT].TableName, ': ', @AnonymizedAuditOldValueSuffixe) ELSE NULL END
	
FROM
	[Audit].[AUDIT]

INNER JOIN
	##CustomerProfileToAnonymize CustomerProfileToAnonymize
		ON CustomerProfileToAnonymize.Id = [AUDIT].[Entity_Id]

WHERE
	[AUDIT].TableName = @CustomerEntityName
	AND [AUDIT].ColumnName IN ('Username', 'FirstName', 'LastName', 'PhoneNumber', 'Email', 'CellNumber', 'PhoneNumberWork', 'FaxNumber')
OPTION (RECOMPILE)


SET @NumberOfRowChanged = @@ROWCOUNT;

SET @CurrentMessageToDisplay = CONCAT('Number of sanitanized customer audit rows: ', @NumberOfRowChanged)
RAISERROR (@CurrentMessageToDisplay, 0, 1) WITH NOWAIT




--*******************************************************************************************
-- Update the Audit table to anonymized colums related to the Address table 
-- audit changes of the selected customer(s)
-- Filter by the columnName :
-- 'Username', 'FirstName', 'LastName', 'PhoneNumber', 'Email', 
-- 'CellNumber', 'PhoneNumberWork', 'FaxNumber'
--*******************************************************************************************
UPDATE [AUDIT] SET 
	[AUDIT].OldValue = CASE WHEN [AUDIT].Operation IN ('U', 'D') THEN CONCAT([AUDIT].TableName, ': ', @AnonymizedAuditNewValueSuffixe) ELSE NULL END
	, [AUDIT].NewValue = CASE WHEN [AUDIT].Operation IN ('I', 'U') THEN CONCAT([AUDIT].TableName, ': ', @AnonymizedAuditOldValueSuffixe) ELSE NULL END
	
FROM
	[Audit].[AUDIT]

INNER JOIN
	dbo.[ADDRESS]
		ON [ADDRESS].Id = [AUDIT].[Entity_Id]

INNER JOIN
	dbo.RELATIONSHIP
		ON RELATIONSHIP.Child_Id = [ADDRESS].Address_Id

INNER JOIN
	dbo.ENTITY parentEntity
		ON parentEntity.Entity_Id = RELATIONSHIP.ParentEntity_Id

INNER JOIN
	dbo.ENTITY childEntity
		ON childEntity.Entity_Id = RELATIONSHIP.ChildEntity_Id

INNER JOIN
	##CustomerProfileToAnonymize CustomerProfileToAnonymize
		ON CustomerProfileToAnonymize.Customer_Id = RELATIONSHIP.Parent_Id

WHERE
	parentEntity.EntityName = @CustomerEntityName
	AND childEntity.EntityName = @AddressEntityName
	AND [AUDIT].TableName = @AddressEntityName
	AND [AUDIT].ColumnName IN ('FirstName', 'LastName', 'PhoneNumber')
OPTION (RECOMPILE)


SET @NumberOfRowChanged = @@ROWCOUNT;
SET @CurrentMessageToDisplay = CONCAT('Number of sanitanized address audit rows: ', @NumberOfRowChanged)
RAISERROR (@CurrentMessageToDisplay, 0, 1) WITH NOWAIT




--***********************************************************
-- *** RE-Enabling triggers on Customer and Address entities
-- **********************************************************
IF (@DisableProfileChanges = 1)
BEGIN
	SET @QueryToExecute = CONCAT('ENABLE TRIGGER ALL ON dbo.[', @AddressEntityName, ']')
	EXEC sp_executeSQL @QueryToExecute

	SET @QueryToExecute = CONCAT('ENABLE TRIGGER ALL ON dbo.', @CustomerEntityName)
	EXEC sp_executeSQL @QueryToExecute
END

GO
