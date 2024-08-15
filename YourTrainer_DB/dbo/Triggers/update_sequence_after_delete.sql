CREATE TRIGGER update_sequence_after_delete
	ON TrainingPlanExercises
	AFTER DELETE
	AS
	BEGIN

		DECLARE @sequence_value INT;
		SELECT @sequence_value = Sequence FROM deleted;

		UPDATE TrainingPlanExercises
		SET Sequence = Sequence - 1
		WHERE Sequence > @sequence_value;

	END;
