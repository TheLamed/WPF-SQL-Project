CREATE PROCEDURE RemoveUserFromOrder
@order INT,
@user INT
AS
BEGIN

DELETE FROM UO WHERE UserID = @user AND OrderID = @order

END