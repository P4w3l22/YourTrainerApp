CREATE PROCEDURE [dbo].[spTrainingPlanExercise_Delete]
	@Id INT
AS
BEGIN

	DELETE 
	FROM dbo.TrainingPlanExercises
	WHERE Id = @Id;

END
