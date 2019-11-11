CREATE PROCEDURE AddOrder 
@owner INT,
@server INT,
@start DATE,
@finish DATE
AS
BEGIN

INSERT INTO Orders (OwnerID, ServerID, StartDate, FinishDate)
VALUES (@owner, @server, @start, @finish)

DECLARE @orderID INT
SELECT @orderID = ID FROM Orders
WHERE OwnerID = @owner AND ServerID = @server AND StartDate = @start AND FinishDate = @finish

INSERT INTO UO (OrderID, UserID)
VALUES (@orderID, @owner)

END