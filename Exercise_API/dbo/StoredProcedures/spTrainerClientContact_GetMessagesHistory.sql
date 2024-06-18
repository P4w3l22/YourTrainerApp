CREATE PROCEDURE [dbo].[spTrainerClientContact_GetMessagesHistory]
	@SenderId INT
AS
BEGIN

	SELECT Id, SenderId, ReceiverId, MessageType, MessageContent, IsRead, SendDateTime
	FROM [dbo].[TrainerClientContact]
	WHERE SenderId = @SenderId;

END
