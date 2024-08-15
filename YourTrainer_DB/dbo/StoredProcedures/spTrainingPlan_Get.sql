CREATE PROCEDURE [dbo].[spTrainingPlan_Get]
	@Id int
AS
BEGIN

	SELECT Id, Title, TrainingDays, Notes, Creator
	FROM dbo.TrainingPlans
	WHERE Id = @Id 

END