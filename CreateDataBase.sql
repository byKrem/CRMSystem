use [master];
go
create database [CRMSystem];
GO
use [CRMSystem];
go
create table [Managers]
(
[Id] int PRIMARY KEY IDENTITY(1,1),
[FirstName] nvarchar(100) NOT NULL,
[SurName] nvarchar(100) NOT NULL,
[MiddleName] nvarchar(100) NULL,
[Salary] money NOT NULL,
[Foto] image NULL 
);
go
create table [Customers]
(
[Id] int PRIMARY KEY IDENTITY(1,1),
[FirstName] nvarchar(100) NOT NULL,
[SurName] nvarchar(100) NOT NULL,
[MiddleName] nvarchar(100) NULL,
[Email] nvarchar(100) NULL,
[Phone] nvarchar(100) NULL,
[ManagerId] int FOREIGN KEY REFERENCES [Managers]([Id])
);
go
create table [OrderStatus]
(
[Id] int PRIMARY KEY IDENTITY(1,1),
[Name] nvarchar(100) NOT NULL
);
go
create table [Orders]
(
[Id] int PRIMARY KEY IDENTITY(1,1),
[Description] nvarchar(500) NOT NULL,
[CustomerId] int FOREIGN KEY REFERENCES [Customers]([Id]),
[OrderStatusId] int FOREIGN KEY REFERENCES [OrderStatus]([Id])
);
go
create table [Products]
(
[Id] int PRIMARY KEY IDENTITY(1,1),
[Name] nvarchar(100) NOT NULL,
[Price] money NOT NULL,
[PriceChange] money DEFAULT(0) NOT NULL
);
create table [ProductOrder]
(
[Id] int PRIMARY KEY IDENTITY(1,1),
[OrderId] int FOREIGN KEY REFERENCES [Orders]([Id]),
[ProductId] int FOREIGN KEY REFERENCES [Products]([Id])
);