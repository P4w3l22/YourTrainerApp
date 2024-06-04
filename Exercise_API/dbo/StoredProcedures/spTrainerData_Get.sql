CREATE PROCEDURE [dbo].[spTrainerData_Get]
	@TrainerId INT
AS
BEGIN

	SELECT *
	FROM dbo.TrainersData
	WHERE TrainerId = @TrainerId;

END