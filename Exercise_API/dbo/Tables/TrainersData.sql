CREATE TABLE [dbo].[TrainersData]
(
	[TrainerId] INT PRIMARY KEY NOT NULL,
	[TrainerName] NVARCHAR(500) NOT NULL,
	[Description] NVARCHAR(MAX) NOT NULL,
	[Email] NVARCHAR(500) NOT NULL,
	[PhoneNumber] NVARCHAR(40) NOT NULL,
	[Rate] DECIMAL NOT NULL,
	[Availability] INT NOT NULL,

	CONSTRAINT [FK_TrainerId] FOREIGN KEY ([TrainerId]) REFERENCES [LocalUsers]([Id])
)