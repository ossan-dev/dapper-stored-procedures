USE [wwi]
GO

/****** Object:  StoredProcedure [usp].[get_sale_order_complex_paging]    Script Date: 13/10/2021 13:03:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [usp].[get_sale_order_complex_paging]
	-- Add the parameters for the stored procedure here
	@offset int = 0,
	@take int = 10,
	@orderBy VARCHAR(500) = 'OrderID ASC',
	@predicate VARCHAR(MAX) = 'WHERE 1 = 1',
	@customerId int = 1,
	@customerId_1 int = 1,	
	@customerId_2 int = 1,
	@contactPersonId int = 1,
	@orderDate datetime2 = '1900-01-01'
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @column_names AS VARCHAR(1000) = STUFF(
						(
							SELECT ',' + COLUMN_NAME
							FROM (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Orders') AS sb
							FOR XML PATH('')
						), 1, 0, '')

	------------------------------------------------- SORT ---------------------------------------------------------

	-- validation logic to prevent bad @orderBy param
	DECLARE @c_value VARCHAR(50)
	DECLARE @c_field VARCHAR(50)
	DECLARE @c_direction VARCHAR(50)

	DECLARE orderBy_cursor CURSOR FOR

	SELECT value
	FROM string_split(@orderBy, ',')

	OPEN orderBy_cursor
	FETCH NEXT FROM orderBy_cursor INTO @c_value

	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @c_field = SUBSTRING(@c_value, 0, CHARINDEX(' ', @c_value))
		SET @c_direction = SUBSTRING(@c_value, CHARINDEX(' ', @c_value) + 1, LEN(@c_value))

		IF UPPER(@c_direction) NOT IN ('ASC', 'DESC')
		BEGIN
			RAISERROR('Invalid parameter for @c_direction: %s', 11, 1, @c_direction);
			RETURN -1;
		END

		-- TODO: make dynamic this section
		-- reject any invalid fields
		IF CHARINDEX(@c_field, @column_names) < 0		
		BEGIN
			RAISERROR('Invalid parameter for @c_field: %s', 11, 1, @c_field);
			RETURN -1;
		END

		FETCH NEXT FROM orderBy_cursor INTO @c_value

	END

	CLOSE orderBy_cursor
	DEALLOCATE orderBy_cursor

	------------------------------------------------- FILTER ---------------------------------------------------------
	DECLARE @tmp_predicate AS VARCHAR(MAX) = RTRIM(LTRIM(REPLACE(@predicate, 'WHERE', '')))
	
	-- https://stackoverflow.com/a/44626125/14394371
	DECLARE @filterField AS VARCHAR(50) = parsename(replace(@tmp_predicate, ' ', '.'), 3)
	DECLARE @filterOperator AS VARCHAR(50) = parsename(replace(@tmp_predicate, ' ', '.'), 2)

	-- TODO: make dynamic this section
	-- reject any invalid fields
	IF CHARINDEX(@filterField, @column_names) < 0
	BEGIN
		RAISERROR('Invalid parameter for @predicate: %s', 11, 1, @predicate);
		RETURN -1;
	END

	-- reject any invalid relational operators
	IF @filterOperator NOT IN ('=', '<>', '>=', '>', '<=', '<', '!=', 'LIKE')
	BEGIN
		RAISERROR('Invalid parameter for @predicate: %s', 11, 1, @predicate);
		RETURN -1;
	END	

	DECLARE @cmd NVARCHAR(MAX) = ''
	DECLARE @params NVARCHAR(MAX) = '@Offset int, @Take int, @CustomerId int, @ContactPersonId int, @OrderDate datetime2, @CustomerId_1 int, @CustomerId_2 int'


	SET @cmd = 'SELECT OrderID, CustomerID, OrderDate, ExpectedDeliveryDate, ContactPersonID
				FROM Sales.Orders ' +
				@predicate +
				' ORDER BY ' + @OrderBy +
				' OFFSET @Offset ROWS
				FETCH NEXT @Take ROWS ONLY;'

	EXEC sp_ExecuteSql @cmd, @params, @Offset = @offset, @Take = @take, @CustomerId = @customerId, @ContactPersonId = @contactPersonId, @OrderDate = @orderDate, @CustomerId_1 = @customerId_1, @CustomerId_2 = @customerId_2;

END
GO


