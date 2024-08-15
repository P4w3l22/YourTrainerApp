CREATE PROCEDURE [dbo].[spMemberData_Insert]
	@MemberId INT,
	@MemberName NVARCHAR(500),
	@Description NVARCHAR(MAX),
	@Email NVARCHAR(500),
	@PhoneNumber NVARCHAR(40),
	@TrainersId NVARCHAR(500),
	@TrainersPlan NVARCHAR(500)
AS

BEGIN

	INSERT INTO dbo.MembersData
	(MemberId, MemberName, Description, Email, PhoneNumber, TrainersId, TrainersPlan) 
	VALUES
	(@MemberId, @MemberName, @Description, @Email, @PhoneNumber, @TrainersId, @TrainersPlan);

END