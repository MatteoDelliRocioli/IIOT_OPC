USE iotOpc
GO

/****** Object:  Table [dbo].[DailyProduction]    Script Date: 03-May-19 16:24:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DailyProduction](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TimeStamp] [datetime2](7) NOT NULL,
	[NumPieces] [numeric](18, 0) NOT NULL,
	[NumPiecesRejected] [numeric](18, 0) NOT NULL,
 CONSTRAINT [PK_DailyProduction] PRIMARY KEY CLUSTERED 
(
	[TimeStamp] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


