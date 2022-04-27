use Addressbook
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE ContactsByCityOrState
@City_State_Name varchar(50),
@City_or_State int
AS
BEGIN
	SET NOCOUNT ON;
	if (@City_or_State=0)
		begin
			select FirstName from Family where City = @City_State_Name
		end
	else if (@City_or_State=1)
		begin
			select FirstName from Family where State = @City_State_Name
		end
END
GO
