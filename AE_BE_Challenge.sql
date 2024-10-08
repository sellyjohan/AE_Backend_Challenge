USE [master]
GO
/****** Object:  Database [AE]    Script Date: 8/7/2024 20:57:11 ******/
CREATE DATABASE [AE]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'AE', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER01\MSSQL\DATA\AE.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'AE_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER01\MSSQL\DATA\AE_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [AE] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AE].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AE] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [AE] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [AE] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [AE] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [AE] SET ARITHABORT OFF 
GO
ALTER DATABASE [AE] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [AE] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [AE] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [AE] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [AE] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [AE] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [AE] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [AE] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [AE] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [AE] SET  DISABLE_BROKER 
GO
ALTER DATABASE [AE] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [AE] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [AE] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [AE] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [AE] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [AE] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [AE] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [AE] SET RECOVERY FULL 
GO
ALTER DATABASE [AE] SET  MULTI_USER 
GO
ALTER DATABASE [AE] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [AE] SET DB_CHAINING OFF 
GO
ALTER DATABASE [AE] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [AE] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [AE] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [AE] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'AE', N'ON'
GO
ALTER DATABASE [AE] SET QUERY_STORE = ON
GO
ALTER DATABASE [AE] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [AE]
GO
/****** Object:  Table [dbo].[Ports]    Script Date: 8/7/2024 20:57:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ports](
	[PortId] [int] IDENTITY(1,1) NOT NULL,
	[PortName] [nvarchar](255) NOT NULL,
	[Longitude] [decimal](9, 6) NOT NULL,
	[Latitude] [decimal](9, 6) NOT NULL,
	[RowStatus] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](255) NOT NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](255) NULL,
 CONSTRAINT [PK__Port__D859BF8F3B509FC9] PRIMARY KEY CLUSTERED 
(
	[PortId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 8/7/2024 20:57:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](255) NOT NULL,
	[RowStatus] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](255) NOT NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](255) NULL,
 CONSTRAINT [PK__Role__8AFACE1A073AFC4E] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ships]    Script Date: 8/7/2024 20:57:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ships](
	[ShipId] [int] IDENTITY(1,1) NOT NULL,
	[ShipName] [nvarchar](255) NOT NULL,
	[Longitude] [decimal](9, 6) NOT NULL,
	[Latitude] [decimal](9, 6) NOT NULL,
	[Velocity] [decimal](10, 2) NOT NULL,
	[RowStatus] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](255) NOT NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](255) NULL,
 CONSTRAINT [PK__Ship__2A05CAB32A6315BF] PRIMARY KEY CLUSTERED 
(
	[ShipId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 8/7/2024 20:57:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[UserRoleId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[RowStatus] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](255) NOT NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](255) NULL,
 CONSTRAINT [PK__UserRole__3D978A35BE2AA0A0] PRIMARY KEY CLUSTERED 
(
	[UserRoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 8/7/2024 20:57:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](255) NOT NULL,
	[FullName] [nvarchar](255) NOT NULL,
	[Birthdate] [date] NULL,
	[RowStatus] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](255) NOT NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](255) NULL,
 CONSTRAINT [PK__User__1788CC4C71D67708] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserShips]    Script Date: 8/7/2024 20:57:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserShips](
	[UserShipid] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[shipId] [int] NOT NULL,
	[RowStatus] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](255) NOT NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](255) NULL,
 CONSTRAINT [PK__UserShip__12CC27EE1E4929C0] PRIMARY KEY CLUSTERED 
(
	[UserShipid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Ports] ON 

INSERT [dbo].[Ports] ([PortId], [PortName], [Longitude], [Latitude], [RowStatus], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1, N'Port A', CAST(12.345678 AS Decimal(9, 6)), CAST(45.678901 AS Decimal(9, 6)), 1, CAST(N'2024-08-04T20:54:09.783' AS DateTime), N'System', CAST(N'2024-08-04T20:54:09.783' AS DateTime), N'System')
INSERT [dbo].[Ports] ([PortId], [PortName], [Longitude], [Latitude], [RowStatus], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2, N'Port B', CAST(-23.456789 AS Decimal(9, 6)), CAST(67.890123 AS Decimal(9, 6)), 1, CAST(N'2024-08-04T20:54:09.783' AS DateTime), N'System', CAST(N'2024-08-04T20:54:09.783' AS DateTime), N'System')
INSERT [dbo].[Ports] ([PortId], [PortName], [Longitude], [Latitude], [RowStatus], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (3, N'Port C', CAST(34.567890 AS Decimal(9, 6)), CAST(-12.345678 AS Decimal(9, 6)), 1, CAST(N'2024-08-04T20:54:09.783' AS DateTime), N'System', CAST(N'2024-08-04T20:54:09.783' AS DateTime), N'System')
INSERT [dbo].[Ports] ([PortId], [PortName], [Longitude], [Latitude], [RowStatus], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (4, N'Port D', CAST(56.789012 AS Decimal(9, 6)), CAST(-23.456789 AS Decimal(9, 6)), 1, CAST(N'2024-08-04T20:54:09.783' AS DateTime), N'System', CAST(N'2024-08-04T20:54:09.783' AS DateTime), N'System')
INSERT [dbo].[Ports] ([PortId], [PortName], [Longitude], [Latitude], [RowStatus], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (5, N'Port E', CAST(-78.901234 AS Decimal(9, 6)), CAST(89.012345 AS Decimal(9, 6)), 1, CAST(N'2024-08-04T20:54:09.783' AS DateTime), N'System', CAST(N'2024-08-04T20:54:09.783' AS DateTime), N'System')
INSERT [dbo].[Ports] ([PortId], [PortName], [Longitude], [Latitude], [RowStatus], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (6, N'Port F', CAST(90.123456 AS Decimal(9, 6)), CAST(-56.789012 AS Decimal(9, 6)), 1, CAST(N'2024-08-04T20:54:09.783' AS DateTime), N'System', CAST(N'2024-08-04T20:54:09.783' AS DateTime), N'System')
INSERT [dbo].[Ports] ([PortId], [PortName], [Longitude], [Latitude], [RowStatus], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (7, N'Port G', CAST(-10.111213 AS Decimal(9, 6)), CAST(20.212223 AS Decimal(9, 6)), 1, CAST(N'2024-08-04T20:54:09.783' AS DateTime), N'System', CAST(N'2024-08-04T20:54:09.783' AS DateTime), N'System')
INSERT [dbo].[Ports] ([PortId], [PortName], [Longitude], [Latitude], [RowStatus], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (8, N'Port H', CAST(30.313233 AS Decimal(9, 6)), CAST(-40.414243 AS Decimal(9, 6)), 1, CAST(N'2024-08-04T20:54:09.783' AS DateTime), N'System', CAST(N'2024-08-04T20:54:09.783' AS DateTime), N'System')
INSERT [dbo].[Ports] ([PortId], [PortName], [Longitude], [Latitude], [RowStatus], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (9, N'Port I', CAST(50.515253 AS Decimal(9, 6)), CAST(60.616263 AS Decimal(9, 6)), 1, CAST(N'2024-08-04T20:54:09.783' AS DateTime), N'System', CAST(N'2024-08-04T20:54:09.783' AS DateTime), N'System')
INSERT [dbo].[Ports] ([PortId], [PortName], [Longitude], [Latitude], [RowStatus], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (10, N'Port J', CAST(-70.717273 AS Decimal(9, 6)), CAST(80.818283 AS Decimal(9, 6)), 1, CAST(N'2024-08-04T20:54:09.783' AS DateTime), N'System', CAST(N'2024-08-04T20:54:09.783' AS DateTime), N'System')
INSERT [dbo].[Ports] ([PortId], [PortName], [Longitude], [Latitude], [RowStatus], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (11, N'Port T', CAST(90.123456 AS Decimal(9, 6)), CAST(-56.789012 AS Decimal(9, 6)), 1, CAST(N'2024-08-04T20:54:09.783' AS DateTime), N'System', CAST(N'2024-08-04T20:54:09.783' AS DateTime), N'System')
SET IDENTITY_INSERT [dbo].[Ports] OFF
GO

/****** Object:  StoredProcedure [dbo].[SP_DeleteRole]    Script Date: 8/7/2024 20:57:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[SP_DeleteRole] 
	@roleid	int,
	@modifiedby NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @RowsAffected INT;

    Update dbo.[Roles]
	set RowStatus = 0,
		ModifiedDate = GETDATE(),
		ModifiedBy = @modifiedby
	where RoleId = @roleid

	SELECT @@ROWCOUNT;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DeleteShip]    Script Date: 8/7/2024 20:57:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[SP_DeleteShip] 
	@shipid	int,
	@modifiedby NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @RowsAffected INT;

    Update dbo.[Ships]
	set RowStatus = 0,
		ModifiedDate = GETDATE(),
		ModifiedBy = @modifiedby
	where ShipId = @shipid

	SELECT @@ROWCOUNT;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DeleteUser]    Script Date: 8/7/2024 20:57:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

--exec sp_deleteuser 1, 'sys'
CREATE procedure [dbo].[SP_DeleteUser] 
	@userid	int,
	@modifiedby NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @RowsAffected INT;

    Update dbo.[Users]
	set RowStatus = 0,
		ModifiedDate = GETDATE(),
		ModifiedBy = @modifiedby
	where UserId = @userid

	SELECT @@ROWCOUNT;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DeleteUserRole]    Script Date: 8/7/2024 20:57:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[SP_DeleteUserRole]
	@userroleid	int,
	@modifiedby NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @RowsAffected INT;

    Update dbo.[UserRoles]
	set RowStatus = 0,
		ModifiedDate = GETDATE(),
		ModifiedBy = @modifiedby
	where UserRoleId = @userroleid

	SELECT @@ROWCOUNT;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DeleteUserShip]    Script Date: 8/7/2024 20:57:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[SP_DeleteUserShip] 
	@usershipid	int,
	@modifiedby NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @RowsAffected INT;

    Update dbo.[UserShips]
	set RowStatus = 0,
		ModifiedDate = GETDATE(),
		ModifiedBy = @modifiedby
	where UserShipid = @usershipid

	SELECT @@ROWCOUNT;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_InsertRole]    Script Date: 8/7/2024 20:57:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

--exec insert role
CREATE procedure [dbo].[SP_InsertRole] 
	@rolename NVARCHAR(255),
	@userid NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @OutputTbl TABLE (RoleId INT)

    Insert Into dbo.[Roles]
		(	RoleName,
			RowStatus,
			CreatedDate,
			CreatedBy,
			ModifiedDate,
			ModifiedBy
		)
	OUTPUT INSERTED.RoleId into @OutputTbl
	Values
		(	@rolename,
			1,
			GETDATE(),
			@userid,
			GETDATE(),
			@userid
		)

	select RoleId, RoleName, RowStatus, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy from Roles
	where RoleId = (select RoleId from @OutputTbl)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_InsertShip]    Script Date: 8/7/2024 20:57:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[SP_InsertShip] 
	@shipname NVARCHAR(255),
    @longitude decimal(9,6),
	@latitude decimal(9,6),
	@velocity decimal(10,2),
	@userid NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @OutputTbl TABLE (ShipId INT)

    Insert Into dbo.[Ships]
		(	ShipName,
			Longitude,
			Latitude,
			Velocity,
			RowStatus,
			CreatedDate,
			CreatedBy,
			ModifiedDate,
			ModifiedBy
		)
	OUTPUT INSERTED.ShipId into @OutputTbl
	Values
		(	@shipname,
			@longitude,
			@latitude,
			@velocity,
			1,
			GETDATE(),
			@userid,
			GETDATE(),
			@userid
		)

	select ShipId, ShipName, Longitude, Latitude, Velocity, RowStatus, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy from Ships
	where ShipId = (select ShipId from @OutputTbl)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_InsertUser]    Script Date: 8/7/2024 20:57:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[SP_InsertUser] 
	@username NVARCHAR(255),
    @fullname NVARCHAR(255),
	@birthdate datetime,
	@createdby NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @OutputTbl TABLE (UserId INT)

    Insert Into dbo.[Users]
		(	UserName,
			FullName,
			Birthdate,
			RowStatus,
			CreatedDate,
			CreatedBy,
			ModifiedDate,
			ModifiedBy
		)
	OUTPUT INSERTED.UserId into @OutputTbl
	Values
		(	@username,
			@fullname,
			@birthdate,
			1,
			GETDATE(),
			@createdby,
			GETDATE(),
			@createdby
		)

	select UserId, UserName, FullName, Birthdate, RowStatus, CreatedDate, 
	CreatedBy, ModifiedDate, ModifiedBy from Users
	where UserId = (select UserId from @OutputTbl)

END
GO
/****** Object:  StoredProcedure [dbo].[SP_InsertUserRole]    Script Date: 8/7/2024 20:57:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[SP_InsertUserRole]
	@userid int,
    @roleid int,
	@createdby NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @OutputTbl TABLE (UserRoleId INT)

    Insert Into dbo.[UserRoles]
		(	UserId,
			RoleId,
			RowStatus,
			CreatedDate,
			CreatedBy,
			ModifiedDate,
			ModifiedBy
		)
	OUTPUT INSERTED.UserRoleId into @OutputTbl
	Values
		(	@userid,
			@roleid,
			1,
			GETDATE(),
			@createdby,
			GETDATE(),
			@createdby
		)

	select UserRoleId, UserId, RoleId, RowStatus, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy from UserRoles
	where UserRoleId = (select UserRoleId from @OutputTbl)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_InsertUserShip]    Script Date: 8/7/2024 20:57:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[SP_InsertUserShip] 
	@userid int,
    @shipid int,
	@createdby NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @OutputTbl TABLE (UserShipId INT)

    Insert Into dbo.[UserShips]
		(	UserId,
			shipId,
			RowStatus,
			CreatedDate,
			CreatedBy,
			ModifiedDate,
			ModifiedBy
		)
	OUTPUT INSERTED.UserShipId into @OutputTbl
	Values
		(	@userid,
			@shipid,
			1,
			GETDATE(),
			@createdby,
			GETDATE(),
			@createdby
		)

	select UserShipId, UserId, shipId, RowStatus, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy from UserShips
	where UserShipId = (select UserShipId from @OutputTbl)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateRole]    Script Date: 8/7/2024 20:57:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[SP_UpdateRole] 
	@roleid	int,
	@rolename NVARCHAR(255),
	@isActive int,
	@modifiedby NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    Update dbo.[Roles]
	set RoleName = @rolename,
		RowStatus = @isActive,
		ModifiedDate = GETDATE(),
		ModifiedBy = @modifiedby
	where RoleId = @roleid

	select RoleId, RoleName, RowStatus, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy from Roles
	where RoleId = @roleid

END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateShip]    Script Date: 8/7/2024 20:57:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[SP_UpdateShip] 
	@shipid	int,
	@shipname NVARCHAR(255),
    @longitude decimal(9,6),
	@latitude decimal(9,6),
	@velocity decimal(10,2),
	@isActive int,
	@modifiedby NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    Update dbo.[Ships]
	set ShipName = @shipname,
		Longitude = @longitude,
		Latitude = @latitude,
		Velocity = @velocity,
		RowStatus = @isActive,
		ModifiedDate = GETDATE(),
		ModifiedBy = @modifiedby
	where ShipId = @shipid

	select ShipId, ShipName, Longitude, Latitude, Velocity, RowStatus, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy from Ships
	where ShipId = @shipid
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateUser]    Script Date: 8/7/2024 20:57:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[SP_UpdateUser] 
	@userid	int,
	@username NVARCHAR(255),
    @fullname NVARCHAR(255),
	@birthdate datetime,
	@isActive int,
	@modifiedby NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    Update dbo.[Users]
	set UserName = @username,
		FullName = @fullname,
		Birthdate = @birthdate,
		RowStatus = @isActive,
		ModifiedDate = GETDATE(),
		ModifiedBy = @modifiedby
	where UserId = @userid

	select UserId, UserName, FullName, Birthdate, RowStatus, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy from [Users]
	where UserId = @userid

END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateUserRole]    Script Date: 8/7/2024 20:57:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[SP_UpdateUserRole]
	@userroleid	int,
	@userid int,
    @roleid int,
	@isActive int,
	@modifiedby NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    Update dbo.[UserRoles]
	set UserId = @userid,
		RoleId = @roleid,
		RowStatus = @isActive,
		ModifiedDate = GETDATE(),
		ModifiedBy = @modifiedby
	where UserRoleId = @userroleid

	select UserRoleId, UserId, RoleId, RowStatus, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy from UserRoles
	where UserRoleId = @userroleid
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateUserShip]    Script Date: 8/7/2024 20:57:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[SP_UpdateUserShip] 
	@usershipid	int,
	@userid	int,
	@shipid int,
	@isActive int,
	@modifiedby NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    Update dbo.[UserShips]
	set UserId = @userid,
		shipId = @shipid,
		RowStatus = @isActive,
		ModifiedDate = GETDATE(),
		ModifiedBy = @modifiedby
	where UserShipid = @usershipid

	select UserShipId, UserId, shipId, RowStatus, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy from UserShips
	where UserShipId = @usershipid
END
GO
USE [master]
GO
ALTER DATABASE [AE] SET  READ_WRITE 
GO
