CREATE PROCEDURE [dbo].[spAssignedTrainingPlans_GetAll]
	@TrainerId INT
AS
BEGIN
	
	SELECT *
	FROM dbo.AssignedTrainingPlans
	WHERE TrainerId = @TrainerId;

END