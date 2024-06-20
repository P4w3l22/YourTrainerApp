CREATE PROCEDURE [dbo].[spTrainerClientContact_GetCooperationProposals]
	@ReceiverId INT,
	@MessageType NVARCHAR(500)
AS
BEGIN
	
	SELECT *
	FROM dbo.TrainerClientContact
	WHERE ReceiverId = @ReceiverId AND
		  MessageType = @MessageType AND
		  IsRead = 0;

END
