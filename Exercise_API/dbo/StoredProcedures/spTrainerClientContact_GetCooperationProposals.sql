CREATE PROCEDURE [dbo].[spTrainerClientContact_GetCooperationProposals]
	@ReceiverId INT
AS
BEGIN
	
	SELECT *
	FROM dbo.TrainerClientContact
	WHERE ReceiverId = @ReceiverId AND
		  MessageType = 'ConfirmClient' AND
		  IsRead = 0;

END
