CREATE PROCEDURE [dbo].[spAssingedTrainingPlans_Insert]
	@TrainerId INT,
	@ClientId INT,
	@PlanId INT
AS
BEGIN

	INSERT INTO dbo.AssignedTrainingPlans
	(TrainerId, ClientId, PlanId) VALUES
	(@TrainerId, @ClientId, @PlanId);

END
