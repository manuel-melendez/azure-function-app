Project Description: 
This project contains an azure function app. The function app contains 5 Http-triggered functions, and communicates with an Azure table.

Get - retrieves all entities from the tables
GetById - retrieves a single entity specified by partition key and rowkey
Create - creates an entity {"Subject":"{add subject}", "Body":"{add body}", "BlobUrl":"{add bloburl", "Type:"{Success or fail}"}
Edit - edits an entity retrieved by partitionkey and rowkey {"Subject":"{add subject}", "Body":"{add body}", "BlobUrl":"{add bloburl", "Type:"{Success or fail}"}
Delete - deletes an entity specified by partitionkey and rowkey

install Azure.Data.Table nuget package
may need to install Newtonsoft.json nuget package 
refer to online documentation for instructions

Configure secrets and configuration for azure storage account - create local settings folder
