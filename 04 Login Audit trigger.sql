USE [CAS]
GO
/****** Object:  Trigger [dbo].[ArcSight_Login_Audit]    Script Date: 29/10/2019 16:00:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER trigger [dbo].[ArcSight_Login_Audit] on [dbo].[Login_Audit]
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
from   inserted