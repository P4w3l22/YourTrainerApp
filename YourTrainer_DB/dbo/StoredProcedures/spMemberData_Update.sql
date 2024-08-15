CREATE PROCEDURE [dbo].[spMemberData_Update]
	@MemberId INT,
	@MemberName NVARCHAR(500),
	@Description NVARCHAR(MAX),
	@Email NVARCHAR(500),
	@PhoneNumber NVARCHAR(40),
	@TrainersId NVARCHAR(500),
	@TrainersPlan NVARCHAR(500)
AS
BEGIN

	UPDATE dbo.MembersData
	SET MemberName = @MemberName,
		Description = @Description,
		Email = @Email,
		PhoneNumber = @PhoneNumber,
		TrainersId = @TrainersId,
		TrainersPlan = @TrainersPlan
	WHERE MemberId = @MemberId;

END
