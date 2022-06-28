BEGIN TRAN

/** 1 - Procedura globalna do uruchomienia pozostalych procedur **/
IF OBJECT_ID ('[Processes].[ProcessRunner_v2]' ,'P') IS NOT NULL
DROP PROCEDURE [Processes].[ProcessRunner_v2]
GO

CREATE PROCEDURE [Processes].[ProcessRunner_v2]
(
@StoredProcedureName NVARCHAR(255)
)
AS
BEGIN
IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = @StoredProcedureName)
BEGIN
	DECLARE @SQL NVARCHAR(500);
	SET @SQL = 'EXEC' + ' ' + @StoredProcedureName
	EXEC @SQL

	UPDATE Processes SET LastProcessDate = GETDATE() WHERE StoredProcedureName = @StoredProcedureName
END
ELSE
	RAISERROR(2000, 1, 1, 'Stored procedure not found')
END


/** 2 - Procedura sprawdzajaca terminowosc dostarczania zamowien */
IF OBJECT_ID ('[Processes].[CheckOrdersTerminary]' ,'P') IS NOT NULL
DROP PROCEDURE [Processes].[CheckOrdersTerminary]
GO

CREATE PROCEDURE [Processes].[CheckOrdersTerminary]
AS
BEGIN
BEGIN TRAN 
	DECLARE @CustomersWithAmount TABLE (CustomerId INT, Amount SMALLMONEY, OrderNumbers NVARCHAR(MAX)); -- tablica zwrotow do klientow pieniedzy
	DECLARE @ShouldStart BIT = 0;
	DECLARE @SellerId INT = (SELECT AccountId FROM Session);

	WITH OrderFromSellerToRemove (Id, CustomerId, OrderDate, Number) AS -- CTE zamowienia u danego sprzedawcy ktore jeszcze nie odebrane
	(
		SELECT Id, CustomerId, OrderDate, Number
		FROM Orders O
		WHERE SellerId = @SellerId 
		AND DATEDIFF(DAY, O.OrderDate, GETDATE()) > 5
		AND NOT EXISTS (SELECT State FROM OrderStates OS WHERE OS.OrderId = O.Id AND State = 6)
	)
	SELECT @ShouldStart = COUNT(*) FROM OrderFromSellerToRemove

	IF @ShouldStart > 0
	BEGIN
		--1 - Obliczanie kwot do zwrotu klientom
		INSERT INTO @CustomersWithAmount (CustomerId, Amount)
		SELECT O.CustomerId, SUM(P.PriceBrutto * C.Amount) FROM OrderFromSellerToRemove O
		INNER JOIN OrderCarts C ON C.OrderId = O.Id
		INNER JOIN Products P ON P.Id = C.ProductId
		GROUP BY (O.CustomerId)

		--2 - Dodanie kwot do konta
		UPDATE C SET C.Balance = C.Balance + C1.Amount 
		FROM Customers C
		INNER JOIN @CustomersWithAmount C1 ON C.Id = C1.CustomerId

		--3 - Sklejanie numerow zamowien + usuniecie przecinku na koncu
		UPDATE CWA SET CWA.OrderNumbers = 
		(SELECT Number + ', ' FROM OrderFromSellerToRemove OTR 
		WHERE OTR.CustomerId = CWA.CustomerId 
		FOR XML PATH ('')) -- '' pozwala uniknac znacznikow glownych dla kazdego wiersza
		FROM @CustomersWithAmount CWA

		UPDATE CWA SET CWA.OrderNumbers = SUBSTRING(CWA.OrderNumbers, 1, LEN(CWA.OrderNumbers) - 2) FROM @CustomersWithAmount CWA

		--4 - Dodanie wiadomosci
		INSERT INTO Mails (Caption, Content, Date, SenderId, ReceiverId, MailRelation)
		SELECT N'Zwrot pieniędzy', 'Witam, skoro przesyłka(i) ' + 
		CWA.OrderNumbers +
		N' nie została(y) dostarczona(e) w terminie zwracamy pieniędzy. Pozdrawiamy.',
		GETDATE(),
		@SellerId,
		CWA.CustomerId,
		3 -- Relacja seller -> customer
		FROM @CustomersWithAmount CWA

		--5 - Usuniecie zamowien - automatycznie usuwane OrderCarts
		DELETE FROM Orders WHERE Id IN (SELECT Id FROM OrderFromSellerToRemove)
	END
COMMIT TRAN
END
GO

COMMIT TRAN