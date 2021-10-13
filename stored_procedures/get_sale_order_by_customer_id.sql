USE [wwi]
GO

/****** Object:  StoredProcedure [usp].[get_sale_order_by_customer_id]    Script Date: 13/10/2021 12:59:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [usp].[get_sale_order_by_customer_id]
	-- Add the parameters for the stored procedure here
	@customerId int = 1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT OrderID, CustomerID, OrderDate, ExpectedDeliveryDate, ContactPersonID
	FROM Sales.Orders
	WHERE CustomerID = @customerId
END
GO

