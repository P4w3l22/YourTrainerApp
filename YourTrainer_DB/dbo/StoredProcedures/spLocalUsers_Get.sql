CREATE PROCEDURE [dbo].[spLocalUsers_Get]
	@UserName NVARCHAR(256)
AS
BEGIN

	SELECT Id, UserName, Name, Password, Role
	FROM dbo.LocalUsers
	WHERE UserName = @UserName;

END
