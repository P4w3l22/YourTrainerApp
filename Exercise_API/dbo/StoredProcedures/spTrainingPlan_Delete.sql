CREATE PROCEDURE [dbo].[spTrainingPlan_Delete]
	@Id INT
AS
BEGIN

	DELETE 
	FROM dbo.TrainingPlans
	WHERE Id = @Id;

END
