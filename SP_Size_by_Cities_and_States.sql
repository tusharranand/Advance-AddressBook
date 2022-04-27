use Addressbook
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE CitesAndStatesCount
AS
BEGIN
	SET NOCOUNT ON;
	select distinct City, State from Family
END
GO