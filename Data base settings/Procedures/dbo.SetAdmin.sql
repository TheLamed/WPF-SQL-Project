﻿CREATE PROCEDURE SetAdmin
@isAdmin INT,
@ID INT
AS
BEGIN

IF @isAdmin = 1
	INSERT INTO Admins (ID) VALUES (@ID)
ELSE
	DELETE FROM Admins WHERE ID = @ID

END