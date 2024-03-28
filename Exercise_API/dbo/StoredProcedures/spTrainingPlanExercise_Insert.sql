CREATE PROCEDURE [dbo].[spTrainingPlanExercise_Insert]
	@TPId INT,
	@EId INT,
	@Series INT,
	@Weights NVARCHAR(200)
AS
BEGIN

	INSERT INTO dbo.TrainingPlanExercises (TPId, EId, Series, Weights)
	VALUES (@TPId, @EId, @Series, @Weights);

END
