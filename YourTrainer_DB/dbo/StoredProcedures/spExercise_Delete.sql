CREATE PROCEDURE [dbo].[spExercise_Delete]
	@Id INT
AS
BEGIN

	DELETE 
	FROM dbo.Exercises
	WHERE Id = @Id;

END
