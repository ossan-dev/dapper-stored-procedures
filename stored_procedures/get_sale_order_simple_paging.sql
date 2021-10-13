USE [wwi]
GO

/****** Object:  StoredProcedure [usp].[get_sale_order_simple_paging]    Script Date: 13/10/2021 13:03:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [usp].[get_sale_order_simple_paging]
	-- Add the parameters for the stored procedure here
	@offset int = 1,
	@take int = 10
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT OrderID, CustomerID, OrderDate, ExpectedDeliveryDate, ContactPersonID
	FROM Sales.Orders
	ORDER BY OrderID
	OFFSET @offset ROWS
	FETCH NEXT @take ROWS ONLY;

END
GO


