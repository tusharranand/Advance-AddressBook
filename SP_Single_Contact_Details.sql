use Addressbook
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE or ALTER PROCEDURE AccessDetailsForFirstName 
	@FirstName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	select FirstName, LastName, Address, City, State, ZipCode, PhoneNumber, Email from Family where FirstName = @FirstName
END
GO
