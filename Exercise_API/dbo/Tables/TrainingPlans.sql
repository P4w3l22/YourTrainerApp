﻿CREATE TABLE [dbo].[TrainingPlans]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[Title] NVARCHAR(200) NOT NULL,
	[TrainingDays] NVARCHAR(80) NOT NULL,
	[Notes] NVARCHAR(2000),
	[Creator] NVARCHAR(200)
)
