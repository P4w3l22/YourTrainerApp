CREATE PROCEDURE [dbo].[spTrainerData_Insert]
	@TrainerId INT,
	@TrainerName NVARCHAR(500),
	@Description NVARCHAR(MAX),
	@Email NVARCHAR(500),
	@PhoneNumber NVARCHAR(40),
	@Rate DECIMAL,
	@Availability INT
AS

BEGIN

	INSERT INTO dbo.TrainersData
	(TrainerId, TrainerName, Description, Email, PhoneNumber, Rate, Availability) 
	VALUES
	(@TrainerId, @TrainerName, @Description, @Email, @PhoneNumber, @Rate, @Availability);

END