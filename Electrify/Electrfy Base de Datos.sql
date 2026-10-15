USE [master]
GO
/****** Object:  Database [Electrify]    Script Date: 9/10/2026 15:43:13 ******/
CREATE DATABASE [Electrify]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Electrify', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\Electrify.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Electrify_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\Electrify_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [Electrify] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Electrify].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Electrify] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Electrify] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Electrify] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Electrify] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Electrify] SET ARITHABORT OFF 
GO
ALTER DATABASE [Electrify] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Electrify] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Electrify] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Electrify] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Electrify] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Electrify] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Electrify] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Electrify] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Electrify] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Electrify] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Electrify] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Electrify] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Electrify] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Electrify] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Electrify] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Electrify] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Electrify] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Electrify] SET RECOVERY FULL 
GO
ALTER DATABASE [Electrify] SET  MULTI_USER 
GO
ALTER DATABASE [Electrify] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Electrify] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Electrify] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Electrify] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Electrify] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Electrify', N'ON'
GO
ALTER DATABASE [Electrify] SET QUERY_STORE = OFF
GO
USE [Electrify]
GO
/****** Object:  User [alumno]    Script Date: 9/10/2026 15:43:13 ******/
CREATE USER [alumno] FOR LOGIN [alumno] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[Alerta]    Script Date: 9/10/2026 15:43:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Alerta](
	[IDAlerta] [int] IDENTITY(1,1) NOT NULL,
	[IDEmpresa] [int] NOT NULL,
	[IDDispositivo] [int] NOT NULL,
	[Mensaje] [varchar](300) NOT NULL,
	[Fecha] [datetime] NOT NULL,
	[Tipo] [varchar](50) NOT NULL,
	[Leida] [bit] NOT NULL,
 CONSTRAINT [PK_Alerta] PRIMARY KEY CLUSTERED 
(
	[IDAlerta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Consejo]    Script Date: 9/10/2026 15:43:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Consejo](
	[IDConsejo] [int] IDENTITY(1,1) NOT NULL,
	[Titulo] [varchar](100) NOT NULL,
	[Descripcion] [varchar](500) NOT NULL,
	[Categoria] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Consejo] PRIMARY KEY CLUSTERED 
(
	[IDConsejo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Consumo]    Script Date: 9/10/2026 15:43:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Consumo](
	[IDConsumo] [int] IDENTITY(1,1) NOT NULL,
	[IDDispositivo] [int] NOT NULL,
	[Fecha] [date] NOT NULL,
	[Hora] [time](7) NOT NULL,
	[ConsumoKWh] [decimal](10, 2) NOT NULL,
 CONSTRAINT [PK_Consumo] PRIMARY KEY CLUSTERED 
(
	[IDConsumo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dispositivo]    Script Date: 9/10/2026 15:43:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dispositivo](
	[IDDispositivo] [int] IDENTITY(1,1) NOT NULL,
	[IDSector] [int] NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Tipo] [varchar](50) NOT NULL,
	[Potencia] [decimal](10, 2) NULL,
	[Estado] [varchar](30) NOT NULL,
 CONSTRAINT [PK_Dispositivo] PRIMARY KEY CLUSTERED 
(
	[IDDispositivo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Empresa]    Script Date: 9/10/2026 15:43:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Empresa](
	[IDEmpresa] [int] IDENTITY(1,1) NOT NULL,
	[RazonSocial] [varchar](100) NOT NULL,
	[CUIT] [varchar](20) NULL,
	[Email] [varchar](100) NULL,
	[Telefono] [varchar](30) NULL,
 CONSTRAINT [PK_Empresa] PRIMARY KEY CLUSTERED 
(
	[IDEmpresa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Estrategia]    Script Date: 9/10/2026 15:43:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Estrategia](
	[IDEstrategia] [int] IDENTITY(1,1) NOT NULL,
	[IDEmpresa] [int] NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Descripcion] [varchar](500) NULL,
	[Objetivo] [varchar](300) NULL,
	[FechaInicio] [date] NOT NULL,
	[FechaFin] [date] NULL,
	[Estado] [varchar](30) NOT NULL,
 CONSTRAINT [PK_Estrategia] PRIMARY KEY CLUSTERED 
(
	[IDEstrategia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InformeConsumo]    Script Date: 9/10/2026 15:43:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InformeConsumo](
	[IDInforme] [int] IDENTITY(1,1) NOT NULL,
	[IDSede] [int] NOT NULL,
	[FechaInicio] [date] NOT NULL,
	[FechaFin] [date] NOT NULL,
	[ConsumoTotal] [decimal](10, 2) NOT NULL,
	[CostoEstimado] [decimal](12, 2) NOT NULL,
 CONSTRAINT [PK_InformeConsumo] PRIMARY KEY CLUSTERED 
(
	[IDInforme] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sector]    Script Date: 9/10/2026 15:43:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sector](
	[IDSector] [int] IDENTITY(1,1) NOT NULL,
	[IDSede] [int] NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Descripcion] [varchar](300) NULL,
 CONSTRAINT [PK_Sector] PRIMARY KEY CLUSTERED 
(
	[IDSector] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sede]    Script Date: 9/10/2026 15:43:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sede](
	[IDSede] [int] IDENTITY(1,1) NOT NULL,
	[IDEmpresa] [int] NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Direccion] [varchar](150) NOT NULL,
	[Ciudad] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Sede] PRIMARY KEY CLUSTERED 
(
	[IDSede] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 9/10/2026 15:43:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[IDUsuario] [int] IDENTITY(1,1) NOT NULL,
	[IDEmpresa] [int] NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Apellido] [varchar](50) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Contraseña] [varchar](100) NOT NULL,
	[Rol] [varchar](30) NOT NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[IDUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Alerta] ON 

INSERT [dbo].[Alerta] ([IDAlerta], [IDEmpresa], [IDDispositivo], [Mensaje], [Fecha], [Tipo], [Leida]) VALUES (2, 2, 4, N'Consumo superior al valor esperado', CAST(N'2026-10-01T10:15:00.000' AS DateTime), N'Consumo alto', 0)
INSERT [dbo].[Alerta] ([IDAlerta], [IDEmpresa], [IDDispositivo], [Mensaje], [Fecha], [Tipo], [Leida]) VALUES (3, 1, 1, N'Revisar el consumo del equipo', CAST(N'2026-10-01T12:30:00.000' AS DateTime), N'Mantenimiento', 0)
SET IDENTITY_INSERT [dbo].[Alerta] OFF
GO
SET IDENTITY_INSERT [dbo].[Consejo] ON 

INSERT [dbo].[Consejo] ([IDConsejo], [Titulo], [Descripcion], [Categoria]) VALUES (1, N'Apagar equipos sin uso', N'Apagar computadoras y equipos cuando termina la jornada laboral', N'Ahorro')
INSERT [dbo].[Consejo] ([IDConsejo], [Titulo], [Descripcion], [Categoria]) VALUES (2, N'Usar iluminacion LED', N'Reemplazar lamparas antiguas por opciones LED de bajo consumo', N'Iluminacion')
INSERT [dbo].[Consejo] ([IDConsejo], [Titulo], [Descripcion], [Categoria]) VALUES (3, N'Mantener equipos', N'Realizar mantenimiento periodico para mejorar la eficiencia energetica', N'Mantenimiento')
SET IDENTITY_INSERT [dbo].[Consejo] OFF
GO
SET IDENTITY_INSERT [dbo].[Consumo] ON 

INSERT [dbo].[Consumo] ([IDConsumo], [IDDispositivo], [Fecha], [Hora], [ConsumoKWh]) VALUES (1, 1, CAST(N'2026-10-01' AS Date), CAST(N'09:00:00' AS Time), CAST(2.10 AS Decimal(10, 2)))
INSERT [dbo].[Consumo] ([IDConsumo], [IDDispositivo], [Fecha], [Hora], [ConsumoKWh]) VALUES (2, 1, CAST(N'2026-10-01' AS Date), CAST(N'12:00:00' AS Time), CAST(2.40 AS Decimal(10, 2)))
INSERT [dbo].[Consumo] ([IDConsumo], [IDDispositivo], [Fecha], [Hora], [ConsumoKWh]) VALUES (3, 2, CAST(N'2026-10-01' AS Date), CAST(N'09:00:00' AS Time), CAST(0.65 AS Decimal(10, 2)))
INSERT [dbo].[Consumo] ([IDConsumo], [IDDispositivo], [Fecha], [Hora], [ConsumoKWh]) VALUES (4, 3, CAST(N'2026-10-01' AS Date), CAST(N'09:00:00' AS Time), CAST(1.10 AS Decimal(10, 2)))
INSERT [dbo].[Consumo] ([IDConsumo], [IDDispositivo], [Fecha], [Hora], [ConsumoKWh]) VALUES (5, 4, CAST(N'2026-10-01' AS Date), CAST(N'10:00:00' AS Time), CAST(6.80 AS Decimal(10, 2)))
INSERT [dbo].[Consumo] ([IDConsumo], [IDDispositivo], [Fecha], [Hora], [ConsumoKWh]) VALUES (6, 5, CAST(N'2026-10-01' AS Date), CAST(N'11:00:00' AS Time), CAST(0.45 AS Decimal(10, 2)))
SET IDENTITY_INSERT [dbo].[Consumo] OFF
GO
SET IDENTITY_INSERT [dbo].[Dispositivo] ON 

INSERT [dbo].[Dispositivo] ([IDDispositivo], [IDSector], [Nombre], [Tipo], [Potencia], [Estado]) VALUES (1, 1, N'Aire Acondicionado', N'Climatizacion', CAST(2.50 AS Decimal(10, 2)), N'Activo')
INSERT [dbo].[Dispositivo] ([IDDispositivo], [IDSector], [Nombre], [Tipo], [Potencia], [Estado]) VALUES (2, 1, N'Iluminacion Oficina', N'Iluminacion', CAST(0.80 AS Decimal(10, 2)), N'Activo')
INSERT [dbo].[Dispositivo] ([IDDispositivo], [IDSector], [Nombre], [Tipo], [Potencia], [Estado]) VALUES (3, 2, N'Servidor Principal', N'Informatica', CAST(1.20 AS Decimal(10, 2)), N'Activo')
INSERT [dbo].[Dispositivo] ([IDDispositivo], [IDSector], [Nombre], [Tipo], [Potencia], [Estado]) VALUES (4, 3, N'Motor Industrial', N'Motor', CAST(7.50 AS Decimal(10, 2)), N'Activo')
INSERT [dbo].[Dispositivo] ([IDDispositivo], [IDSector], [Nombre], [Tipo], [Potencia], [Estado]) VALUES (5, 4, N'Luz Deposito', N'Iluminacion', CAST(0.60 AS Decimal(10, 2)), N'Activo')
SET IDENTITY_INSERT [dbo].[Dispositivo] OFF
GO
SET IDENTITY_INSERT [dbo].[Empresa] ON 

INSERT [dbo].[Empresa] ([IDEmpresa], [RazonSocial], [CUIT], [Email], [Telefono]) VALUES (1, N'Electrify Soluciones SA', N'30712345678', N'contacto@electrify.com', N'1144556677')
INSERT [dbo].[Empresa] ([IDEmpresa], [RazonSocial], [CUIT], [Email], [Telefono]) VALUES (2, N'Industrias del Sur SRL', N'30787654321', N'info@industriasdelsur.com', N'1133224455')
SET IDENTITY_INSERT [dbo].[Empresa] OFF
GO
SET IDENTITY_INSERT [dbo].[Estrategia] ON 

INSERT [dbo].[Estrategia] ([IDEstrategia], [IDEmpresa], [Nombre], [Descripcion], [Objetivo], [FechaInicio], [FechaFin], [Estado]) VALUES (1, 1, N'Reducir consumo fuera de horario', N'Apagar equipos que no se utilizan al finalizar la jornada', N'Reducir el consumo electrico fuera del horario laboral', CAST(N'2026-10-01' AS Date), CAST(N'2026-12-31' AS Date), N'En curso')
INSERT [dbo].[Estrategia] ([IDEstrategia], [IDEmpresa], [Nombre], [Descripcion], [Objetivo], [FechaInicio], [FechaFin], [Estado]) VALUES (2, 2, N'Optimizar motores', N'Revisar el uso y mantenimiento de los motores industriales', N'Disminuir el consumo de energia en produccion', CAST(N'2026-10-01' AS Date), CAST(N'2027-03-31' AS Date), N'Planificada')
SET IDENTITY_INSERT [dbo].[Estrategia] OFF
GO
SET IDENTITY_INSERT [dbo].[InformeConsumo] ON 

INSERT [dbo].[InformeConsumo] ([IDInforme], [IDSede], [FechaInicio], [FechaFin], [ConsumoTotal], [CostoEstimado]) VALUES (1, 1, CAST(N'2026-10-01' AS Date), CAST(N'2026-10-31' AS Date), CAST(125.50 AS Decimal(10, 2)), CAST(18825.00 AS Decimal(12, 2)))
INSERT [dbo].[InformeConsumo] ([IDInforme], [IDSede], [FechaInicio], [FechaFin], [ConsumoTotal], [CostoEstimado]) VALUES (2, 2, CAST(N'2026-10-01' AS Date), CAST(N'2026-10-31' AS Date), CAST(980.75 AS Decimal(10, 2)), CAST(147112.50 AS Decimal(12, 2)))
SET IDENTITY_INSERT [dbo].[InformeConsumo] OFF
GO
SET IDENTITY_INSERT [dbo].[Sector] ON 

INSERT [dbo].[Sector] ([IDSector], [IDSede], [Nombre], [Descripcion]) VALUES (1, 1, N'Administracion', N'Oficinas administrativas y puestos de trabajo')
INSERT [dbo].[Sector] ([IDSector], [IDSede], [Nombre], [Descripcion]) VALUES (2, 1, N'Sala de Servidores', N'Equipos informaticos y redes')
INSERT [dbo].[Sector] ([IDSector], [IDSede], [Nombre], [Descripcion]) VALUES (3, 2, N'Produccion', N'Maquinas utilizadas en la linea de produccion')
INSERT [dbo].[Sector] ([IDSector], [IDSede], [Nombre], [Descripcion]) VALUES (4, 2, N'Deposito', N'Area de almacenamiento y logistica')
SET IDENTITY_INSERT [dbo].[Sector] OFF
GO
SET IDENTITY_INSERT [dbo].[Sede] ON 

INSERT [dbo].[Sede] ([IDSede], [IDEmpresa], [Nombre], [Direccion], [Ciudad]) VALUES (1, 1, N'Oficina Central', N'Av Siempre Viva 123', N'Buenos Aires')
INSERT [dbo].[Sede] ([IDSede], [IDEmpresa], [Nombre], [Direccion], [Ciudad]) VALUES (2, 2, N'Planta Industrial', N'Calle 9 Numero 450', N'Avellaneda')
SET IDENTITY_INSERT [dbo].[Sede] OFF
GO
SET IDENTITY_INSERT [dbo].[Usuario] ON 

INSERT [dbo].[Usuario] ([IDUsuario], [IDEmpresa], [Nombre], [Apellido], [Email], [Contraseña], [Rol]) VALUES (1, 1, N'Martin', N'Gomez', N'admin@electrify.com', N'Demo123', N'Administrador')
INSERT [dbo].[Usuario] ([IDUsuario], [IDEmpresa], [Nombre], [Apellido], [Email], [Contraseña], [Rol]) VALUES (2, 2, N'Lucia', N'Fernandez', N'lucia@industriasdelsur.com', N'Demo123', N'Usuario')
INSERT [dbo].[Usuario] ([IDUsuario], [IDEmpresa], [Nombre], [Apellido], [Email], [Contraseña], [Rol]) VALUES (3, 2, N'Pedro', N'Ramirez', N'pedro@industriasdelsur.com', N'Demo123', N'Tecnico')
SET IDENTITY_INSERT [dbo].[Usuario] OFF
GO
ALTER TABLE [dbo].[Alerta] ADD  CONSTRAINT [DF_Alerta_Leida]  DEFAULT ((0)) FOR [Leida]
GO
ALTER TABLE [dbo].[Alerta]  WITH CHECK ADD  CONSTRAINT [FK_Alerta_Dispositivo] FOREIGN KEY([IDDispositivo])
REFERENCES [dbo].[Dispositivo] ([IDDispositivo])
GO
ALTER TABLE [dbo].[Alerta] CHECK CONSTRAINT [FK_Alerta_Dispositivo]
GO
ALTER TABLE [dbo].[Alerta]  WITH CHECK ADD  CONSTRAINT [FK_Alerta_Empresa] FOREIGN KEY([IDEmpresa])
REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
ALTER TABLE [dbo].[Alerta] CHECK CONSTRAINT [FK_Alerta_Empresa]
GO
ALTER TABLE [dbo].[Consumo]  WITH CHECK ADD  CONSTRAINT [FK_Consumo_Dispositivo] FOREIGN KEY([IDDispositivo])
REFERENCES [dbo].[Dispositivo] ([IDDispositivo])
GO
ALTER TABLE [dbo].[Consumo] CHECK CONSTRAINT [FK_Consumo_Dispositivo]
GO
ALTER TABLE [dbo].[Dispositivo]  WITH CHECK ADD  CONSTRAINT [FK_Dispositivo_Sector] FOREIGN KEY([IDSector])
REFERENCES [dbo].[Sector] ([IDSector])
GO
ALTER TABLE [dbo].[Dispositivo] CHECK CONSTRAINT [FK_Dispositivo_Sector]
GO
ALTER TABLE [dbo].[Estrategia]  WITH CHECK ADD  CONSTRAINT [FK_Estrategia_Empresa] FOREIGN KEY([IDEmpresa])
REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
ALTER TABLE [dbo].[Estrategia] CHECK CONSTRAINT [FK_Estrategia_Empresa]
GO
ALTER TABLE [dbo].[InformeConsumo]  WITH CHECK ADD  CONSTRAINT [FK_InformeConsumo_Sede] FOREIGN KEY([IDSede])
REFERENCES [dbo].[Sede] ([IDSede])
GO
ALTER TABLE [dbo].[InformeConsumo] CHECK CONSTRAINT [FK_InformeConsumo_Sede]
GO
ALTER TABLE [dbo].[Sector]  WITH CHECK ADD  CONSTRAINT [FK_Sector_Sede] FOREIGN KEY([IDSede])
REFERENCES [dbo].[Sede] ([IDSede])
GO
ALTER TABLE [dbo].[Sector] CHECK CONSTRAINT [FK_Sector_Sede]
GO
ALTER TABLE [dbo].[Sede]  WITH CHECK ADD  CONSTRAINT [FK_Sede_Empresa] FOREIGN KEY([IDEmpresa])
REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
ALTER TABLE [dbo].[Sede] CHECK CONSTRAINT [FK_Sede_Empresa]
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Empresa] FOREIGN KEY([IDEmpresa])
REFERENCES [dbo].[Empresa] ([IDEmpresa])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Empresa]
GO
USE [master]
GO
ALTER DATABASE [Electrify] SET  READ_WRITE 
GO


