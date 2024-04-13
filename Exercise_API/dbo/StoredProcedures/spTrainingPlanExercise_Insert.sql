CREATE PROCEDURE [dbo].[spTrainingPlanExercise_Insert]
	@TPId INT,
	@EId INT,
	@Series INT,
	@Reps NVARCHAR(50),
	@Weights NVARCHAR(200)
AS
BEGIN

	INSERT INTO dbo.TrainingPlanExercises (TPId, EId, Series, Reps, Weights)
	VALUES (@TPId, @EId, @Series, @Reps, @Weights);

END
