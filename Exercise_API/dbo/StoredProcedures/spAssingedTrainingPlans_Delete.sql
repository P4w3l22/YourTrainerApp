CREATE PROCEDURE [dbo].[spAssingedTrainingPlans_Delete]
	@Id INT
AS
BEGIN

	DELETE 
	FROM dbo.AssignedTrainingPlans
	WHERE Id = @Id;

END
