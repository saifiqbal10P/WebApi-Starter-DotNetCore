Starter Template for .Net Core Web Api
--------------------------------------
This project contains recipecore.net that will provide CRUD operations for EntitFrameWork. Repository pattern has been implemented on this template.

Functionality Implemented 
--------------------
1- Swagger
2- Authentication, Authorization with .net core identity
3- Authentication mechanism is jwt bearer token with refresh token implemented.
4- Nlog is used for logging
5- Some action filter has been implemented to handle global exception and model validation
6- Entityframework code approach has been used in this template with intitial seed function configured
7- Automapper

How to Start
------------
1- Test Controller has been available for quick start guidance. Please have a look.
2- A controller will be represting an entity for e.g users. To fetch list of users from AspNetUsers table. There must be a usercontroller with userservice through constructor injection. Please see DependecyInjection.cs how services and repository are inejected. Any new service and repository that is created will be required to register to the DependecyInjection.cs file.
3- On how to create service and repository for any entity. Please have a look at TestTableService and TestTableRepository. 
4- Working Authentication is available. It can be tested from swagger. with UserName : superAdmin ,pwd: B00km@rk
5- TestController can be tested with authorization by fist calling login . Capture token , then passing in Authorization header through postman or from swagger. 
6- Logging sample on how to create Logs is availabel on Testcontroller-> GetAllTestResults(). Log file is available on project bin folder.
7- For Every api response DataTransferObject class has been used that will return paginginfo ,filter, sorting etc..
8- For Crud operations please see AuthService.cs


How to add migrations and update database
-----------------------------------------
open package manager console then write 'add-migration 'migrationName''. see other commands at https://www.entityframeworktutorial.net/efcore/entity-framework-core-migration.aspx

If above one doesn't work then open cli. Navigate to .Core project. Write "dotnet ef migrations add 'migrationname' -s ../Template". -S refers to startup project. For further command, use above mentioned link.


TO DO
------
Unit Tests