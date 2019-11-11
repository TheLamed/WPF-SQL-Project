CREATE PROCEDURE UpdateServer
@ID INT,
@proc TEXT,
@RAM INT,
@SSD INT,
@Location INT
AS
BEGIN

UPDATE Servers
SET Processor = @proc, RAM = @RAM, SSD = @SSD, LocationID = @Location
WHERE ID = @ID

END