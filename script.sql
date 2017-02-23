CREATE DATABASE [restaurant]
GO

USE [restaurant]
GO
/****** Object:  Table [dbo].[cuisine]    Script Date: 2/22/2017 4:45:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cuisine](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[type] [varchar](255) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[restaurant]    Script Date: 2/22/2017 4:45:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[restaurant](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NULL,
	[location] [varchar](255) NULL,
	[delivery] [tinyint] NULL,
	[cuisine_id] [int] NULL
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[cuisine] ON 

INSERT [dbo].[cuisine] ([id], [type]) VALUES (4, N'mexican')
SET IDENTITY_INSERT [dbo].[cuisine] OFF
SET IDENTITY_INSERT [dbo].[restaurant] ON 

INSERT [dbo].[restaurant] ([id], [name], [location], [delivery], [cuisine_id]) VALUES (5, N'food', N'seattle', 1, 4)
INSERT [dbo].[restaurant] ([id], [name], [location], [delivery], [cuisine_id]) VALUES (6, N'aaaaa', N'asdjflk', 1, 4)
INSERT [dbo].[restaurant] ([id], [name], [location], [delivery], [cuisine_id]) VALUES (7, N'zzzzzzzzzzzz', N'sds', 1, 4)
INSERT [dbo].[restaurant] ([id], [name], [location], [delivery], [cuisine_id]) VALUES (8, N'fffffffffffff', N'sadfsdf', 1, 4)
SET IDENTITY_INSERT [dbo].[restaurant] OFF
