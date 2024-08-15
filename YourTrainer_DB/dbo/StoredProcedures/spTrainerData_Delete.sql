CREATE PROCEDURE [dbo].[spTrainerData_Delete]
	@TrainerId INT
AS
BEGIN

	DELETE
	FROM dbo.TrainersData
	WHERE TrainerId = @TrainerId;

END
