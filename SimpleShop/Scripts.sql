CREATE DATABASE SimpleShopDatabase
GO
USE SimpleShopDatabase
GO
CREATE TABLE [User]
(
	UserId INT IDENTITY PRIMARY KEY,
	Username NVARCHAR(50) NOT NULL UNIQUE,
	[Password] NVARCHAR(50) NOT NULL,
	Family NVARCHAR(50) NOT NULL,
	Name NVARCHAR(50) NOT NULL,
	Mobile NVARCHAR(11) NOT NULL,
	IsAdmin BIT NOT NULL DEFAULT 0
)
GO
CREATE TABLE [State]
(
	StateId INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(50) NOT NULL UNIQUE
)
GO
CREATE TABLE City
(
	CityId INT IDENTITY PRIMARY KEY,
	StateId INT NOT NULL FOREIGN KEY
	REFERENCES [State](StateId)
	ON DELETE CASCADE,
	Name NVARCHAR(50)
)
CREATE TABLE Customer
(
	CustomerId INT IDENTITY PRIMARY KEY,
	Username NVARCHAR(50) NOT NULL UNIQUE,
	[Password] NVARCHAR(50) NOT NULL,
	Family NVARCHAR(50) NOT NULL,
	Name NVARCHAR(50) NOT NULL,
	Mobile NVARCHAR(11) NOT NULL,
	CityId INT NOT NULL FOREIGN KEY
	REFERENCES City(CityId)
	ON DELETE CASCADE,
	[Address] NVARCHAR(1000) NOT NULL
)
GO
CREATE TABLE Category
(
	CategoryId INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(50) NOT NULL UNIQUE,
	PhotoFilename NVARCHAR(50)
)
GO
CREATE TABLE Subcategory
(
	SubcategoryId INT IDENTITY PRIMARY KEY,
	CategoryId INT NOT NULL FOREIGN KEY
	REFERENCES Category(CategoryId)
	ON DELETE CASCADE,
	Name NVARCHAR(50) NOT NULL,
	PhotoFilename NVARCHAR(50)
)
GO
CREATE TABLE Product
(
	ProductId INT IDENTITY PRIMARY KEY,
	SubcategoryId INT NOT NULL FOREIGN KEY
	REFERENCES Subcategory(SubcategoryId)
	ON DELETE CASCADE,
	Name NVARCHAR(200) NOT NULL,
	UnitPrice INT NOT NULL,
	PhotoFilename NVARCHAR(50)
)
GO
CREATE TABLE ProductPhoto
(
	ProductPhotoId INT IDENTITY PRIMARY KEY,
	ProductId INT NOT NULL FOREIGN KEY
	REFERENCES Product(ProductId),
	PhotoFilename NVARCHAR(50) NOT NULL
)
GO
CREATE TABLE Unit
(
	UnitId INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(50) NOT NULL UNIQUE
)
GO
CREATE TABLE ProductAttribute
(
	ProductAttributeId INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(100) NOT NULL,
	UnitId INT NOT NULL FOREIGN KEY
	REFERENCES Unit(UnitId)
	ON DELETE CASCADE,
	UNIQUE(Name,UnitId)
)
GO
CREATE TABLE ProductAttributeValue
(
	ProductAttributeValueId INT IDENTITY PRIMARY KEY,
	ProductId INT NOT NULL FOREIGN KEY
	REFERENCES Product(ProductId),
	ProductAttributeId INT NOT NULL FOREIGN KEY
	REFERENCES ProductAttribute(ProductAttributeId),
	[Value] NVARCHAR(MAX)
)
GO
CREATE TABLE [Order]
(
	OrderId INT IDENTITY PRIMARY KEY,
	CustomerId INT NOT NULL FOREIGN KEY
	REFERENCES Customer(CustomerId)
	ON DELETE CASCADE,
	OrderDate DATETIME NOT NULL,
	IsPayed BIT NOT NULL DEFAULT 0,
	IsSent BIT NOT NULL DEFAULT 0,
	PaymentCode NVARCHAR(50) NOT NULL DEFAULT N''
)
GO
CREATE TABLE OrderDetail
(
	OrderDetailId INT IDENTITY PRIMARY KEY,
	OrderId INT NOT NULL FOREIGN KEY
	REFERENCES [Order](OrderId)
	ON DELETE CASCADE,
	ProductId INT NOT NULL FOREIGN KEY
	REFERENCES Product(ProductId)
	ON DELETE CASCADE,
	Quantity INT NOT NULL DEFAULT 1,
	UnitPrice INT NOT NULL
)
GO
