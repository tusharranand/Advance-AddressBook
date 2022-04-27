use Addressbook
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE DisplayAllDetails
	
AS
BEGIN
	SET NOCOUNT ON;
	select FirstName, LastName, Address, City, State, ZipCode, PhoneNumber, Email from Family
END
GO
