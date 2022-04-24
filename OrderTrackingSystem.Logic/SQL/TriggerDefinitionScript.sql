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

/* Trigger sprawdzający czy przy dodaniu maila
klucz obcy SenderId oraz ReceiverId wskazuje na kontrahenta
albo producenta w zależności od ustawionej flagi kireunku
gdy tak nie jest usuwa wstawiony rekord */

IF OBJECT_ID ('TR__Mails_ReceiverSender', 'TR') IS NOT NULL
DROP TRIGGER TR__Mails_ReceiverSender
GO

CREATE TRIGGER TR__Mails_ReceiverSender
ON Mails
AFTER INSERT
AS

DECLARE @ReceiverId INT;
DECLARE @SenderId INT;
DECLARE @IsFromCustomer BIT;
SELECT @ReceiverId = ReceiverId, @SenderId = SenderId, @IsFromCustomer = IsFromCustomer FROM INSERTED

IF @IsFromCustomer = 1 
BEGIN
IF (SELECT COUNT(Id) FROM
   (SELECT Id FROM Customers WHERE Id = @SenderId 
	UNION
	SELECT Id FROM Sellers WHERE Id = @ReceiverId) AS TBL) < 2
DELETE FROM Mails WHERE Id = (SELECT Id FROM INSERTED)
END;
ELSE
IF (SELECT COUNT(Id) FROM
   (SELECT Id FROM Customers WHERE Id = @ReceiverId 
	UNION
	SELECT Id FROM Sellers WHERE Id = @SenderId) AS TBL) < 2
DELETE FROM Mails WHERE Id = (SELECT Id FROM INSERTED)
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

COMMIT TRAN
