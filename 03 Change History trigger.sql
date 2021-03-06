USE [CAS]
GO
/****** Object:  Trigger [dbo].[ArcSight_Change_History]    Script Date: 30/10/2019 21:29:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER trigger [dbo].[ArcSight_Change_History] on [dbo].[Change_History]
after insert
as
delete
from   Change_History
where  Change_History_ID in ( select Change_History_ID
                              from   inserted
							  where  rtrim(ltrim(coalesce(Before_Value,''))) = '' and 
			                         rtrim(ltrim(coalesce(After_Value,''))) = '' )

insert into ArcSight
select Change_History_ID,
       coalesce(Group_CIF,''),
       coalesce(CIF_Number,''),
	   a.Entity,
	   Field_Name,
	   Before_Value,
	   After_Value,
	   Username,
	   DateTime_Change_Logged
from   inserted a left outer join
       [Group] b on a.Group_ID = b.Group_ID left outer join
	   Customer c on a.Customer_ID = c.Customer_ID
where  rtrim(ltrim(coalesce(Before_Value,''))) != '' or 
       rtrim(ltrim(coalesce(After_Value,''))) != ''