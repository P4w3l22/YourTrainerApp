CREATE PROCEDURE [dbo].[spExercise_Get]
	@Id int
AS
BEGIN

	SELECT *
	FROM dbo.Exercises
	WHERE Id = @Id;

END