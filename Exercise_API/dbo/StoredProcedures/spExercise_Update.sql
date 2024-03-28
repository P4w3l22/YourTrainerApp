CREATE PROCEDURE [dbo].[spExercise_Update]
	@Id INT,
	@Name NVARCHAR(100),
	@Force NVARCHAR(50),
	@Level NVARCHAR(50),
	@Mechanic NVARCHAR(30),
	@Equipment NVARCHAR(70),
	@PrimaryMuscles NVARCHAR(150),
	@SecondaryMuscles NVARCHAR(300),
	@Instructions NVARCHAR(MAX),
	@Category NVARCHAR(50),
	@ImgPath1 NVARCHAR(200),
	@ImgPath2 NVARCHAR(200)
AS
BEGIN

	UPDATE dbo.Exercises
	SET Name = @Name,
		Force = @Force,
		Level = @Level,
		Mechanic = @Mechanic,
		Equipment = @Equipment,
		PrimaryMuscles = @PrimaryMuscles,
		SecondaryMuscles = @SecondaryMuscles,
		Instructions = @Instructions,
		Category = @Category,
		ImgPath1 = @ImgPath1,
		ImgPath2 = @ImgPath2
	WHERE Id = @Id;

END