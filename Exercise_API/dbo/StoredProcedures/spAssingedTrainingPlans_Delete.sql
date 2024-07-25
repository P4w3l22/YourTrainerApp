CREATE PROCEDURE [dbo].[spAssingedTrainingPlans_Delete]
	@PlanId INT
AS
BEGIN

	DELETE 
	FROM dbo.AssignedTrainingPlans
	WHERE PlanId = @PlanId;

END
