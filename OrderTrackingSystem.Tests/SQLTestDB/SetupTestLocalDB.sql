USE [master]
GO

----------------------------------------------------BAZOWY SKRYPT-----------------------------------------------
IF EXISTS(SELECT * FROM master.sys.databases WHERE name='OrderTrackingSystem_Test')
DROP DATABASE [OrderTrackingSystem_Test]

CREATE DATABASE [OrderTrackingSystem_Test]
GO

USE [OrderTrackingSystem_Test]
GO

BEGIN TRAN
IF OBJECT_ID ('Localizations', 'U') IS NULL
CREATE TABLE Localizations (
	Id INT IDENTITY(1,1) NOT NULL,
	Country NVARCHAR(100) NOT NULL,
	City NVARCHAR(100) NOT NULL,
	Street NVARCHAR(100) NOT NULL,
	Flat TINYINT NOT NULL,
	House TINYINT NOT NULL,
	ZipCode NVARCHAR(25) NOT NULL,
	CONSTRAINT PK__Localizations_Id PRIMARY KEY CLUSTERED (Id),
	CONSTRAINT CK__Localizations_Flat CHECK (Flat > 0),
	CONSTRAINT CK__Localizations_House CHECK (House > 0)
);
GO

IF OBJECT_ID ('Customers', 'U') IS NULL
CREATE TABLE Customers (
	Id INT IDENTITY(1,1) NOT NULL,
	Name NVARCHAR(100) NOT NULL,
	Surname NVARCHAR(100) NOT NULL,
	Age TINYINT NOT NULL,
	Number NVARCHAR(50) NOT NULL,
	Email NVARCHAR(50) NOT NULL,
	Balance SMALLMONEY NOT NULL,
	LocalizationId INT NOT NULL,
	CONSTRAINT PK__Customers_Id PRIMARY KEY CLUSTERED (Id),
	CONSTRAINT CK__Customers_Age CHECK (Age > 10),
	CONSTRAINT CK__Customers_Number CHECK (LEN(Number) >= 5),
	CONSTRAINT CK__Customers_Balance CHECK (Balance >= 0),
	CONSTRAINT UQ__Customers_LocalizationId UNIQUE (LocalizationId),
	CONSTRAINT FK__Customers_LocalizationId FOREIGN KEY (LocalizationId) REFERENCES Localizations (Id) ON DELETE NO ACTION
);
GO

IF OBJECT_ID ('Sellers', 'U') IS NULL
CREATE TABLE Sellers (
	Id INT IDENTITY(1,1) NOT NULL,
	Name NVARCHAR(100) NOT NULL,
	OpenDate DATETIME NOT NULL,
	TIN NVARCHAR(10) NOT NULL,
	Number NVARCHAR(50) NOT NULL,
	Email NVARCHAR(50) NOT NULL,
	LocalizationId INT NOT NULL,
	CONSTRAINT PK__Sellers_Id PRIMARY KEY CLUSTERED (Id),
	CONSTRAINT CK__Sellers_TIN CHECK (LEN(TIN) = 10),
	CONSTRAINT CK__Sellers_Number CHECK (LEN(Number) >= 5),
	CONSTRAINT UQ__Sellers_LocalizationId UNIQUE (LocalizationId),
	CONSTRAINT FK__Sellers_LocalizationId FOREIGN KEY (LocalizationId) REFERENCES Localizations (Id) ON DELETE NO ACTION
);
GO

IF OBJECT_ID ('Pickups', 'U') IS NULL
CREATE TABLE Pickups (
	Id INT IDENTITY(1,1) NOT NULL,
	Capacity SMALLINT NOT NULL,
	OpenDate DATETIME NOT NULL,
	CloseDate DATETIME NOT NULL,
	LocalizationId INT NOT NULL,
	CONSTRAINT PK__Pickups_Id PRIMARY KEY CLUSTERED (Id),
	CONSTRAINT CK__Pickups_Capacity CHECK (Capacity > 0),
	CONSTRAINT UQ__Pickups_LocalizationId UNIQUE (LocalizationId),
	CONSTRAINT FK__Pickups_LocalizationId FOREIGN KEY (LocalizationId) REFERENCES Localizations (Id) ON DELETE NO ACTION
);
GO

IF OBJECT_ID ('ComplaintDefinitions', 'U') IS NULL
CREATE TABLE ComplaintDefinitions (
	Id INT IDENTITY(1,1) NOT NULL,
	ComplaintName NVARCHAR(255) NOT NULL,
	RemainDays TINYINT NOT NULL,
	Definition NVARCHAR(MAX) NOT NULL,
	CONSTRAINT PK__ComplaintDefinitions_Id PRIMARY KEY CLUSTERED (Id),
	CONSTRAINT CK__ComplaintDefinitions_RemainDays CHECK (RemainDays > 0),
);
GO

IF OBJECT_ID ('Orders', 'U') IS NULL
CREATE TABLE Orders (
	Id INT IDENTITY(1,1) NOT NULL,
	Number NVARCHAR(255) NOT NULL,
	CustomerId INT NOT NULL,
	PayType TINYINT NOT NULL,
	DeliveryType TINYINT NOT NULL,
	PickupId INT NOT NULL,
	SellerId INT NOT NULL,
	ComplaintDefinitionId INT,
	CONSTRAINT PK__Orders_Id PRIMARY KEY CLUSTERED (Id),
	CONSTRAINT UQ__Orders_Number UNIQUE(Number),
	CONSTRAINT CK__Orders_PayType CHECK (PayType IN (1,2,3,4)),
	CONSTRAINT FK__Orders_CustomerId FOREIGN KEY (CustomerId) REFERENCES Customers (Id) ON DELETE CASCADE,
	CONSTRAINT FK__Orders_PickupId FOREIGN KEY (PickupId) REFERENCES Pickups (Id) ON DELETE CASCADE,
	CONSTRAINT FK__Orders_ComplaintDefinitionId FOREIGN KEY (ComplaintDefinitionId) REFERENCES ComplaintDefinitions(Id) ON DELETE SET NULL,
	CONSTRAINT FK__Orders_SellerId FOREIGN KEY (SellerId) REFERENCES Sellers (Id) ON DELETE NO ACTION
);
GO

IF OBJECT_ID ('ComplaintStates', 'U') IS NULL
CREATE TABLE ComplaintStates (
	OrderId INT NOT NULL,
	State TINYINT NOT NULL,
	Date TINYINT NOT NULL,
	CONSTRAINT FK__ComplaintStates_OrderId FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE,
	CONSTRAINT CK__ComplaintStates_State CHECK (State IN (1,2,3)),
);
GO

IF OBJECT_ID ('ComplaintFolders', 'U') IS NULL
CREATE TABLE ComplaintFolders (
	Id INT IDENTITY(1,1) NOT NULL,
	Name NVARCHAR(100) NOT NULL,
	ParentComplaintFolderId INT,
	CONSTRAINT PK__ComplaintFolders_Id PRIMARY KEY CLUSTERED (Id),
	CONSTRAINT FK__ComplaintFolders_ParentComplaintFolderId FOREIGN KEY (ParentComplaintFolderId) REFERENCES ComplaintFolders(Id) ON DELETE NO ACTION
);
GO

IF OBJECT_ID ('ComplaintRelations', 'U') IS NULL
CREATE TABLE ComplaintRelations (
	ComplaintId INT NOT NULL,
	ComplaintFolderId INT NOT NULL,
	CONSTRAINT FK__ComplaintRelations_ComplaintId FOREIGN KEY (ComplaintId) REFERENCES ComplaintDefinitions(Id) ON DELETE CASCADE,
	CONSTRAINT FK__ComplaintRelations_ComplaintFolderId FOREIGN KEY (ComplaintFolderId) REFERENCES ComplaintFolders(Id) ON DELETE CASCADE
);
GO

IF OBJECT_ID ('OrderStates', 'U') IS NULL
CREATE TABLE OrderStates (
	Id INT IDENTITY(1,1) NOT NULL,
	OrderId INT NOT NULL,
	State NVARCHAR(100) NOT NULL,
	Date DATETIME NOT NULL,
	Description NVARCHAR(500) NOT NULL,
	CONSTRAINT PK__OrderStates_Id PRIMARY KEY CLUSTERED (Id),
	CONSTRAINT FK__OrderStates_OrderId FOREIGN KEY (OrderId) REFERENCES Orders (Id) ON DELETE CASCADE
);
GO

IF OBJECT_ID ('Products', 'U') IS NULL
CREATE TABLE Products (
	Id INT IDENTITY(1,1) NOT NULL,
	Name NVARCHAR(100) NOT NULL,
	PriceNetto SMALLMONEY NOT NULL,
	VAT TINYINT NOT NULL,
	PriceBrutto SMALLMONEY NOT NULL,
	Category TINYINT NOT NULL,
	Weight DECIMAL(3,1) NOT NULL,
	Discount TINYINT NOT NULL,
	SellerId INT NOT NULL
	CONSTRAINT PK__Products_Id PRIMARY KEY CLUSTERED (Id),
	CONSTRAINT CK__Products_PriceNetto CHECK (PriceNetto > 0),
	CONSTRAINT CK__Products_PriceBrutto CHECK (PriceBrutto > 0),
	CONSTRAINT CK__Products_Discount CHECK (Discount BETWEEN 0 AND 100),
	CONSTRAINT CK__Products_Category CHECK (Category IN (1,2,3,4,5)),
	CONSTRAINT FK__Products_SellerId FOREIGN KEY (SellerId) REFERENCES Sellers (Id) ON DELETE CASCADE
);
GO

IF OBJECT_ID ('Sells', 'U') IS NULL
CREATE TABLE Sells (
	Id INT IDENTITY(1,1) NOT NULL,
	Number NVARCHAR(255) NOT NULL,
	SellingDate DATETIME NOT NULL,
	CustomerId INT NOT NULL,
	SellerId INT NOT NULL,
	PickupDays INT,
	CONSTRAINT PK__Sells_Id PRIMARY KEY CLUSTERED(Id),
	CONSTRAINT FK__Sells_CustomerId FOREIGN KEY (CustomerId) REFERENCES Customers(Id) ON DELETE CASCADE,
	CONSTRAINT FK__Sells_SellerId FOREIGN KEY (CustomerId) REFERENCES Customers(Id) ON DELETE NO ACTION
);
GO

IF OBJECT_ID ('Mails', 'U') IS NULL
CREATE TABLE Mails (
	Id INT IDENTITY(1,1) NOT NULL,
	Caption NVARCHAR(255) NOT NULL,
	Content NVARCHAR(MAX) NOT NULL,
	Date DATETIME NOT NULL,
	SenderId INT NOT NULL,
	ReceiverId INT NOT NULL,
	IsFromCustomer BIT NOT NULL,
	CONSTRAINT PK__Mails_Id PRIMARY KEY CLUSTERED (Id)
);
GO

IF OBJECT_ID ('Users', 'U') IS NULL
CREATE TABLE Users (
	AccountId INT NOT NULL,
	Login NVARCHAR(50) NOT NULL,
	Password NVARCHAR(50) NOT NULL,
	AccountType BIT NOT NULL
);
GO

IF OBJECT_ID ('Vouchers', 'U') IS NULL
CREATE TABLE Vouchers (
	Id INT IDENTITY(1,1) NOT NULL,
	Number NVARCHAR(25) NOT NULL,
	Value DECIMAL(19,2) NOT NULL,
	RemainValue DECIMAL(19,2) NOT NULL,
	ExpireDate DATETIME NOT NULL,
	CustomerId INT,
	CONSTRAINT PK__Vouchers_Id PRIMARY KEY CLUSTERED (Id),
	CONSTRAINT FK__Vouchers_CustomerId FOREIGN KEY (CustomerId) REFERENCES Customers (Id) ON DELETE CASCADE
);
GO

/* Implementacja tabel do redukowania relacji M-N */
/* Wiele produktow - do wielu zamowien */
IF OBJECT_ID ('SellCarts', 'U') IS NULL
CREATE TABLE SellCarts (
	ProductId INT NOT NULL,
	Amount TINYINT NOT NULL,
	SellId INT NOT NULL,
	CONSTRAINT CK__SellCarts_Amount CHECK (Amount > 0),
	CONSTRAINT FK__SellCarts_ProductId FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE,
	CONSTRAINT FK__SellCarts_SellId FOREIGN KEY (SellId) REFERENCES Sells(Id) ON DELETE NO ACTION
);
GO

/* Wiele produktow - do wielu wysylek */
IF OBJECT_ID ('OrderCarts', 'U') IS NULL
CREATE TABLE OrderCarts (
	Amount SMALLINT NOT NULL,
	ProductId INT NOT NULL,
	OrderId INT NOT NULL,
	CONSTRAINT CK__OrderCarts_Amount CHECK (Amount > 0),
	CONSTRAINT FK__OrderCarts_ProductId FOREIGN KEY (ProductId) REFERENCES Products (Id) ON DELETE CASCADE,
	CONSTRAINT FK__OrderCarts_OrderId FOREIGN KEY (OrderId) REFERENCES Orders (Id) ON DELETE CASCADE
);
GO

/* Wiele wiadomosci - do wielu zamowien */
IF OBJECT_ID ('MailOrderRelations', 'U') IS NULL
CREATE TABLE MailOrderRelations (
	MailId INT NOT NULL,
	OrderId INT NOT NULL,
	CONSTRAINT FK__MailOrderRelations_MailId FOREIGN KEY (MailId) REFERENCES Mails (Id) ON DELETE CASCADE,
	CONSTRAINT FK__MailOrderRelations_OrderId FOREIGN KEY (OrderId) REFERENCES Orders (Id) ON DELETE CASCADE
);
GO

IF OBJECT_ID ('Session', 'U') IS NULL
CREATE TABLE Session (
	AccountId INT NOT NULL,
	IsClient BIT NOT NULL,
);
GO

ALTER TABLE Orders
ADD OrderDate DATETIME  
GO

ALTER TABLE Pickups
DROP COLUMN OpenDate,
			CloseDate
GO

ALTER TABLE Pickups
ADD WorkHours NVARCHAR(50)
GO

ALTER TABLE OrderCarts
ADD Id INT IDENTITY(1,1)
GO

ALTER TABLE OrderCarts
ADD CONSTRAINT PK__OrderCarts_Id PRIMARY KEY (Id)
GO

ALTER TABLE Products
ADD SubCateogryId INT
GO

CREATE TABLE ProductCategories (
	Id INT IDENTITY(1,1),
	Title NVARCHAR(255),
	ParentCategoryId INT,
	CONSTRAINT PK__ProductCategories_Id PRIMARY KEY CLUSTERED (Id),
	CONSTRAINT FK__ProductCategories_ParentCateogoryId FOREIGN KEY (ParentCategoryId) REFERENCES ProductCategories(Id) ON DELETE NO ACTION
)
GO

ALTER TABLE Products
ADD CONSTRAINT FK__Products_SubCateogryId FOREIGN KEY (SubCateogryId) REFERENCES ProductCategories (Id) ON DELETE SET NULL
GO

ALTER TABLE SellCarts
ADD Id INT IDENTITY(1,1)
GO

ALTER TABLE SellCarts
ADD CONSTRAINT PK__SellCarts_Id PRIMARY KEY (Id)
GO

ALTER TABLE Mails
DROP COLUMN IsFromCustomer
GO

ALTER TABLE Mails
ADD MailRelation TINYINT
GO

ALTER TABLE Mails
ADD CONSTRAINT CK__Mails_MailRelation CHECK (MailRelation IN (1,2,3))
GO

ALTER TABLE MailOrderRelations
ADD Id INT IDENTITY(1,1)
GO

ALTER TABLE MailOrderRelations
ADD CONSTRAINT PK__MailOrderRelations_Id PRIMARY KEY (Id)
GO

ALTER TABLE ComplaintStates
DROP COLUMN Date
GO

ALTER TABLE ComplaintStates
ADD Date DATETIME
GO

ALTER TABLE ComplaintRelations
ADD Id INT IDENTITY(1,1)
GO

ALTER TABLE ComplaintRelations
ADD CONSTRAINT PK__ComplaintRelations_Id PRIMARY KEY (Id)
GO

ALTER TABLE ComplaintStates
ADD Id INT IDENTITY(1,1)
GO

ALTER TABLE ComplaintStates
ADD CONSTRAINT PK__ComplaintStates_Id PRIMARY KEY (Id)
GO

ALTER TABLE ComplaintStates
ADD SolutionDate DATETIME,
	EndDate DATETIME,
	ComplaintDefinitionId INT 
GO

ALTER TABLE ComplaintStates
ALTER COLUMN ComplaintDefinitionId INT NOT NULL
GO

ALTER TABLE ComplaintStates
ADD CONSTRAINT FK__ComplaintStates_ComplaintDefinitionId FOREIGN KEY (ComplaintDefinitionId) REFERENCES ComplaintDefinitions (Id)
GO

ALTER TABLE OrderStates
ALTER COLUMN State INT NOT NULL
GO

ALTER TABLE OrderStates
DROP COLUMN Description
GO

ALTER TABLE Users
ALTER COLUMN Password NVARCHAR(MAX) NOT NULL
GO

ALTER TABLE Users
ADD Id INT IDENTITY(1,1) NOT NULL
GO

ALTER TABLE Users
ADD CONSTRAINT PK__Users_Id PRIMARY KEY CLUSTERED (Id)
GO

ALTER TABLE Products
DROP CONSTRAINT FK__Products_SubCateogryId
GO

ALTER TABLE Products
DROP CONSTRAINT CK__Products_Category
GO

ALTER TABLE Products
DROP COLUMN SubCateogryId
GO

ALTER TABLE Products
ALTER COLUMN Category INT NOT NULL
GO

ALTER TABLE Products
ADD CONSTRAINT FK__Products_CategoryId FOREIGN KEY (Category) REFERENCES ProductCategories(Id)
GO

ALTER TABLE Products
ADD ImageData VARBINARY(MAX)
GO

CREATE SCHEMA Processes
GO

CREATE TABLE Processes (
	Id INT IDENTITY(1,1),
	Name NVARCHAR(500) NOT NULL,
	LastProcessDate SMALLDATETIME,
	Description NVARCHAR(MAX),
	StoredProcedureName NVARCHAR(255) NOT NULL
	CONSTRAINT PK__Processes_Id PRIMARY KEY CLUSTERED (Id)
);
GO


ALTER TABLE MailOrderRelations
DROP CONSTRAINT FK__MailOrderRelations_OrderId
GO

ALTER TABLE MailOrderRelations
ADD CONSTRAINT FK__MailOrderRelations_OrderId FOREIGN KEY (OrderId) REFERENCES Orders (Id) ON DELETE NO ACTION
GO

--Dodanie domyslnych procedur
INSERT INTO Processes (Name, LastProcessDate, Description, StoredProcedureName) VALUES
(N'Sprawdzanie terminowości dostarczania zamówień', NULL,
 N'Gdy zostało mniej niż 2 dni do dostarczenia zamówienia jest wysyłana wiadomość do klienta, gdy zamówienie zostalo przeterminowane - zamówienie jest usuwane a kwota zwracana kleintowi',
 N'Processes.CheckOrdersTerminary')
 GO

 INSERT INTO Processes (Name, LastProcessDate, Description, StoredProcedureName) VALUES
(N'Rozwiązanie wszystkich reklamacji klientów', NULL,
 N'Reklamacje od klientów są zamykane, zwracane pieniądze na konta klientów i zmiana statusu przesyłki na rozwiązanie reklamacji',
 N'Processes.AcceptAllComplaints')
 GO
--Koniec dodania procedur

INSERT INTO ProductCategories (Title, ParentCategoryId) VALUES ('Main', NULL)

-------------------------------------------------------PROCEDURY-------------------------------------------------------
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


IF OBJECT_ID ('[Processes].[AcceptAllComplaints]' ,'P') IS NOT NULL
DROP PROCEDURE [Processes].[AcceptAllComplaints]
GO

CREATE PROCEDURE [Processes].[AcceptAllComplaints]
AS
BEGIN
BEGIN TRAN 
	DECLARE @ShouldStart BIT = 0;
	DECLARE @SellerId INT = (SELECT AccountId FROM Session);

	WITH SellerComplaints (ComplaintId, OrderId, CustomerId, OrderNumber) AS
	(
		SELECT CS.Id, O.Id, CC.Id, O.Number FROM ComplaintStates CS
		INNER JOIN Orders O ON CS.OrderId = O.Id
		INNER JOIN ComplaintDefinitions CD ON CS.ComplaintDefinitionId = CD.Id
		CROSS APPLY (SELECT Id FROM Customers C WHERE C.Id = O.CustomerId) CC
		WHERE O.SellerId = @SellerId AND 
			  CS.State NOT IN (0,3) AND --anulowana lub ukonczona
			  DATEDIFF(DAY, CS.Date, GETDATE()) > CD.RemainDays --przekroczylo dni na rozpatrzenie
	),
	AmounToReturn (CustomerId, Amount) AS
	(
		SELECT SC.CustomerId, SUM(P.PriceBrutto * OC.Amount) FROM SellerComplaints SC
		INNER JOIN OrderCarts OC ON OC.OrderId = SC.OrderId
		INNER JOIN Products P ON P.Id = OC.ProductId
		GROUP BY SC.CustomerId
	)
	SELECT @ShouldStart = COUNT(*) FROM SellerComplaints

	IF @ShouldStart > 0
	BEGIN
		-- 1 - zamkniecie reklamacji i zmiana statusu
		UPDATE CS SET CS.SolutionDate = GETDATE() 
		FROM ComplaintStates CS
		INNER JOIN SellerComplaints SC ON CS.Id = SC.ComplaintId

		INSERT INTO OrderStates (OrderId, State, Date) 
		SELECT SC.OrderId,
		8, --rozwiazanie reklamacji
		GETDATE()
		FROM SellerComplaints SC

		-- 2 - zwrot pieniedzy
		UPDATE C SET C.Balance = C.Balance + ATR.Amount
		FROM Customers C
		INNER JOIN AmountToReturn ATR ON ATR.CustomerId = C.Id

		-- 3 - generowanie wiadomosci
		INSERT INTO Mails (Caption, Content, Date, SenderId, ReceiverId, MailRelation)
		SELECT N'Zwrot pieniędzy', 'Witam reklamacja zamówienia ' + CS.OrderNumber + ' została rozpatrzona',
		GETDATE(),
		@SellerId,
		CS.CustomerId,
		3 -- Relacja Seller -> Customer
		FROM SellerComplaints CS
	END
COMMIT TRAN
END
GO

----------------------------------------------TRIGGERY-------------------------------------------------
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
GO