CREATE PROCEDURE AddLocation
@country TEXT, 
@city TEXT, 
@image IMAGE
AS
BEGIN

INSERT INTO Location (Country, City, Photo)
VALUES (@country, @city, @image)

END