CREATE PROCEDURE [dbo].[spTrainerData_Insert]
	@TrainerId INT,
	@Description NVARCHAR(MAX),
	@Email NVARCHAR(500),
	@PhoneNumber NVARCHAR(40),
	@Availability INT
AS

BEGIN

	INSERT INTO dbo.TrainersData
	(TrainerId, Description, Email, PhoneNumber, Availability) 
	VALUES
	(@TrainerId, @Description, @Email, @PhoneNumber, @Availability);

END