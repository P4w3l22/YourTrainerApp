CREATE PROCEDURE [dbo].[spAssignedTrainingPlans_Get]
	@ClientId INT
AS
BEGIN

	SELECT *
	FROM dbo.AssignedTrainingPlans
	WHERE ClientId = @ClientId;

END