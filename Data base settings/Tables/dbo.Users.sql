CREATE TABLE [dbo].[Users] (
    [ID]       INT  IDENTITY (0, 1) NOT NULL,
    [Name]     TEXT NULL,
    [Surname]  TEXT NULL,
    [Login]    TEXT NOT NULL,
    [Password] TEXT NOT NULL,
    [email]    TEXT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

