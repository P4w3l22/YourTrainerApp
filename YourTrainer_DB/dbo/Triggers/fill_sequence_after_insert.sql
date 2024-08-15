CREATE TRIGGER fill_sequence_after_insert
	ON TrainingPlanExercises
	AFTER INSERT
	AS
	BEGIN

		DECLARE @inserted_id INT;
		DECLARE @inserted_TPId INT;

		SELECT @inserted_id = Id, @inserted_TPId = TPId FROM inserted;

		UPDATE TrainingPlanExercises
		SET Sequence = (SELECT COUNT(*) FROM TrainingPlanExercises WHERE TPId = @inserted_TPid)
		WHERE Id = @inserted_id;

	END;