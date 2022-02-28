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
albo producenta, gdy tak nie jest usuwa wstawiony rekord */

IF OBJECT_ID ('TR__Mails_ReceiverSender', 'TR') IS NOT NULL
DROP TRIGGER TR__Mails_ReceiverSender
GO

CREATE TRIGGER TR__Mails_ReceiverSender
ON Mails
AFTER INSERT
AS

DECLARE @ReceiverId INT;
DECLARE @SenderId INT;
SELECT @ReceiverId = ReceiverId, @SenderId = SenderId FROM INSERTED

IF (SELECT COUNT(Id) FROM
			   (SELECT Id FROM Customers WHERE Id IN (@ReceiverId, @SenderId) 
			   UNION
			   SELECT Id FROM Sellers WHERE Id IN (@ReceiverId, @SenderId)) AS TBL) <> 2
DELETE FROM Mails WHERE Id = (SELECT Id FROM INSERTED)
GO
