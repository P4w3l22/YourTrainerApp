CREATE TABLE [dbo].[AssignedTrainingPlans]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[TrainerId] INT NOT NULL,
	[ClientId] INT NOT NULL,
	[PlanId] INT NOT NULL,

	CONSTRAINT [FK_ATP_TrainerId] FOREIGN KEY ([TrainerId]) REFERENCES [TrainersData]([TrainerId]),
	CONSTRAINT [FK_ATP_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [MembersData]([MemberId]),
	CONSTRAINT [FK_ATP_TrainingPlanId] FOREIGN KEY ([PlanId]) REFERENCES [TrainingPlans]([Id])
)
