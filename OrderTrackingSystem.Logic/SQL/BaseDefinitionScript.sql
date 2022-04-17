USE [master]
GO

IF EXISTS(SELECT * FROM master.sys.databases WHERE name='OrderTrackingSystem')
DROP DATABASE [OrderTrackingSystem]

CREATE DATABASE [OrderTrackingSystem]
GO

USE [OrderTrackingSystem]
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

COMMIT TRAN