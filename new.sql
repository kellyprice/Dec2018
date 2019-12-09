USE [DTU]
GO
/****** Object:  Table [dbo].[ArcSight]    Script Date: 09/12/2019 10:23:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArcSight](
	[LogId] [int] IDENTITY(1,1) NOT NULL,
	[Process] [varchar](20) NOT NULL,
	[Action] [varchar](2000) NOT NULL,
	[Login] [varchar](200) NOT NULL,
	[ActionDate] [datetime] NOT NULL,
 CONSTRAINT [PK_ArcSight] PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[XMLFile]    Script Date: 09/12/2019 10:23:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[XMLFile](
	[FileId] [int] IDENTITY(1,1) NOT NULL,
	[Process] [varchar](20) NOT NULL,
	[XMLFile] [varbinary](max) NOT NULL,
	[Login] [varchar](200) NOT NULL,
	[ActionDate] [datetime] NOT NULL,
 CONSTRAINT [PK_XMLFile] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[Insert_ArcSight]    Script Date: 09/12/2019 10:23:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Insert_ArcSight]
(
	@process varchar(20),
	@action varchar(2000),
	@login varchar(200),
	@actiondate datetime
)
AS
BEGIN
	insert into ArcSight
	select @process,
	       @action,
		   @login,
		   @actiondate
END
GO
/****** Object:  StoredProcedure [dbo].[Insert_XMLFile]    Script Date: 09/12/2019 10:23:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[Insert_XMLFile]
(
	@process varchar(20),
	@xmlfile varbinary(max),
	@login varchar(200),
	@actiondate datetime
)
AS
BEGIN
	insert into XMLFile
	select @process,
	       @xmlfile,
		   @login,
		   @actiondate
END
GO
/****** Object:  Trigger [dbo].[CheckXMLFiles]    Script Date: 09/12/2019 10:23:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE trigger [dbo].[CheckXMLFiles] on [dbo].[XMLFile]
after insert
as
declare @actiondate datetime
declare @process varchar(20)
declare @sendxmlfile varbinary(max)
declare @login varchar(200)
declare @fileid int

select @fileid = FileId,
       @process = Process,
       @sendxmlfile = XMLFile,
       @actiondate = convert(varchar,ActionDate,23),
	   @login = [Login]
from   inserted

declare @xmlfile varchar(max)

if @process = 'Send'
begin
  select top 1
         @xmlfile = XMLFile
  from   XMLFile
  where  convert(varchar,ActionDate,23) = @actiondate and
         Process = 'Transform'
  order by FileId desc

  if coalesce(@sendxmlfile,'') != coalesce(@xmlfile,'')
  begin
    insert into ArcSight
	select 'FileCheck',
	       'Send and Transform files for FileId = ' + convert(varchar,@fileid) + ' do not match.',
		   @login,
		   getdate()
  end
  else
  begin
    insert into ArcSight
	select 'FileCheck',
	       'Send and Transform files for FileId = ' + convert(varchar,@fileid) + ' match.',
		   @login,
		   getdate()
  end
end

if @process = 'Send'
begin
  select top 1
         @xmlfile = XMLFile
  from   XMLFile
  where  convert(varchar,ActionDate,23) = @actiondate and
         Process = 'Validate'
  order by FileId desc

  if coalesce(@sendxmlfile,'') != coalesce(@xmlfile,'')
  begin
    insert into ArcSight
	select 'FileCheck',
	       'Send and Validate files for FileId = ' + convert(varchar,@fileid) + ' do not match.',
		   @login,
		   getdate()
  end
  else
  begin
    insert into ArcSight
	select 'FileCheck',
	       'Send and Validate files for FileId = ' + convert(varchar,@fileid) + ' match.',
		   @login,
		   getdate()
  end
end

if @process = 'Validate'
begin
  select top 1
         @xmlfile = XMLFile
  from   XMLFile
  where  convert(varchar,ActionDate,23) = @actiondate and
         Process = 'Transform'
  order by FileId desc

  if coalesce(@sendxmlfile,'') != coalesce(@xmlfile,'')
  begin
    insert into ArcSight
	select 'FileCheck',
	       'Validate and Transform files for FileId = ' + convert(varchar,@fileid) + ' do not match.',
		   @login,
		   getdate()
  end
  else
  begin
    insert into ArcSight
	select 'FileCheck',
	       'Validate and Transform files for FileId = ' + convert(varchar,@fileid) + ' match.',
		   @login,
		   getdate()
  end
end
GO
ALTER TABLE [dbo].[XMLFile] ENABLE TRIGGER [CheckXMLFiles]
GO
