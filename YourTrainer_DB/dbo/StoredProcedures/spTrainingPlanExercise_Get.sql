CREATE PROCEDURE [dbo].[spTrainingPlanExercise_Get]
	@Id int
AS
BEGIN

	SELECT *
	FROM dbo.TrainingPlanExercises
	WHERE Id = @Id;

END