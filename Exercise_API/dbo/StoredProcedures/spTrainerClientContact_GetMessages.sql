CREATE PROCEDURE [dbo].[spTrainerClientContact_GetMessages]
	@ReceiverId INT,
	@MessageType NVARCHAR(500)
AS
BEGIN

	SELECT Id, SenderId, ReceiverId, MessageType, MessageContent, IsRead, SendDateTime
	FROM [dbo].[TrainerClientContact]
	WHERE ReceiverId = @ReceiverId AND 
		  MessageType = @MessageType;

END