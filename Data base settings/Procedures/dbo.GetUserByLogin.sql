﻿CREATE PROCEDURE GetUserByLogin
@login TEXT
AS
BEGIN

SELECT * FROM Users WHERE Login LIKE @login

END