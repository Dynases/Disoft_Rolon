USE [BDDistBHF_CF]
GO

/****** Object:  Table [dbo].[TI0051]    Script Date: 21/02/2020 18:49:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TI0051](
	[ihid] [int] NOT NULL,
	[ihigid] [int] NULL,
	[ihcodpro] [int] NULL,
	[ihcant] [int] NULL,
	[ihtotcant] [int] NULL,
	[ihpcosto] [decimal](18, 4) NULL,
	[ihfact] [date] NULL,
	[ihhact] [nvarchar](5) NULL,
	[ihuact] [nvarchar](10) NULL,
 CONSTRAINT [PK_TI0051] PRIMARY KEY CLUSTERED 
(
	[ihid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


