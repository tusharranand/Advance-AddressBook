SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE RemoveContact
@FirstName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	delete from Family where FirstName = @FirstName
END
GO
