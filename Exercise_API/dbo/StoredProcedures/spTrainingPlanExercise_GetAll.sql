CREATE PROCEDURE [dbo].[spTrainingPlanExercises_GetAll]
	@TPId INT
AS
BEGIN

	SELECT *
	FROM dbo.TrainingPlanExercises
	WHERE TPId = @TPId;

END