﻿CREATE PROCEDURE [dbo].[spTrainingPlan_Get]
	@Id int
AS
BEGIN

	SELECT Title, TrainingDays, Notes
	FROM dbo.TrainingPlans
	WHERE Id = @Id 

END