CREATE TABLE [dbo].[Location] (
    [ID]      INT   IDENTITY (0, 1) NOT NULL,
    [Country] TEXT  NULL,
    [City]    TEXT  NULL,
    [Photo]   IMAGE NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

