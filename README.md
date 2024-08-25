This general REST API implements CRUD endpoints for 3 models that are stored in a SQL Server database.
The models are User, UserSalary and UserJobInfo. The client has the ability to interact with the API to perform a wide range of tasks, with the underlying tasks being executed with dapper using SQL statements to query the database or by using Entity Framework.
Authentication is implemented using JWT token with HmacSha512 security algorithm. The application will be deployed to the cloud using Microsoft Azure.

