CREATE PROCEDURE RegisterUser
@name TEXT,
@surname TEXT,
@login TEXT,
@password TEXT,
@email TEXT,
@isAdmin INT
AS
BEGIN

INSERT INTO Users (Login, Password, Name, Surname, email)
VALUES (@login, @password, @name, @surname, @email)

DECLARE @ID INT
SELECT @ID = ID FROM Users WHERE Login LIKE @login AND Password LIKE @password

EXEC SetAdmin @isAdmin, @ID

END