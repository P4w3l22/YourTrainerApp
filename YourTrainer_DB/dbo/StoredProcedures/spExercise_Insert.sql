CREATE PROCEDURE [dbo].[spExercise_Insert]
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

	INSERT INTO dbo.Exercises (Name, Force, Level, Mechanic, Equipment, PrimaryMuscles, SecondaryMuscles, Instructions, Category, ImgPath1, ImgPath2)
	VALUES (@Name, @Force, @Level, @Mechanic, @Equipment, @PrimaryMuscles, @SecondaryMuscles, @Instructions, @Category, @ImgPath1, @ImgPath2);

END
