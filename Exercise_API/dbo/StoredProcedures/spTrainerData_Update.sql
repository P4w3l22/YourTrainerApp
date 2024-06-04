CREATE PROCEDURE [dbo].[spTrainerData_Update]
	@TrainerId INT,
	@Description NVARCHAR(MAX),
	@Email NVARCHAR(500),
	@PhoneNumber NVARCHAR(40),
	@Availability INT
AS
BEGIN

	UPDATE dbo.TrainersData
	SET Description = @Description,
		Email = @Email,
		PhoneNumber = @PhoneNumber,
		Availability = @Availability
	WHERE TrainerId = @TrainerId;

END
