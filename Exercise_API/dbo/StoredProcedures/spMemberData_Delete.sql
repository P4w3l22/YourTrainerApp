CREATE PROCEDURE [dbo].[spMemberData_Delete]
	@MemberId INT
AS
BEGIN

	DELETE
	FROM dbo.MembersData
	WHERE MemberId = @MemberId;

END
