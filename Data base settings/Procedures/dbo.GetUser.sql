CREATE PROCEDURE GetUser
@login TEXT,
@password TEXT
AS
BEGIN

SELECT TOP 1 * FROM Users WHERE Login LIKE @login AND Password LIKE @password

END