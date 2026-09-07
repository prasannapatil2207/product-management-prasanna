-- Product Management Database
-- SQL Server LocalDB

IF DB_ID('ProductDB') IS NULL
BEGIN
    CREATE DATABASE ProductDB;
END
GO

USE ProductDB;
GO

IF OBJECT_ID('dbo.Products', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Products
    (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Category NVARCHAR(100) NOT NULL,
        Price DECIMAL(10,2) NOT NULL,
        Quantity INT NOT NULL
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Products)
BEGIN
    INSERT INTO dbo.Products (Name, Category, Price, Quantity)
    VALUES
    (N'Laptop', N'Electronics', 55000.00, 10),
    (N'Mouse', N'Electronics', 800.00, 25),
    (N'Keyboard', N'Electronics', 1200.00, 20);
END
GO
