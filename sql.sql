USE [NBAD]
GO

CREATE TABLE [dbo].[ArcSight](
	[ArcSightId] [int] IDENTITY(1,1) NOT NULL,
	[Change_History_ID] [int] NOT NULL,
	[Group_Name] [nvarchar](400) NULL,
	[Customer_Name] [nvarchar](400) NULL,
	[Entity] [nvarchar](400) NULL,
	[Action] [nvarchar](400) NULL,
	[Before_Value] [nvarchar](max) NULL,
	[After_Value] [nvarchar](max) NULL,
	[User] [nvarchar](400) NULL,
	[CreatedOn] [datetime] NULL,
 CONSTRAINT [PK_ArcSight] PRIMARY KEY CLUSTERED 
(
	[ArcSightId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

create trigger ArcSight_Change_History on dbo.Change_History
after insert
as
insert into ArcSight
select Change_History_ID,
       Group_Name,
	   Customer_Name,
	   a.Entity,
	   Field_Name,
	   Before_Value,
	   After_Value,
	   Username,
	   DateTime_Change_Logged
from   inserted a left outer join
       [Group] b on a.Group_ID = b.Group_ID left outer join
	   Customer c on a.Customer_ID = c.Customer_ID
go

create trigger ArcSight_Login_Audit on dbo.Login_Audit
after insert
as
insert into ArcSight
select Login_Audit_ID,
       null,
	   null,
	   null,
	   [Action],
	   null,
	   null,
	   Username [User],
	   Datetime_Of_Action CreatedOn
from   Login_Audit
go

USE [ACExtractDatabase]
GO

CREATE TABLE [dbo].[ArcSight](
	[ArcSightId] [int] IDENTITY(1,1) NOT NULL,
	[Action] [nvarchar](400) NULL,
	[Exception] [nvarchar](max) NULL,
	[ExceptionTrace] [nvarchar](max) NULL,
	[User] [nvarchar](400) NULL,
	[CreatedOn] [datetime] NULL,
 CONSTRAINT [PK_ArcSight] PRIMARY KEY CLUSTERED 
(
	[ArcSightId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

create trigger ARcSight_ACExLog on dbo.ACExLog
after insert
as
select ACExLogId,
       [Message],
	   ExceptionMessage,
       ExceptionStackTrace,
       CreatedBy,
       CreatedOn
from   inserted
go
