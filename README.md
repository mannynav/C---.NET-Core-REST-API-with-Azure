This general REST API implements CRUD endpoints for 5 models that are stored in a SQL Server database. Database management was done using Azure Data Studio and the application will be deployed to Azure cloud.

### Models
The models are Post, UserComplete, User, UserSalary and UserJobInfo. The client has the ability to interact with the API to perform a wide range of tasks, with the underlying tasks being executed with Dapper, using SQL statements to query the database or by using Entity Framework. Another option is to use the UserComplete model, which consolidates User, UserSalary and UserJobInfo into a single model, this models controller uses SQL stored procedures and Dapper's dynamic parameters to simplify the SQL queries compared to the original User, UserSalary and UserJobInfo. With the use of stored procedures, the total number of endpoints for the User controller was reduced. The User, UserSalary and UserJobInfo are redundent but are left in for testing purposes.

### Authentication and Authorization
Authentication is implemented using JWT token with HmacSha512 security algorithm. Clients trying to use the UserComplete controller will need to be authorized to do. However, no authorization is needed to use them.

### Deployment
The API and SQL database are deployed to Azure.
