CREATE PROCEDURE [dbo].[spLocalUsers_Insert]
	@UserName NVARCHAR(256),
	@Name NVARCHAR(256),
	@Password NVARCHAR(500),
	@Role NVARCHAR(40)
AS
BEGIN

	INSERT INTO dbo.LocalUsers (UserName, Name, Password, Role)
	VALUES (@UserName, @Name, @Password, @Role);

END
