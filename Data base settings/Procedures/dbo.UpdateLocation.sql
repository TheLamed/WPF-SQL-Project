CREATE PROCEDURE UpdateLocation
@ID INT,
@country TEXT, 
@city TEXT, 
@image IMAGE
AS
BEGIN

UPDATE Location 
SET Country = @country, City = @city, Photo = @image
WHERE ID = @ID

END