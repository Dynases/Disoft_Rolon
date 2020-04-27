USE [BDDistBHF_CF]
GO

/****** Object:  UserDefinedTableType [dbo].[TC0013Type]    Script Date: 20/02/2020 18:12:52 ******/
CREATE TYPE [dbo].[TC0013Type] AS TABLE(
	[cbnumi] [int] NULL,
	[cbtccanumi] [int] NULL,
	[cbtccanumi1] [int] NULL,
	[cadesc] [nvarchar](100) NULL,
	[cbcant] [int] NULL,
	[estado] [int] NOT NULL
)
GO


