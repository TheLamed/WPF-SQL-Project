CREATE PROCEDURE AddUserToOrder
@order INT,
@user INT
AS
BEGIN

INSERT INTO UO (UserID, OrderID)
VALUES (@user, @order)

END