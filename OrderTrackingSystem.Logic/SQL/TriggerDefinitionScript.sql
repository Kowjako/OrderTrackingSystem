BEGIN TRAN
/* Trigger sprawdzający czy przy dodaniu użytkownika 
klucz obcy AccountId wskazuje na kontrahenta albo 
producenta, gdy tak nie jest usuwa wstawiony rekord */

IF OBJECT_ID ('TR__Users_UserReference', 'TR') IS NOT NULL
DROP TRIGGER TR__Users_UserReference
GO

CREATE TRIGGER TR__Users_UserReference
ON Users
AFTER INSERT
AS

DECLARE @AccountId INT;
SELECT @AccountId = AccountId FROM INSERTED

IF NOT EXISTS (SELECT 1 FROM Customers WHERE Id = @AccountId 
			   UNION
			   SELECT 1 FROM Sellers WHERE Id = @AccountId)
DELETE FROM Users WHERE AccountId = @AccountId
GO

/* Trigger usuwajacy szablony reklamacyjne przy usunięciu
relacji folder - szablon, bo istnienie szablony zakłada 
przynależność tego szablonu do określonego folderu */

IF OBJECT_ID('TR__ComplaintRelations_OnDelete', 'TR') IS NOT NULL
DROP TRIGGER TR__ComplaintRelations_OnDelete
GO

CREATE TRIGGER TR__ComplaintRelations_OnDelete
ON ComplaintRelations
AFTER DELETE
AS

DECLARE @ComplaintId INT;

SELECT @ComplaintId = ComplaintId FROM DELETED
DELETE FROM ComplaintDefinitions WHERE ID = @ComplaintId
GO

/* Trigger ustawiający na zamówienie że była założona reklamacja
po tym jak użytkownik doda reklamację */
IF OBJECT_ID('TR__ComplaintStates_OnInsert', 'TR') IS NOT NULL
DROP TRIGGER TR__ComplaintStates_OnInsert
GO

CREATE TRIGGER TR__ComplaintStates_OnInsert
ON ComplaintStates
AFTER INSERT
AS

DECLARE @OrderId INT = (SELECT OrderId FROM INSERTED);
DECLARE @ComplaintDefinitionId INT = (SELECT ComplaintDefinitionId FROM INSERTED);

UPDATE Orders SET ComplaintDefinitionId = @ComplaintDefinitionId WHERE Id = @OrderId
GO

COMMIT TRAN
