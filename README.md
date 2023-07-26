# INFINITE.CORE
.NET 7 Web API & MVC Project Starter Template

## Installation of .NET 7 SDK & RUNTIMES
1. Download .NET 7 SDK and runtimes from the following link:
   ```
   https://dotnet.microsoft.com/en-us/download/dotnet/7.0
   ```

## Migration
1. Run "Database.sql" on your SQL Server to create the necessary database and tables.

## Running the Project
1. Open the "INFINITE.CORE.sln" solution file in Visual Studio 2022.
2. To run the Web API, select "INFINITE.CORE.API" and choose to run with or without debugging.
3. To run the MVC project, select "INFINITE.CORE.MVC" and choose to run with or without debugging.

## Backend Code Generator
### 1. Tool Installation
Install the following tool using Command Prompt or Terminal:
```shell
dotnet tool install --global dotnet-ef 
or
dotnet tool update --global dotnet-ef
```

### 2. Scaffolding 
Run the following command using Command Prompt or Terminal to generate code from the database schema:
```shell
dotnet ef dbcontext scaffold "Server=[host];Database=[database];user id =[user];password=[password];MultipleActiveResultSets=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer --output-dir "..\INFINITE.CORE.Data\Model" -c ApplicationDBContext --context-dir "..\INFINITE.CORE.Data" --namespace "INFINITE.CORE.Data.Model" --context-namespace "INFINITE.CORE.Data" --no-pluralize -f --no-onconfiguring --schema "dbo"
```
Replace the placeholders in [] with your database credentials. For example, [host] should be replaced with your server address.

### 3. Generated Files
The generated code can be found in the "INFINITE.CORE.DATA/Generated/Backend" folder.

## Service Proxy Code Generator
After creating a new endpoint in the "INFINITE.CORE.API" project, run the project to generate the service proxy code. The generated code can be found in "INFINITE.CORE.API/Proxy/Generated/".

To use the updated proxy, replace the old proxy file located in "INFINITE.CORE.MVC/wwwroot/js/".


## Roadmap
- Include an explanation of the project structure in Readme.md.
- Update the default data type for the Key Property.
- Improve Code Quality.
- Implement a Frontend Code Generator.
- Provide support for Microservices.


## Authors
- [@yogichandras](https://github.com/yogichandras/)
- [@papicoding on Youtube](https://www.youtube.com/channel/UCVxy3e7hZ4QAIRIABVX4Aog)
