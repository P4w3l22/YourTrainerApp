CREATE TABLE [dbo].[TrainersData]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[TrainerId] INT NOT NULL,
	[Description] NVARCHAR(MAX) NOT NULL,
	[Email] NVARCHAR(500) NOT NULL,
	[PhoneNumber] NVARCHAR(40) NOT NULL,
	[Availability] INT NOT NULL,

	CONSTRAINT [FK_TrainerId] FOREIGN KEY ([TrainerId]) REFERENCES [LocalUsers]([Id])
)