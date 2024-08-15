CREATE TABLE [dbo].[MembersData]
(
	[MemberId] INT PRIMARY KEY NOT NULL,
	[MemberName] NVARCHAR(500) NOT NULL,
	[Description] NVARCHAR(MAX) NOT NULL,
	[Email] NVARCHAR(500) NOT NULL,
	[PhoneNumber] NVARCHAR(40) NOT NULL,
	[TrainersId] NVARCHAR(500) NOT NULL,
	[TrainersPlan] NVARCHAR(500) NOT NULL,

	CONSTRAINT [FK_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [LocalUsers]([Id])
)