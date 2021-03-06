USE [CAS]
GO
/****** Object:  StoredProcedure [dbo].[sp_Update_Group_incStatusNotes]    Script Date: 31/10/2019 15:15:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Update_Group_incStatusNotes] 
	
	( @Group_ID INT = 0, 
	@Group_Name varchar(255) ,
	@Group_Account_Manager varchar(255),
	@Group_CIF varchar(255), @Group_Status varchar(255) ,
     @Group_Notes varchar(Max)
	  , @GroupId INT, @CustID INT --not needed
	  , @User varchar(50)
	   )
	 
AS
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @myERROR int -- Local @@ERROR
       , @myRowCount int -- Local @@ROWCOUNT

	
    SET transaction isolation level serializable
	BEGIN TRAN
	
		declare @Group_Notes_old varchar(max)

		select @Group_Notes_old = Group_Notes
	    from   [Group]
		where  Group_ID = @group_ID
		   
		UPDATE [Group] 
		SET	   Group_Name = @Group_Name,
			   Group_Account_Manager = @Group_Account_Manager,
			   Group_CIF = @Group_CIF,
			   Group_Status = @Group_Status,
               Group_Notes = @Group_Notes
		WHERE  Group_ID = @Group_ID

		if coalesce(@Group_Notes_old,'') != coalesce(@Group_Notes,'')
		begin
		  insert into Change_History
		  select @Group_ID,
		         0,
				 'Group',
				 @Group_ID,
				 'Group_Notes',
				 @Group_Notes_old,
				 @Group_Notes,
				 'String',
				 getdate(),
				 @user
		end
	
	UPDATE [Group] SET Group_Status = 'Proposed'  WHERE Group_ID = @GroupId;	   
	INSERT [Group_Status_History] VALUES (@GroupId, @Group_Status,'Group Updated',@User,getdate())

	SELECT @myERROR = @@ERROR, @myRowCount = @@ROWCOUNT
    IF @myERROR != 0 GOTO HANDLE_ERROR

	
	commit tran
	
	RETURN 0

HANDLE_ERROR:
    ROLLBACK TRAN
    RETURN @myERROR


------------------------------------------------------------------------




