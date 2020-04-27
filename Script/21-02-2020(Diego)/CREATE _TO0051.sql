USE [BDDistBHF_CF]
GO

/****** Object:  Table [dbo].[TO0051]    Script Date: 21/02/2020 14:53:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TO0051](
	[omnumi] [int] IDENTITY(1,1) NOT NULL,
	[omolnumi] [int] NOT NULL,
	[omestado] [int] NOT NULL,
	[omTipoCam] [decimal](18, 2) NOT NULL,
	[omCorteD] [decimal](18, 2) NOT NULL,
	[omCantD] [decimal](18, 2) NOT NULL,
	[omTotalD] [decimal](18, 2) NOT NULL,
	[omCorteB] [decimal](18, 2) NOT NULL,
	[omCantB] [decimal](18, 2) NOT NULL,
	[omTotalB] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_TO0051] PRIMARY KEY CLUSTERED 
(
	[omnumi] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[TO0051]  WITH CHECK ADD  CONSTRAINT [FK_TO0051_TO005] FOREIGN KEY([omolnumi])
REFERENCES [dbo].[TO005] ([olnumi])
GO

ALTER TABLE [dbo].[TO0051] CHECK CONSTRAINT [FK_TO0051_TO005]
GO


