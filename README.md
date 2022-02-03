# customer-ops
A PostgreSQL database implementation, using ADO.NET compatible .NET provider NpgSQL. The database being used manipulates "Customer" data. 

## Features
- This project implements CRUD operations by simply using NpgSQL commands.
## Output
Opening a connection to the database  
Finished dropping table (if existed)  
Finished creating table  
Successfully added the customer  
Successfully added the customer  
Successfully added the customer  
1, Emre, Kılıç, 123456789  
2, George, Piker, 123785789  
3, Ash, Livington, 785456789  
Successfully updated the customer  
2, George, Piker, 123785789  
3, Ash, Livington, 785456789  
1, Emre, Quelich, 48954846546  
Successfully removed the customer  
3, Ash, Livington, 785456789  
1, Emre, Quelich, 48954846546  

## Learning Outcomes
- How to connect local or remote PostgreSQL to Dotnet environment.
- How to use NPGSQL API to manage the async commands.
