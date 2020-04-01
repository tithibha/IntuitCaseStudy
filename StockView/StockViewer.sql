DECLARE @dbname nvarchar(128)
SET @dbname = N'StockViewer'

DECLARE @isExists INT
DECLARE @isExistsLog INT
exec master.dbo.xp_fileexist 'D:\StockViewer.mdf', 
@isExists OUTPUT
exec master.dbo.xp_fileexist 'D:\StockViewer_log.LDF', 
@isExistsLog OUTPUT

IF (@isExists=1 and @isExistsLog=1)
	begin
		EXEC sp_attach_db 'StockViewer','D:\StockViewer.mdf'
	end

else
	begin
		EXEC sp_configure 'xp_cmdshell', 1
		RECONFIGURE
		IF(@isExists =1)
			execute xp_cmdshell 'del D:\StockViewer.mdf'
		IF(@isExistsLog=1)
			execute xp_cmdshell 'del D:\StockViewer_log.LDF'
		IF (EXISTS (SELECT name 
			FROM master.dbo.sysdatabases 
			WHERE ('[' + name + ']' = @dbname 
			OR name = @dbname)))
				begin
					drop database StockViewer
				end

				begin
					CREATE DATABASE [StockViewer]  ON 
					(
						NAME = N'StockViewer', 
						FILENAME = N'D:\StockViewer.mdf' , 
						SIZE = 3145728KB , 
						MAXSIZE = 3145728KB, 
						FILEGROWTH = 0KB
					) 
					LOG ON 
					(
						NAME = N'_log', 
						FILENAME = N'D:\StockViewer_log.LDF' , 
						SIZE = 1048576KB, 
						MAXSIZE = 1048576KB,
						FILEGROWTH = 1024KB		
					)
					COLLATE SQL_Latin1_General_CP1_CI_AS

				end

	end
	go


DECLARE @dbname nvarchar(128)
SET @dbname = N'StockViewer'

IF (EXISTS (SELECT name 
FROM master.dbo.sysdatabases 
WHERE ('[' + name + ']' = @dbname 
OR name = @dbname)))
begin


USE [StockViewer]

/****** Object:  Table [dbo].[UsageAnalyticsParameters]    Script Date: 12/19/2013 01:10:23 ******/
IF NOT EXISTS ( 
    SELECT * FROM sys.tables t
    INNER JOIN sys.schemas s on t.schema_id = s.schema_id
    WHERE s.name = 'dbo' and t.name = 'StockTrend' )
Begin


SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[StockTrend](
	[StockSymbol] [nvarchar](50) NOT NULL,
	[StockName] [nvarchar](50) NULL ,
	[Price] [nvarchar](50) NULL,
	[TimeStamp] [datetime] NOT NULL,
	[Change] [nvarchar](50),
	[YearHigh] [nvarchar](50),
	[YearLow] [nvarchar](50),

) ON [PRIMARY]

end
end


IF NOT EXISTS ( 
    SELECT * FROM sys.tables t
    INNER JOIN sys.schemas s on t.schema_id = s.schema_id
    WHERE s.name = 'dbo' and t.name = 'AllCompanies' )
Begin


SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[AllCompanies](
	[Exchange] [nvarchar](max) NULL,
	[Symbol] [varchar](100) NOT NULL,
	[Name] [nvarchar](max)  NULL,
	[Price] [nvarchar](max) NULL,
	Primary Key(Symbol)
) ON [PRIMARY]

End




GO

/****** Object:  StoredProcedure [dbo].[InsertAllCompanies]    Script Date: 04/30/2014 08:57:32 ******/

	CREATE TYPE TempTable2 as Table
(	
	[Exchange] [varchar](max) NULL,
	[Symbol] [varchar](100) NOT NULL,
	[Name] [nvarchar](max)  NULL,
	[Price] [nvarchar](max) NULL
);


IF NOT EXISTS (
    SELECT * FROM sys.procedures p
    INNER JOIN sys.schemas s on p.schema_id = s.schema_id
    WHERE s.name = 'dbo' and p.name = 'InsertAllCompanies'
)
    EXEC sp_executesql @statement = N'CREATE PROCEDURE [dbo].[InsertAllCompanies] AS ' ;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


Alter Procedure [dbo].[InsertAllCompanies]	@data	TempTable2 readonly

 AS 
 BEGIN
  BEGIN TRY
	BEGIN TRANSACTION EVENTDATA
	

	
	insert into [AllCompanies] ([Exchange],[Symbol],[Name],[Price]) (SELECT * FROM @data)
			
	COMMIT TRANSACTION EVENTDATA
  END TRY
  BEGIN CATCH
	ROLLBACK TRANSACTION EVENTDATA
	
  END CATCH
END
GO


Create type TempTable3 as Table
(	
	[StockSymbol] [nvarchar](50) NOT NULL,
	[StockName] [nvarchar](50) null ,
	[Price] [nvarchar](50) null,	
	[Change] [nvarchar](50) ,
	[YearHigh] [nvarchar](50) ,
	[YearLow] [nvarchar](50) 
);


IF NOT EXISTS (
    SELECT * FROM sys.procedures p
    INNER JOIN sys.schemas s on p.schema_id = s.schema_id
    WHERE s.name = 'dbo' and p.name = 'InsertStockTrend'
)
    EXEC sp_executesql @statement = N'CREATE PROCEDURE [dbo].[InsertStockTrend] AS ' ;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


Alter Procedure [dbo].[InsertStockTrend]	@data TempTable3  readonly

 AS 
 BEGIN
  BEGIN TRY
	BEGIN TRANSACTION EVENTDATA
	
	insert into [StockTrend] ([StockSymbol],[StockName],[Price],[Change],[YearHigh],[YearLow]) (SELECT * FROM @data)
			
	COMMIT TRANSACTION EVENTDATA
  END TRY
  BEGIN CATCH
	ROLLBACK TRANSACTION EVENTDATA
	
  END CATCH
END
GO