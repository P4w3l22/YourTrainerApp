CREATE PROCEDURE [dbo].[spMemberData_Get]
	@MemberId INT
AS
BEGIN

	SELECT *
	FROM dbo.MembersData
	WHERE MemberId = @MemberId;

END