CREATE PROCEDURE AddServer
@proc TEXT,
@RAM INT,
@SSD INT,
@Location INT
AS
BEGIN

INSERT INTO Servers (Processor, RAM, SSD, LocationID)
VALUES (@proc, @RAM, @SSD, @Location)

END