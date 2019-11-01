USE [CAS]
GO
/****** Object:  Trigger [dbo].[Customer_Delete]    Script Date: 29/10/2019 16:05:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE trigger [dbo].[Customer_Delete] on [dbo].[Customer]
after delete
as
BEGIN TRY
  insert into Customer_DELETED
  select *,
         getdate()
  from   deleted
END TRY
BEGIN CATCH
END CATCH;
GO
ALTER TABLE [dbo].[Customer] ENABLE TRIGGER [Customer_Delete]
GO
/****** Object:  Trigger [dbo].[Customer_Condition_Delete]    Script Date: 29/10/2019 16:05:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE trigger [dbo].[Customer_Condition_Delete] on [dbo].[Customer_Condition]
after delete
as
BEGIN TRY
  insert into Customer_Condition_DELETED
  select *,
         getdate()
  from   deleted
END TRY
BEGIN CATCH
END CATCH;
GO
ALTER TABLE [dbo].[Customer_Condition] ENABLE TRIGGER [Customer_Condition_Delete]
GO
/****** Object:  Trigger [dbo].[Customer_Library_Delete]    Script Date: 29/10/2019 16:05:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE trigger [dbo].[Customer_Library_Delete] on [dbo].[Customer_Library]
after delete
as
BEGIN TRY
  insert into Customer_Library_DELETED
  select *,
         getdate()
  from   deleted
END TRY
BEGIN CATCH
END CATCH;
GO
ALTER TABLE [dbo].[Customer_Library] ENABLE TRIGGER [Customer_Library_Delete]
GO
/****** Object:  Trigger [dbo].[Customer_Status_History_Insert]    Script Date: 29/10/2019 16:05:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE trigger [dbo].[Customer_Status_History_Insert] on [dbo].[Customer_Status_History]
after insert
as
BEGIN TRY
  declare @action varchar(50) = ( select [Action] from inserted )

  if @action = 'Customer Condition Delete'
  begin
    insert into Change_History
    select b.Group_ID, a.Customer_ID, 'Customer_Condition', c.Customer_Condition_ID, 'DELETE', 'DELETE', 'DELETE', 'N/A', a.Datetime_Of_Action, a.Username
    from   inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Customer_Condition_DELETED c on b.Customer_ID = c.Customer_ID
    where  Customer_Condition_ID = ( select top 1 Customer_Condition_ID from Customer_Condition_DELETED order by Deleted_On desc )
  end
  else if @action = 'Customer Library Delete'
  begin
    insert into Change_History
    select b.Group_ID, a.Customer_ID, 'Customer_Library', c.[File_ID], 'DELETE', 'DELETE', 'DELETE', 'N/A', a.Datetime_Of_Action, a.Username
    from   inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Customer_Library_DELETED c on b.Customer_ID = c.Customer_ID
    where  [File_ID] = ( select top 1 [File_ID] from Customer_Library_DELETED order by Deleted_On desc )
  end
  else if @action = 'Facility Delete'
  begin
    insert into Change_History
    select b.Group_ID, a.Customer_ID, 'Facility', c.Facility_ID, 'DELETE', 'DELETE', 'DELETE', 'N/A', a.Datetime_Of_Action, a.Username
    from   inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility_DELETED c on b.Customer_ID = c.Customer_ID
    where  Facility_ID = ( select top 1 Facility_ID from Facility_DELETED order by Deleted_On desc )
  end
  else if @action = 'Facility Condition Delete'
  begin
    insert into Change_History 
    select b.Group_ID, a.Customer_ID, 'Facility_Condition', d.Facility_Condition_ID, 'DELETE', 'DELETE', 'DELETE', 'N/A', a.Datetime_Of_Action, a.Username
    from   inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID inner join Facility_Condition_DELETED d on c.Facility_ID = d.Facility_ID
    where  Facility_Condition_ID = ( select top 1 Facility_Condition_ID from Facility_Condition_DELETED order by Deleted_On desc )
  end
  else if @action = 'Document Delete'
  begin
    insert into Change_History
    select b.Group_ID, a.Customer_ID, 'Document', c.Document_ID, 'DELETE', 'DELETE', 'DELETE', 'N/A', a.Datetime_Of_Action, a.Username
    from   inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Document_DELETED c on b.Customer_ID = c.Customer_ID
    where  Document_ID = ( select top 1 Document_ID from Document_DELETED order by Deleted_On desc )
  end
  else if @action = 'Document Condition Delete'
  begin
    insert into Change_History
    select b.Group_ID, a.Customer_ID, 'Document_Condition', d.Document_Condition_ID, 'DELETE', 'DELETE', 'DELETE', 'N/A', a.Datetime_Of_Action, a.Username
    from   inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Document c on b.Customer_ID = c.Customer_ID inner join Document_Condition_DELETED d on c.Document_ID = d.Document_ID
    where  Document_Condition_ID = ( select top 1 Document_Condition_ID from Document_Condition_DELETED order by Deleted_On desc )
  end
  else if @action = 'Security Delete'
  begin
    insert into Change_History
    select b.Group_ID, a.Customer_ID, 'Security', c.Security_ID, 'DELETE', 'DELETE', 'DELETE', 'N/A', a.Datetime_Of_Action, a.Username
    from   inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Security_DELETED c on b.Customer_ID = c.Customer_ID
    where  Security_ID = ( select top 1 Security_ID from Security_DELETED order by Deleted_On desc )
  end
  else if @action = 'Security Condition Delete'
  begin
    insert into Change_History
    select b.Group_ID, a.Customer_ID, 'Security_Condition', d.Security_Condition_ID, 'DELETE', 'DELETE', 'DELETE', 'N/A', a.Datetime_Of_Action, a.Username
    from   inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join [Security] c on b.Customer_ID = c.Customer_ID inner join Security_Condition_DELETED d on c.Security_ID = d.Security_ID
    where  Security_Condition_ID = ( select top 1 Security_Condition_ID from Security_Condition_DELETED order by Deleted_On desc )
  end
  else if @action = 'Facility Document Link Delete'
  begin
    insert into Change_History
    select b.Group_ID, a.Customer_ID, 'Facility_X_Document', d.Fac_X_Doc_ID, 'DELETE', 'DELETE', 'DELETE', 'N/A', a.Datetime_Of_Action, a.Username
    from   inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID inner join Facility_X_Document_DELETED d on c.Facility_ID = d.Facility_ID
    where  Fac_X_Doc_ID = ( select top 1 Fac_X_Doc_ID from Facility_X_Document_DELETED order by Deleted_On desc )
  end
  else if @action = 'Document Security Link Delete'
  begin
    insert into Change_History
    select b.Group_ID, a.Customer_ID, 'Document_X_Security', d.Doc_X_Sec_ID, 'DELETE', 'DELETE', 'DELETE', 'N/A', a.Datetime_Of_Action, a.Username
    from   inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Document c on b.Customer_ID = c.Customer_ID inner join Document_X_Security_DELETED d on c.Document_ID = d.Document_ID
    where  Doc_X_Sec_ID = ( select top 1 Doc_X_Sec_ID from Document_X_Security_DELETED order by Deleted_On desc )
  end
  else if @action = 'New Customer'
  begin
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer', b.Customer_ID, 'Group_ID', null, Group_ID, 'Int32', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer', b.Customer_ID, 'Customer_Name', null, Customer_Name, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer', b.Customer_ID, 'CIF_Number', null, CIF_Number, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer', b.Customer_ID, 'Account_No', null, Account_No, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer', b.Customer_ID, 'Type_of_Entity', null, Type_of_Entity, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer', b.Customer_ID, 'Date_of_Relationship', null, Date_of_Relationship, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer', b.Customer_ID, 'Country_of_Domicile', null, Country_of_Domicile, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer', b.Customer_ID, 'Country_of_Risk', null, Country_of_Risk, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer', b.Customer_ID, 'Special_Category_Connected_Accounts', null, Special_Category_Connected_Accounts, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer', b.Customer_ID, 'Industry_SIC_Code', null, Industry_SIC_Code, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer', b.Customer_ID, 'Relationship_Officer', null, Relationship_Officer, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer', b.Customer_ID, 'NBAD_Internal_Rating_Current', null, NBAD_Internal_Rating_Current, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer', b.Customer_ID, 'NBAD_Internal_Rating_Proposed', null, NBAD_Internal_Rating_Proposed, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer', b.Customer_ID, 'Rating_Model_Used', null, Rating_Model_Used, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer', b.Customer_ID, 'Last_Annual_Review_Date', null, Last_Annual_Review_Date, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer', b.Customer_ID, 'Next_Annual_Review_Date', null, Next_Annual_Review_Date, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer', b.Customer_ID, 'Activity', null, Activity, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer', b.Customer_ID, 'Date_of_Establishment', null, Date_of_Establishment, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer', b.Customer_ID, 'Account_Opening_Date', null, Account_Opening_Date, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer', b.Customer_ID, 'Industry_description', null, Industry_description, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer', b.Customer_ID, 'Asset_Class', null, Asset_Class, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer', b.Customer_ID, 'Interim_Review_Date', null, Interim_Review_Date, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer', b.Customer_ID, 'Brief_History__Background', null, Brief_History__Background, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer', b.Customer_ID, 'Last_Call_Date', null, Last_Call_Date, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer', b.Customer_ID, 'Last_Call_Notes', null, Last_Call_Notes, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer', b.Customer_ID, 'Customer_Status', null, Customer_Status, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer', b.Customer_ID, 'Customer_Notes', null, Customer_Notes, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID
  end
  else if @action = 'Customer Condition Addition'
  begin
    declare @customer_condition_id int = (select max(Customer_Condition_ID) from Customer_Condition )  
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer_Condition', Customer_Condition_ID, 'Customer_ID', null, c.Customer_ID, 'Int32', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Customer_Condition c on b.Customer_ID = c.Customer_ID where c.Customer_Condition_ID = @customer_condition_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer_Condition', Customer_Condition_ID, 'KYC_Check_Status', null, KYC_Check_Status, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Customer_Condition c on b.Customer_ID = c.Customer_ID where c.Customer_Condition_ID = @customer_condition_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer_Condition', Customer_Condition_ID, 'Date_KYC_Checks_Completed', null, Date_KYC_Checks_Completed, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Customer_Condition c on b.Customer_ID = c.Customer_ID where c.Customer_Condition_ID = @customer_condition_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer_Condition', Customer_Condition_ID, 'Condition', null, Condition, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Customer_Condition c on b.Customer_ID = c.Customer_ID where c.Customer_Condition_ID = @customer_condition_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer_Condition', Customer_Condition_ID, 'Date_Conditions_Satisfied', null, Date_Conditions_Satisfied, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Customer_Condition c on b.Customer_ID = c.Customer_ID where c.Customer_Condition_ID = @customer_condition_id
  end
  else if @action = 'Customer Library Addition'
  begin
    declare @file_id int = (select max([File_ID]) from Customer_Library )
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer_Library', [File_ID], 'Customer_ID', null, c.Customer_ID, 'Int32', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Customer_Library c on b.Customer_ID = c.Customer_ID where c.[File_ID] = @file_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer_Library', [File_ID], 'File_Name', null, File_Name, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Customer_Library c on b.Customer_ID = c.Customer_ID where c.[File_ID] = @file_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer_Library', [File_ID], 'File_Description', null, File_Description, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Customer_Library c on b.Customer_ID = c.Customer_ID where c.[File_ID] = @file_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Customer_Library', [File_ID], 'Date_Added', null, Date_Added, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Customer_Library c on b.Customer_ID = c.Customer_ID where c.[File_ID] = @file_id
  end
  else if @action = 'Facility Addition'
  begin
    declare @facility_id int = (select max(Facility_ID) from Facility )
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility', Facility_ID, 'Customer_ID', null, c.Customer_ID, 'Int32', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID where c.Facility_ID = @facility_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility', Facility_ID, 'Type_of_Facility', null, Type_of_Facility, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID where c.Facility_ID = @facility_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility', Facility_ID, 'Facility_Description', null, Facility_Description, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID where c.Facility_ID = @facility_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility', Facility_ID, 'Seniority', null, Seniority, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID where c.Facility_ID = @facility_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility', Facility_ID, 'Facility_Rating', null, Facility_Rating, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID where c.Facility_ID = @facility_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility', Facility_ID, 'Currency', null, Currency, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID where c.Facility_ID = @facility_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility', Facility_ID, 'Current_Limit', null, Current_Limit, 'Decimal', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID where c.Facility_ID = @facility_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility', Facility_ID, 'Proposed_Limit', null, Proposed_Limit, 'Decimal', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID where c.Facility_ID = @facility_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility', Facility_ID, 'Tenor__Final_Maturity', null, Tenor__Final_Maturity, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID where c.Facility_ID = @facility_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility', Facility_ID, 'Pricing_Base_Rate', null, Pricing_Base_Rate, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID where c.Facility_ID = @facility_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility', Facility_ID, 'Pricing_Margin', null, Pricing_Margin, 'Decimal', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID where c.Facility_ID = @facility_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility', Facility_ID, 'Fees_Payable', null, Fees_Payable, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID where c.Facility_ID = @facility_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility', Facility_ID, 'Date_of_Approval', null, Date_of_Approval, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID where c.Facility_ID = @facility_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility', Facility_ID, 'Facility_Code', null, Facility_Code, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID where c.Facility_ID = @facility_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility', Facility_ID, 'Funded_Outstanding', null, Funded_Outstanding, 'Decimal', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID where c.Facility_ID = @facility_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility', Facility_ID, 'Unfunded_Outstanding', null, Unfunded_Outstanding, 'Decimal', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID where c.Facility_ID = @facility_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility', Facility_ID, 'Coverage_Percent', null, Coverage_Percent, 'Decimal', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID where c.Facility_ID = @facility_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility', Facility_ID, 'LTV', null, LTV, 'Decimal', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID where c.Facility_ID = @facility_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility', Facility_ID, 'Repayment_Schedule', null, Repayment_Schedule, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID where c.Facility_ID = @facility_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility', Facility_ID, 'Interest__Fee_Payment_Schedule', null, Interest__Fee_Payment_Schedule, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID where c.Facility_ID = @facility_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility', Facility_ID, 'Account_Closure_Date', null, Account_Closure_Date, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID where c.Facility_ID = @facility_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility', Facility_ID, 'RAROC', null, RAROC, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID where c.Facility_ID = @facility_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility', Facility_ID, 'Purpose', null, Purpose, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID where c.Facility_ID = @facility_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility', Facility_ID, 'Agent_Name', null, Agent_Name, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID where c.Facility_ID = @facility_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility', Facility_ID, 'Guarantor', null, Guarantor, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID where c.Facility_ID = @facility_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility', Facility_ID, 'Loan_Number', null, Loan_Number, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID where c.Facility_ID = @facility_id
  end
  else if @action = 'Facility Condition Addition'
  begin
    declare @facility_condition_id int = (select max(Facility_Condition_ID) from Facility_Condition )
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility_Condition', Facility_Condition_ID, 'Facility_ID', null, d.Facility_ID, 'Int32', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID inner join Facility_Condition d on c.Facility_ID = d.Facility_ID where d.Facility_Condition_ID = @facility_condition_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility_Condition', Facility_Condition_ID, 'Condition_Name', null, Condition_Name, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID inner join Facility_Condition d on c.Facility_ID = d.Facility_ID where d.Facility_Condition_ID = @facility_condition_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility_Condition', Facility_Condition_ID, 'Condition_Type', null, Condition_Type, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID inner join Facility_Condition d on c.Facility_ID = d.Facility_ID where d.Facility_Condition_ID = @facility_condition_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility_Condition', Facility_Condition_ID, 'Last_Review_Date', null, Last_Review_Date, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID inner join Facility_Condition d on c.Facility_ID = d.Facility_ID where d.Facility_Condition_ID = @facility_condition_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility_Condition', Facility_Condition_ID, 'Next_Review_Date', null, Next_Review_Date, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID inner join Facility_Condition d on c.Facility_ID = d.Facility_ID where d.Facility_Condition_ID = @facility_condition_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility_Condition', Facility_Condition_ID, 'Test_Value', null, Test_Value, 'Decimal', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID inner join Facility_Condition d on c.Facility_ID = d.Facility_ID where d.Facility_Condition_ID = @facility_condition_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility_Condition', Facility_Condition_ID, 'Actual_Value', null, Actual_Value, 'Decimal', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID inner join Facility_Condition d on c.Facility_ID = d.Facility_ID where d.Facility_Condition_ID = @facility_condition_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility_Condition', Facility_Condition_ID, 'Facility_Status', null, Facility_Status, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID inner join Facility_Condition d on c.Facility_ID = d.Facility_ID where d.Facility_Condition_ID = @facility_condition_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility_Condition', Facility_Condition_ID, 'Last_Formal_Valuation_Date', null, Last_Formal_Valuation_Date, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID inner join Facility_Condition d on c.Facility_ID = d.Facility_ID where d.Facility_Condition_ID = @facility_condition_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility_Condition', Facility_Condition_ID, 'Last_Estimated_Valuation_Date', null, Last_Estimated_Valuation_Date, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID inner join Facility_Condition d on c.Facility_ID = d.Facility_ID where d.Facility_Condition_ID = @facility_condition_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility_Condition', Facility_Condition_ID, 'Last_Estimated_LTV', null, Last_Estimated_LTV, 'Decimal', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID inner join Facility_Condition d on c.Facility_ID = d.Facility_ID where d.Facility_Condition_ID = @facility_condition_id
  end
  else if @action = 'Document Addition'
  begin
    declare @document_id int = (select max(Document_ID) from Document )
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Document', Document_ID, 'Type_Of_Document', null, Type_Of_Document, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Document c on b.Customer_ID = c.Customer_ID where c.Document_ID = @document_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Document', Document_ID, 'Documentation_Description', null, Documentation_Description, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Document c on b.Customer_ID = c.Customer_ID where c.Document_ID = @document_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Document', Document_ID, 'Documentation_Status', null, Documentation_Status, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Document c on b.Customer_ID = c.Customer_ID where c.Document_ID = @document_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Document', Document_ID, 'Date_Drafted_and_Signed_Internally', null, Date_Drafted_and_Signed_Internally, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Document c on b.Customer_ID = c.Customer_ID where c.Document_ID = @document_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Document', Document_ID, 'Internal_Confirmation_That_Terms_Match_Sanction', null, Internal_Confirmation_That_Terms_Match_Sanction, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Document c on b.Customer_ID = c.Customer_ID where c.Document_ID = @document_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Document', Document_ID, 'Date_Sent_To_Customer', null, Date_Sent_To_Customer, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Document c on b.Customer_ID = c.Customer_ID where c.Document_ID = @document_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Document', Document_ID, 'Date_Executed', null, Date_Executed, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Document c on b.Customer_ID = c.Customer_ID where c.Document_ID = @document_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Document', Document_ID, 'Confirmation_That_Signatures_Match_Mandate', null, Confirmation_That_Signatures_Match_Mandate, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Document c on b.Customer_ID = c.Customer_ID where c.Document_ID = @document_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Document', Document_ID, 'Expiry__Review_Date_of_Document', null, Expiry__Review_Date_of_Document, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Document c on b.Customer_ID = c.Customer_ID where c.Document_ID = @document_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Document', Document_ID, 'Date_To_Safe', null, Date_To_Safe, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Document c on b.Customer_ID = c.Customer_ID where c.Document_ID = @document_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Document', Document_ID, 'Customer_ID', null, c.Customer_ID, 'Int32', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Document c on b.Customer_ID = c.Customer_ID where c.Document_ID = @document_id
  end
  else if @action = 'Document Condition Addition'
  begin
    declare @document_condition_id int = (select max(Document_Condition_ID) from Document_Condition )
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Document_Condition', Document_Condition_ID, 'Document_ID', null, d.Document_ID, 'Int32', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Document c on b.Customer_ID = c.Customer_ID inner join Document_Condition d on c.Document_ID = d.Document_ID where d.Document_Condition_ID = @document_condition_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Document_Condition', Document_Condition_ID, 'Condition_Type', null, Condition_Type, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Document c on b.Customer_ID = c.Customer_ID inner join Document_Condition d on c.Document_ID = d.Document_ID where d.Document_Condition_ID = @document_condition_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Document_Condition', Document_Condition_ID, 'Condition_Name', null, Condition_Name, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Document c on b.Customer_ID = c.Customer_ID inner join Document_Condition d on c.Document_ID = d.Document_ID where d.Document_Condition_ID = @document_condition_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Document_Condition', Document_Condition_ID, 'Date_Satisfied', null, Date_Satisfied, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Document c on b.Customer_ID = c.Customer_ID inner join Document_Condition d on c.Document_ID = d.Document_ID where d.Document_Condition_ID = @document_condition_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Document_Condition', Document_Condition_ID, 'Date_of_Next_Condition_Check', null, Date_of_Next_Condition_Check, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Document c on b.Customer_ID = c.Customer_ID inner join Document_Condition d on c.Document_ID = d.Document_ID where d.Document_Condition_ID = @document_condition_id
  end
  else if @action = 'Security Addition'
  begin
    declare @security_id int = (select max(Security_ID) from [Security] )
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Security', Security_ID, 'Security_Description', null, Security_Description, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join [Security] c on b.Customer_ID = c.Customer_ID where c.Security_ID = @security_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Security', Security_ID, 'Permissible_Collateral__Haircuts_Under_Traded_Product_Collateral_Agreement', null, Permissible_Collateral__Haircuts_Under_Traded_Product_Collateral_Agreement, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join [Security] c on b.Customer_ID = c.Customer_ID where c.Security_ID = @security_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Security', Security_ID, 'Date_of_Collateral_Perfection', null, Date_of_Collateral_Perfection, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join [Security] c on b.Customer_ID = c.Customer_ID where c.Security_ID = @security_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Security', Security_ID, 'Location_of_Security__Collateral', null, Location_of_Security__Collateral, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join [Security] c on b.Customer_ID = c.Customer_ID where c.Security_ID = @security_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Security', Security_ID, 'Original_Valuation_Currency', null, Original_Valuation_Currency, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join [Security] c on b.Customer_ID = c.Customer_ID where c.Security_ID = @security_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Security', Security_ID, 'Original_Valuation', null, Original_Valuation, 'Decimal', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join [Security] c on b.Customer_ID = c.Customer_ID where c.Security_ID = @security_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Security', Security_ID, 'Basis_for_Original_Valuation', null, Basis_for_Original_Valuation, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join [Security] c on b.Customer_ID = c.Customer_ID where c.Security_ID = @security_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Security', Security_ID, 'Date_of_Last_Valuation', null, Date_of_Last_Valuation, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join [Security] c on b.Customer_ID = c.Customer_ID where c.Security_ID = @security_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Security', Security_ID, 'Index_Basis_Used_For_Revaluation', null, Index_Basis_Used_For_Revaluation, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join [Security] c on b.Customer_ID = c.Customer_ID where c.Security_ID = @security_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Security', Security_ID, 'Current_Estimate_Value_Currency', null, Current_Estimate_Value_Currency, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join [Security] c on b.Customer_ID = c.Customer_ID where c.Security_ID = @security_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Security', Security_ID, 'Current_Estimate_Value', null, Current_Estimate_Value, 'Decimal', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join [Security] c on b.Customer_ID = c.Customer_ID where c.Security_ID = @security_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Security', Security_ID, 'Date_of_Next_Valuation', null, Date_of_Next_Valuation, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join [Security] c on b.Customer_ID = c.Customer_ID where c.Security_ID = @security_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Security', Security_ID, 'Customer_ID', null, c.Customer_ID, 'Int32', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join [Security] c on b.Customer_ID = c.Customer_ID where c.Security_ID = @security_id
  end
  else if @action = 'Security Condition Addition'
  begin
    declare @security_condition_id int = (select max(Security_Condition_ID) from Security_Condition )
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Security_Condition', Security_Condition_ID, 'Security_ID', null, d.Security_ID, 'Int32', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Security c on b.Customer_ID = c.Customer_ID inner join Security_Condition d on c.Security_ID = d.Security_ID where d.Security_Condition_ID = @security_condition_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Security_Condition', Security_Condition_ID, 'Security_Condition', null, Security_Condition, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Security c on b.Customer_ID = c.Customer_ID inner join Security_Condition d on c.Security_ID = d.Security_ID where d.Security_Condition_ID = @security_condition_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Security_Condition', Security_Condition_ID, 'Date_Conditions_Satisfied', null, Date_Conditions_Satisfied, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Security c on b.Customer_ID = c.Customer_ID inner join Security_Condition d on c.Security_ID = d.Security_ID where d.Security_Condition_ID = @security_condition_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Security_Condition', Security_Condition_ID, 'Valuation_Date', null, Valuation_Date, 'DateTime', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Security c on b.Customer_ID = c.Customer_ID inner join Security_Condition d on c.Security_ID = d.Security_ID where d.Security_Condition_ID = @security_condition_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Security_Condition', Security_Condition_ID, 'Perfection_Status', null, Perfection_Status, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Security c on b.Customer_ID = c.Customer_ID inner join Security_Condition d on c.Security_ID = d.Security_ID where d.Security_Condition_ID = @security_condition_id
  end
  else if @action = 'Facility Document Link Addition'
  begin
    declare @fac_x_doc_id int = (select max(Fac_X_Doc_ID) from Facility_X_Document )
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility_X_Document', Fac_X_Doc_ID, 'Facility_ID', null, d.Facility_ID, 'Int32', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID inner join Facility_X_Document d on c.Facility_ID = d.Facility_ID where d.Fac_X_Doc_ID = @fac_x_doc_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Facility_X_Document', Fac_X_Doc_ID, 'Document_ID', null, Document_ID, 'Int32', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Facility c on b.Customer_ID = c.Customer_ID inner join Facility_X_Document d on c.Facility_ID = d.Facility_ID where d.Fac_X_Doc_ID = @fac_x_doc_id
  end
  else if @action = 'Document Security Link Addition'
  begin
    declare @doc_x_sec_id int = (select max(Doc_X_Sec_ID) from Document_X_Security )
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Document_X_Security', Doc_X_Sec_ID, 'Document_ID', null, d.Document_ID, 'Int32', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Document c on b.Customer_ID = c.Customer_ID inner join Document_X_Security d on c.Document_ID = d.Document_ID where d.Doc_X_Sec_ID = @doc_x_sec_id
    insert into Change_History select b.Group_ID, b.Customer_ID, 'Document_X_Security', Doc_X_Sec_ID, 'Security_ID', null, Security_ID, 'Int32', a.Datetime_Of_Action, a.Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID inner join Document c on b.Customer_ID = c.Customer_ID inner join Document_X_Security d on c.Document_ID = d.Document_ID where d.Doc_X_Sec_ID = @doc_x_sec_id
  end
  else if @action = 'Customer_Update'
  begin
    declare @customer_id int
    declare @customer_status_history_id int
    declare @status varchar(50)
    select @customer_id = Customer_ID, @customer_status_history_id = Customer_Status_History_ID, @status = [Status] from inserted
    declare @old_status varchar(50) = ( select top 1 [Status] from Customer_Status_History where Customer_ID = @customer_id and Customer_Status_History_ID != @customer_status_history_id order by Customer_Status_History_ID desc )
    if coalesce(@old_status,'') != @status
      insert into Change_History select b.Group_ID, a.Customer_ID, 'Customer', a.Customer_ID, 'Customer_Status', coalesce(@old_status,''), @status, 'String', Datetime_Of_Action, Username from inserted a inner join Customer b on a.Customer_ID = b.Customer_ID
  end
END TRY
BEGIN CATCH
END CATCH;
GO
ALTER TABLE [dbo].[Customer_Status_History] ENABLE TRIGGER [Customer_Status_History_Insert]
GO
/****** Object:  Trigger [dbo].[Document_Delete]    Script Date: 29/10/2019 16:05:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE trigger [dbo].[Document_Delete] on [dbo].[Document]
after delete
as
BEGIN TRY
  insert into Document_DELETED
  select *,
         getdate()
  from   deleted
END TRY
BEGIN CATCH
END CATCH;
GO
ALTER TABLE [dbo].[Document] ENABLE TRIGGER [Document_Delete]
GO
/****** Object:  Trigger [dbo].[Document_Condition_Delete]    Script Date: 29/10/2019 16:05:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE trigger [dbo].[Document_Condition_Delete] on [dbo].[Document_Condition]
after delete
as
BEGIN TRY
  insert into Document_Condition_DELETED
  select *,
         getdate()
  from   deleted
END TRY
BEGIN CATCH
END CATCH;
GO
ALTER TABLE [dbo].[Document_Condition] ENABLE TRIGGER [Document_Condition_Delete]
GO
/****** Object:  Trigger [dbo].[Document_X_Security_Delete]    Script Date: 29/10/2019 16:05:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE trigger [dbo].[Document_X_Security_Delete] on [dbo].[Document_X_Security]
after delete
as
BEGIN TRY
  insert into Document_X_Security_DELETED
  select *,
         getdate()
  from   deleted
END TRY
BEGIN CATCH
END CATCH;
GO
ALTER TABLE [dbo].[Document_X_Security] ENABLE TRIGGER [Document_X_Security_Delete]
GO
/****** Object:  Trigger [dbo].[Facility_Delete]    Script Date: 29/10/2019 16:05:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE trigger [dbo].[Facility_Delete] on [dbo].[Facility]
after delete
as
BEGIN TRY
  insert into Facility_DELETED
  select *,
         getdate()
  from   deleted
END TRY
BEGIN CATCH
END CATCH;
GO
ALTER TABLE [dbo].[Facility] ENABLE TRIGGER [Facility_Delete]
GO
/****** Object:  Trigger [dbo].[Facility_Condition_Delete]    Script Date: 29/10/2019 16:05:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE trigger [dbo].[Facility_Condition_Delete] on [dbo].[Facility_Condition]
after delete
as
BEGIN TRY
  insert into Facility_Condition_DELETED
  select *,
         getdate()
  from   deleted
END TRY
BEGIN CATCH
END CATCH;
GO
ALTER TABLE [dbo].[Facility_Condition] ENABLE TRIGGER [Facility_Condition_Delete]
GO
/****** Object:  Trigger [dbo].[Facility_X_Document_Delete]    Script Date: 29/10/2019 16:05:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE trigger [dbo].[Facility_X_Document_Delete] on [dbo].[Facility_X_Document]
after delete
as
BEGIN TRY
  insert into Facility_X_Document_DELETED
  select *,
         getdate()
  from   deleted
END TRY
BEGIN CATCH
END CATCH;
GO
ALTER TABLE [dbo].[Facility_X_Document] ENABLE TRIGGER [Facility_X_Document_Delete]
GO
/****** Object:  Trigger [dbo].[Group_Delete]    Script Date: 29/10/2019 16:05:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE trigger [dbo].[Group_Delete] on [dbo].[Group]
after delete
as
BEGIN TRY
  insert into Group_DELETED
  select *,
         getdate()
  from   deleted
END TRY
BEGIN CATCH
END CATCH;
GO
ALTER TABLE [dbo].[Group] ENABLE TRIGGER [Group_Delete]
GO
/****** Object:  Trigger [dbo].[Group_Status_History_Insert]    Script Date: 29/10/2019 16:05:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE trigger [dbo].[Group_Status_History_Insert] on [dbo].[Group_Status_History]
after insert
as
declare @action varchar(50) = ( select [Action] from inserted )

if @action = 'GROUP DELETED'
begin
  insert into Change_History
  select Group_ID, 0, 'Group', Group_ID, 'DELETE', 'DELETE', 'DELETE', 'N/A', Datetime_Of_Action, Username
  from   inserted
  where  Group_ID = ( select top 1 Group_ID from Group_DELETED order by Deleted_On desc )
end
else if @action = 'Customer Delete'
begin
  insert into Change_History
  select b.Group_ID, b.Customer_ID, 'Customer', b.Customer_ID, 'DELETE', 'DELETE', 'DELETE', 'N/A', a.Datetime_Of_Action, a.Username
  from   inserted a inner join Customer_DELETED b on a.Group_ID = b.Group_ID
  where  Customer_ID = ( select top 1 Customer_ID from Customer_DELETED order by Deleted_On desc )
end
else if @action = 'New Group'
begin
  insert into Change_History select b.Group_ID, 0, 'Group', b.Group_ID, 'Group_Name', null, Group_Name, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join [Group] b on a.Group_ID = b.Group_ID
  insert into Change_History select b.Group_ID, 0, 'Group', b.Group_ID, 'Group_Account_Manager', null, Group_Account_Manager, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join [Group] b on a.Group_ID = b.Group_ID
  insert into Change_History select b.Group_ID, 0, 'Group', b.Group_ID, 'Group_CIF', null, Group_CIF, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join [Group] b on a.Group_ID = b.Group_ID
  insert into Change_History select b.Group_ID, 0, 'Group', b.Group_ID, 'Group_Status', null, Group_Status, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join [Group] b on a.Group_ID = b.Group_ID
  insert into Change_History select b.Group_ID, 0, 'Group', b.Group_ID, 'Group_Notes', null, Group_Notes, 'String', a.Datetime_Of_Action, a.Username from inserted a inner join [Group] b on a.Group_ID = b.Group_ID
end
else if @action = 'Group Updated'
begin
  declare @group_id int
  declare @group_status_history_id int
  declare @status varchar(50)
  select @group_id = Group_ID, @group_status_history_id = Group_Status_History_ID, @status = [Status] from inserted
  declare @old_status varchar(50) = ( select top 1 [Status] from Group_Status_History where Group_ID = @group_id and Group_Status_History_ID != @group_status_history_id order by Group_Status_History_ID desc )
  if coalesce(@old_status,'') != @status
    insert into Change_History select Group_ID, 0, 'Group', Group_ID, 'Group_Status', coalesce(@old_status,''), @status, 'String', Datetime_Of_Action, Username from inserted
end
GO
ALTER TABLE [dbo].[Group_Status_History] ENABLE TRIGGER [Group_Status_History_Insert]
GO
/****** Object:  Trigger [dbo].[Security_Delete]    Script Date: 29/10/2019 16:05:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE trigger [dbo].[Security_Delete] on [dbo].[Security]
after delete
as
BEGIN TRY
  insert into Security_DELETED
  select *,
         getdate()
  from   deleted
END TRY
BEGIN CATCH
END CATCH;
GO
ALTER TABLE [dbo].[Security] ENABLE TRIGGER [Security_Delete]
GO
/****** Object:  Trigger [dbo].[Security_Condition_Delete]    Script Date: 29/10/2019 16:05:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE trigger [dbo].[Security_Condition_Delete] on [dbo].[Security_Condition]
after delete
as
BEGIN TRY
  insert into Security_Condition_DELETED
  select *,
         getdate()
  from   deleted
END TRY
BEGIN CATCH
END CATCH;
GO
ALTER TABLE [dbo].[Security_Condition] ENABLE TRIGGER [Security_Condition_Delete]
GO
