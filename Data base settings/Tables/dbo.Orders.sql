CREATE TABLE [dbo].[Orders] (
    [ID]         INT  IDENTITY (0, 1) NOT NULL,
    [OwnerID]    INT  NOT NULL,
    [ServerID]   INT  NOT NULL,
    [StartDate]  DATE NOT NULL,
    [FinishDate] DATE NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED ([ID] ASC)
);

