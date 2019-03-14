--** Script Order_Anonymized

SET XACT_ABORT ON
SET NOCOUNT ON


--*************************************************************
-- *** Configuration Section  
-- *** Modify the variables  in this section foy your needs
--*************************************************************



-- START OF CONFIGURATION SECTION

-- ***********************************************************************************************
-- ***********************************************************************************************

-- Did you ran Profile_Anonymized.sql first?

-- Nothing to configure here, 
-- Will run with the choice of one or all customer_id selected in the Profile_Anonymized script

-- END OF CONFIGURATION SECTION

-- ***********************************************************************************************
-- ***********************************************************************************************












-- ********************************
-- *** Start of Program
-- ********************************



--***********************************************************
-- *** If not connected to the right Database, script exits
-- **********************************************************
IF(DB_NAME() NOT LIKE '%Order')
BEGIN
	RAISERROR ('This is not an order database.', 16, 1)
	RETURN
END



--****************************************************************
-- *** If no CustomerProfileToAnonymize table found, script exists
-- ***************************************************************
IF OBJECT_ID('tempdb..##CustomerProfileToAnonymize') IS  NULL
BEGIN
	RAISERROR ('You have to run Profile_Anonymized script first', 16, 1)
	RETURN
END



-- **********************************************************************************
-- *** Update the ORDER table to anonymized colums related to customer(s) selected
-- **********************************************************************************
UPDATE [ORDER] SET
	[ORDER].Email = CONCAT(CustomerProfileToAnonymize.CustomerEntityName, ' ', [ORDER].OrderNumber, CustomerProfileToAnonymize.AnonymizedFakeEmailSuffixe)
	, [ORDER].CustomerPhone = CustomerProfileToAnonymize.AnonymizedPhoneNumber
	, [ORDER].CustomerName = CONCAT(CustomerProfileToAnonymize.CustomerEntityName, ' ', [ORDER].OrderNumber)

FROM
	dbo.[ORDER]

INNER JOIN
	##CustomerProfileToAnonymize CustomerProfileToAnonymize
		ON CustomerProfileToAnonymize.Id = [ORDER].Customer_Id
OPTION (RECOMPILE)




-- *****************************************************************************************
-- *** Update the SHIPMENTADDRESS table to anonymized colums related to customer(s) selected
-- *****************************************************************************************
UPDATE SHIPMENTADDRESS SET
	SHIPMENTADDRESS.LastName = CONCAT(CustomerProfileToAnonymize.CustomerEntityName, ' ', SHIPMENTADDRESS.TransactionOrderNumber)
	, SHIPMENTADDRESS.FirstName = CONCAT(CustomerProfileToAnonymize.CustomerEntityName, ' ', SHIPMENTADDRESS.TransactionOrderNumber)
	, SHIPMENTADDRESS.PhoneNumber = CustomerProfileToAnonymize.AnonymizedPhoneNumber

FROM
	dbo.SHIPMENTADDRESS

INNER JOIN
	dbo.[ORDER]
		ON [ORDER].TransactionOrderNumber = SHIPMENTADDRESS.TransactionOrderNumber

INNER JOIN
	##CustomerProfileToAnonymize CustomerProfileToAnonymize
		ON CustomerProfileToAnonymize.Id = [ORDER].Customer_Id
OPTION (RECOMPILE)




-- ****************************************************************************************
-- *** Update the PAYMENTADDRESS table to anonymized colums related to customer(s) selected
-- ****************************************************************************************
UPDATE PAYMENTADDRESS SET
	PAYMENTADDRESS.LastName = CONCAT(CustomerProfileToAnonymize.CustomerEntityName, ' ', PAYMENTADDRESS.TransactionOrderNumber)
	, PAYMENTADDRESS.FirstName = CONCAT(CustomerProfileToAnonymize.CustomerEntityName, ' ', PAYMENTADDRESS.TransactionOrderNumber)
	, PAYMENTADDRESS.PhoneNumber = CustomerProfileToAnonymize.AnonymizedPhoneNumber

FROM
	dbo.PAYMENTADDRESS

INNER JOIN
	dbo.[ORDER]
		ON [ORDER].TransactionOrderNumber = PAYMENTADDRESS.TransactionOrderNumber

INNER JOIN
	##CustomerProfileToAnonymize CustomerProfileToAnonymize
		ON CustomerProfileToAnonymize.Id = [ORDER].Customer_Id




-- *************************************************************
-- *** The temporary table CustomerProfileToAnonymize is dropped
-- *** It is no longger needed
-- *************************************************************
IF OBJECT_ID('tempdb..##CustomerProfileToAnonymize') IS NOT NULL
	DROP TABLE ##CustomerProfileToAnonymize
GO
