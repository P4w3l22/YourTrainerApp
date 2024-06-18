CREATE PROCEDURE [dbo].[spTrainerClientContact_GetMessages]
	@SenderId INT,
	@ReceiverId INT,
	@MessageType NVARCHAR(500)
AS
BEGIN

	SELECT Id, SenderId, ReceiverId, MessageType, MessageContent, IsRead, SendDateTime
	FROM [dbo].[TrainerClientContact]
	WHERE SenderId = @SenderId AND
		  ReceiverId = @ReceiverId AND 
		  MessageType = @MessageType;

END