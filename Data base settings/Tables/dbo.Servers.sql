CREATE TABLE [dbo].[Servers] (
    [ID]         INT  IDENTITY (0, 1) NOT NULL,
    [Processor]  TEXT NULL,
    [RAM]        INT  NULL,
    [SSD]        INT  NULL,
    [LocationID] INT  NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

