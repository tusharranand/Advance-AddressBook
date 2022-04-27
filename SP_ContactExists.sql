use Addressbook
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE or ALTER PROCEDURE ContactExists 
	@FirstName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	declare @result bit = 0
	if exists (SELECT * FROM Family where FirstName = @FirstName)
    begin
      set @result = 1;
    end
	return @result;
END
GO
