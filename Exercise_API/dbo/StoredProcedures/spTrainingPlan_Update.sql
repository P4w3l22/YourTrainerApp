CREATE PROCEDURE [dbo].[spTrainingPlan_Update]
	@Id INT,
	@Title NVARCHAR(200),
	@TrainingDays NVARCHAR(15),
	@Notes NVARCHAR(2000),
	@Creator NVARCHAR(200)
AS
BEGIN

	UPDATE dbo.TrainingPlans
	SET Title = @Title,
		TrainingDays = @TrainingDays,
		Notes = @Notes,
		Creator = @Creator
	WHERE Id = @Id;

END