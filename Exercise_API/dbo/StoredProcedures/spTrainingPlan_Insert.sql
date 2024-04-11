CREATE PROCEDURE [dbo].[spTrainingPlan_Insert]
	@Title NVARCHAR(200),
	@TrainingDays NVARCHAR(15),
	@Notes NVARCHAR(2000),
	@Creator NVARCHAR(200)
AS
BEGIN

	INSERT INTO dbo.TrainingPlans (Title, TrainingDays, Notes, Creator)
	VALUES (@Title, @TrainingDays, @Notes, @Creator);

END
