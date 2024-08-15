CREATE PROCEDURE [dbo].[spTrainingPlanExercise_Update]
	@Id INT,
	@TPId INT,
	@EId INT,
	@Series INT,
	@Reps NVARCHAR(50),
	@Weights NVARCHAR(200)
AS
BEGIN

	UPDATE dbo.TrainingPlanExercises
	SET TPId = @TPId,
		EId = @EId,
		Series = @Series,
		Reps = @Reps,
		Weights = @Weights
	WHERE Id = @Id;

END