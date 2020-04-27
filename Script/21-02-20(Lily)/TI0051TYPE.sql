USE [BDDistBHF_CF]
GO

/****** Object:  UserDefinedTableType [dbo].[TI0051Type]    Script Date: 21/02/2020 18:49:59 ******/
CREATE TYPE [dbo].[TI0051Type] AS TABLE(
	[ihid] [int] NULL,
	[ihigid] [int] NULL,
	[ihcodpro] [int] NULL,
	[cadesc] [nvarchar](100) NULL,
	[ihcant] [int] NULL,
	[ihtotcant] [int] NULL,
	[ihpcosto] [decimal](18, 4) NULL,
	[ihstock] [decimal](18, 2) NULL,
	[ihfact] [date] NULL,
	[ihhact] [nvarchar](5) NULL,
	[ihuact] [nvarchar](10) NULL,
	[estado] [int] NULL
)
GO


