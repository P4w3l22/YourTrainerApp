CREATE PROCEDURE [dbo].[spTrainerClientContact_SetAsRead]
	@Id INT
AS
BEGIN

	UPDATE [dbo].[spTrainerClientContact_SetAsRead]
	SET IsRead = 1 
	WHERE Id = @Id;

END
