# dapper-stored-procedures
Show off Dapper capabilitites by using stored procedures to get paginated data. 

## data source
The demo db was found at [here](https://github.com/microsoft/sql-server-samples/releases/download/wide-world-importers-v1.0/WideWorldImporters-Standard.bacpac).  
First thing, you've to go in the installation folder of SQL Server and locate the SqlPackage CLI utility (my path is: "C:\Program Files\Microsoft SQL Server\150\DAC\bin").  
The script to run is:  
`.\SqlPackage.exe /Action:Import /SourceFile:"C:\Users\ivan.pesenti\Downloads\WideWorldImporters-Standard.bacpac" /TargetConnectionString:"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=wwi;Integrated Security=true;"`  

## branches
1. PurchaseOrdersController: implementation with [Dapper.SimpleCRUD](https://dapper-tutorial.net/dapper-simplecrud)
1. SaleOrdersController: implementation with stored procedures