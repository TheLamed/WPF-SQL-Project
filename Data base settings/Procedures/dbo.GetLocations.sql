CREATE PROCEDURE GetLocations
@part TEXT
AS
BEGIN

SELECT * FROM Location 
WHERE Country LIKE CONCAT('%', @part, '%')
OR City LIKE CONCAT('%', @part, '%')

END