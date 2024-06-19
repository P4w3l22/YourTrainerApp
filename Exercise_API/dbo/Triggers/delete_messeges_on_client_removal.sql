--CREATE TRIGGER delete_messeges_on_trainer_removal
--	ON MembersData
--	AFTER UPDATE
--	AS
--	BEGIN

--		IF UPDATE(TrainersId)
--		BEGIN

--			DECLARE @ClientId INT;
--			DECLARE @TrainerId INT;
			
--			SELECT @ClientId = MemberId FROM INSERTED;
--			SELECT @TrainerId = TrainersId FROM DELETED;

--			--SELECT @ClientId = i.MemberId,
--			--	   @TrainerId = d.TrainersId
--			--FROM INSERTED i
--			--INNER JOIN DELETED d ON i.Id = d.Id
--			--INNER JOIN MembersData m ON m.Id = d.Id

--			DELETE FROM [dbo].[TrainerClientContact]
--			WHERE SenderId IN (@TrainerId, @CLientId) AND 
--					ReceiverId IN (@TrainerId, @CLientId);

--		END;

--	END;