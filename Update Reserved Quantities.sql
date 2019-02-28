SET XACT_ABORT ON


DECLARE @SkuToChange AS NVARCHAR(64) = 'SkuValue'
DECLARE @QuantityToReserve AS INT = IntegerValue


BEGIN TRANSACTION

SELECT 'Before', INVENTORY_QUANTITIES.SKU, INVENTORY_QUANTITIES.ReservedQuantity FROM dbo.INVENTORY_QUANTITIES WHERE INVENTORY_QUANTITIES.SKU = @SkuToChange
UPDATE dbo.INVENTORY_QUANTITIES SET INVENTORY_QUANTITIES.ReservedQuantity = ISNULL(INVENTORY_QUANTITIES.ReservedQuantity, 0) + @QuantityToReserve WHERE INVENTORY_QUANTITIES.SKU = @SkuToChange
SELECT 'After', INVENTORY_QUANTITIES.SKU, INVENTORY_QUANTITIES.ReservedQuantity FROM dbo.INVENTORY_QUANTITIES WHERE INVENTORY_QUANTITIES.SKU = @SkuToChange

COMMIT