USE [master];
GO

BEGIN TRY
	DROP DATABASE [VendingMachine];
END TRY
BEGIN CATCH	
END CATCH
GO

CREATE DATABASE [VendingMachine];
GO

USE [VendingMachine];
GO

CREATE TABLE [dbo].[Category](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Noise] [varchar](150) NOT NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED ([Id] ASC)
 );

CREATE TABLE [dbo].[Inventory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Qty] [int] NOT NULL,
	[Row] [int] NOT NULL,
	[Col] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
 CONSTRAINT [PK_Inventory] PRIMARY KEY CLUSTERED ([Id] ASC)
 );
 
CREATE TABLE [dbo].[Product](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Image] [varchar](500) NOT NULL,
	[Price] [real] NOT NULL,
	[CategoryId] [int] NOT NULL,
	CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([Id] ASC)
 );

 CREATE TABLE [dbo].[TransactionItem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VendingTransactionId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[SalePrice] [real] NOT NULL,
	CONSTRAINT [PK_TransactionItem] PRIMARY KEY CLUSTERED([Id] ASC)
 );
 
CREATE TABLE [dbo].[VendingTransaction](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[UserId] [int] NOT NULL,
	CONSTRAINT [PK_VendingTransaction] PRIMARY KEY CLUSTERED([Id] ASC)
);

CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Hash] [varchar](50) NOT NULL,
	[Salt] [varchar](50) NOT NULL,
	CONSTRAINT [PK_VendUser] PRIMARY KEY CLUSTERED ([Id] ASC)
);

ALTER TABLE [dbo].[Inventory] ADD  CONSTRAINT [DF_Inventory_Qty]  DEFAULT ((0)) FOR [Qty];
ALTER TABLE [dbo].[Inventory]  WITH CHECK ADD  CONSTRAINT [FK_Inventory_Product] FOREIGN KEY([ProductId]) 
	REFERENCES [dbo].[Product] ([Id])
	ON DELETE CASCADE;
ALTER TABLE [dbo].[Inventory] CHECK CONSTRAINT [FK_Inventory_Product];
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Category] FOREIGN KEY([CategoryId])
	REFERENCES [dbo].[Category] ([Id])
	ON DELETE CASCADE;
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Category];
ALTER TABLE [dbo].[TransactionItem]  WITH CHECK ADD  CONSTRAINT [FK_TransactionItem_Product] FOREIGN KEY([ProductId])
	REFERENCES [dbo].[Product] ([Id]);
ALTER TABLE [dbo].[TransactionItem] CHECK CONSTRAINT [FK_TransactionItem_Product];
ALTER TABLE [dbo].[TransactionItem]  WITH CHECK ADD  CONSTRAINT [FK_TransactionItem_VendingTransaction] FOREIGN KEY([VendingTransactionId])
	REFERENCES [dbo].[VendingTransaction] ([Id])
	ON DELETE CASCADE;
ALTER TABLE [dbo].[TransactionItem] CHECK CONSTRAINT [FK_TransactionItem_VendingTransaction];
ALTER TABLE [dbo].[VendingTransaction]  WITH CHECK ADD  CONSTRAINT [FK_VendingTransaction_User] FOREIGN KEY([UserId])
	REFERENCES [dbo].[User] ([Id])
	ON DELETE CASCADE;
ALTER TABLE [dbo].[VendingTransaction] CHECK CONSTRAINT [FK_VendingTransaction_User];

USE [master];
ALTER DATABASE [VendingMachine] SET  READ_WRITE;
USE [VendingMachine];

--SET IDENTITY_INSERT [dbo].[Category] ON;
--INSERT [dbo].[Category] ([Id], [Name], [Noise]) VALUES (38, N'Chips', N'Crunch, Crunch, Crunch, Yum!');
--INSERT [dbo].[Category] ([Id], [Name], [Noise]) VALUES (39, N'Candy', N'Lick, Lick, Yum!');
--INSERT [dbo].[Category] ([Id], [Name], [Noise]) VALUES (40, N'Nuts', N'Munch, Munch, Yum!');
--INSERT [dbo].[Category] ([Id], [Name], [Noise]) VALUES (41, N'Gum', N'Chew, Chew, Yum!');
--SET IDENTITY_INSERT [dbo].[Category] OFF;

--SET IDENTITY_INSERT [dbo].[Product] ON;
--INSERT [dbo].[Product] ([Id], [Name], [Price], [CategoryId]) VALUES (61, N'Lays Regular', 0.5, 38);
--INSERT [dbo].[Product] ([Id], [Name], [Price], [CategoryId]) VALUES (62, N'Pringles Barbeque', 0.65, 38);
--INSERT [dbo].[Product] ([Id], [Name], [Price], [CategoryId]) VALUES (63, N'Ruffles Sour Cream and Chives', 0.75, 38);
--INSERT [dbo].[Product] ([Id], [Name], [Price], [CategoryId]) VALUES (64, N'M&Ms Plain', 0.55, 39);
--INSERT [dbo].[Product] ([Id], [Name], [Price], [CategoryId]) VALUES (65, N'M&Ms Peanut', 0.55, 39);
--INSERT [dbo].[Product] ([Id], [Name], [Price], [CategoryId]) VALUES (66, N'Gummy Bears', 1, 39);
--INSERT [dbo].[Product] ([Id], [Name], [Price], [CategoryId]) VALUES (67, N'Peanuts', 1, 40);
--INSERT [dbo].[Product] ([Id], [Name], [Price], [CategoryId]) VALUES (68, N'Cashews', 1.5, 40);
--INSERT [dbo].[Product] ([Id], [Name], [Price], [CategoryId]) VALUES (69, N'Sunflower Seeds', 1.25, 40);
--INSERT [dbo].[Product] ([Id], [Name], [Price], [CategoryId]) VALUES (70, N'Hubba Bubba', 0.75, 41);
--INSERT [dbo].[Product] ([Id], [Name], [Price], [CategoryId]) VALUES (71, N'Bubble Yum', 0.75, 41);
--INSERT [dbo].[Product] ([Id], [Name], [Price], [CategoryId]) VALUES (72, N'Trident', 0.65, 41);
--SET IDENTITY_INSERT [dbo].[Product] OFF;

--SET IDENTITY_INSERT [dbo].[Inventory] ON;
--INSERT [dbo].[Inventory] ([Id], [Qty], [Row], [Col], [ProductId]) VALUES (62, 5, 1, 1, 61);
--INSERT [dbo].[Inventory] ([Id], [Qty], [Row], [Col], [ProductId]) VALUES (63, 5, 1, 2, 62);
--INSERT [dbo].[Inventory] ([Id], [Qty], [Row], [Col], [ProductId]) VALUES (64, 5, 1, 3, 63);
--INSERT [dbo].[Inventory] ([Id], [Qty], [Row], [Col], [ProductId]) VALUES (65, 5, 2, 1, 64);
--INSERT [dbo].[Inventory] ([Id], [Qty], [Row], [Col], [ProductId]) VALUES (66, 3, 2, 2, 65);
--INSERT [dbo].[Inventory] ([Id], [Qty], [Row], [Col], [ProductId]) VALUES (67, 5, 2, 3, 66);
--INSERT [dbo].[Inventory] ([Id], [Qty], [Row], [Col], [ProductId]) VALUES (68, 5, 3, 1, 67);
--INSERT [dbo].[Inventory] ([Id], [Qty], [Row], [Col], [ProductId]) VALUES (69, 5, 3, 2, 68);
--INSERT [dbo].[Inventory] ([Id], [Qty], [Row], [Col], [ProductId]) VALUES (70, 5, 3, 3, 69);
--INSERT [dbo].[Inventory] ([Id], [Qty], [Row], [Col], [ProductId]) VALUES (71, 5, 4, 1, 70);
--INSERT [dbo].[Inventory] ([Id], [Qty], [Row], [Col], [ProductId]) VALUES (72, 5, 4, 2, 71);
--INSERT [dbo].[Inventory] ([Id], [Qty], [Row], [Col], [ProductId]) VALUES (73, 5, 4, 3, 72);
--SET IDENTITY_INSERT [dbo].[Inventory] OFF;


