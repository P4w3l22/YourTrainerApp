CREATE TABLE [dbo].[TrainerClientContact]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[SenderId] INT NOT NULL,
	[ReceiverId] INT NOT NULL,
	[MessageType] NVARCHAR(500) NOT NULL,
	[MessageContent] NVARCHAR(MAX) NOT NULL,
	[IsRead] BIT NOT NULL,
	[SendDateTime] DATETIME NOT NULL,

	CONSTRAINT [FK_Contact_SenderId] FOREIGN KEY ([SenderId]) REFERENCES [LocalUsers]([Id]),
	CONSTRAINT [FK_Contact_ReceiverId] FOREIGN KEY ([ReceiverId]) REFERENCES [LocalUsers]([Id])

)
