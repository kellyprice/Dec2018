USE [CAS]
GO
/****** Object:  StoredProcedure [dbo].[sp_Update_Group_Status_To_DELETE]    Script Date: 31/10/2019 14:35:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Update_Group_Status_To_DELETE] 
       
       ( @id INT
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
       
              UPDATE 
					[Group] 
              SET           
					Group_Status = 'Proposed Deletion'
              WHERE 
					Group_ID = @id

			INSERT [Group_Status_History] VALUES (@id,'Proposed Deletion','Group Updated',@User,getdate())
	
       
       
       SELECT @myERROR = @@ERROR, @myRowCount = @@ROWCOUNT
    IF @myERROR != 0 GOTO HANDLE_ERROR
 
       
       commit tran
       
       RETURN 0
 
HANDLE_ERROR:
    ROLLBACK TRAN
    RETURN @myERROR




