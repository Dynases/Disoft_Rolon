USE [BDDistBHF_CF]
GO

/****** Object:  Table [dbo].[TI005]    Script Date: 21/02/2020 18:48:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TI005](
	[igid] [int] NOT NULL,
	[igfdoc] [date] NULL,
	[igobs] [nvarchar](200) NULL,
	[igcodpack] [int] NULL,
	[igcantP] [int] NULL,
	[igpcosto] [decimal](18, 4) NULL,
	[igcantNP] [int] NULL,
	[igest] [int] NULL,
	[igalm] [int] NULL,
	[igfact] [date] NULL,
	[ighact] [nvarchar](5) NULL,
	[iguact] [nvarchar](10) NULL,
 CONSTRAINT [PK_TI005] PRIMARY KEY CLUSTERED 
(
	[igid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


