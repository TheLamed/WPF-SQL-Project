CREATE PROCEDURE UpadeUser 
@ID INT,
@name TEXT, 
@surname TEXT, 
@login TEXT, 
@password TEXT, 
@email TEXT
AS
BEGIN

UPDATE Users
SET Name = @name, Surname = @surname, Login = @login, Password = @password, email = @email
WHERE ID = @ID

END