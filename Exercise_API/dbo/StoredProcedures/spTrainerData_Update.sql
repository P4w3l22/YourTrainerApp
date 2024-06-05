CREATE PROCEDURE [dbo].[spTrainerData_Update]
	@TrainerId INT,
	@TrainerName NVARCHAR(500),
	@Description NVARCHAR(MAX),
	@Email NVARCHAR(500),
	@PhoneNumber NVARCHAR(40),
	@Rate DECIMAL,
	@Availability INT
AS
BEGIN

	UPDATE dbo.TrainersData
	SET TrainerName = @TrainerName,
		Description = @Description,
		Email = @Email,
		PhoneNumber = @PhoneNumber,
		Rate = @Rate,
		Availability = @Availability
	WHERE TrainerId = @TrainerId;

END
