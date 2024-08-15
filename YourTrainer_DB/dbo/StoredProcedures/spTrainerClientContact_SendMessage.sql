CREATE PROCEDURE [dbo].[spTrainerClientContact_SendMessage]
	@SenderId INT, 
	@ReceiverId INT, 
	@MessageType NVARCHAR(500), 
	@MessageContent NVARCHAR(MAX)
AS
BEGIN

	INSERT INTO [dbo].[TrainerClientContact]
	(SenderId, ReceiverId, MessageType, MessageContent, IsRead, SendDateTime)
	VALUES
	(@SenderId, @ReceiverId, @MessageType, @MessageContent, 0, GETDATE())

END