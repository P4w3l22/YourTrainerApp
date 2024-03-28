CREATE PROCEDURE [dbo].[spTrainingPlanExercise_Update]
	@Id INT,
	@TPId INT,
	@EId INT,
	@Series INT,
	@Weights NVARCHAR(200)
AS
BEGIN

	UPDATE dbo.TrainingPlanExercises
	SET TPId = @TPId,
		EId = @EId,
		Series = @Series,
		Weights = @Weights
	WHERE Id = @Id;

END