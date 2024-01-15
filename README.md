# aspnetcore-dapper

    - Dapper
	    - .NET Micro ORM
	    - ORM = Object Relational Mapper - Responsible for mapping between DB and C# objects
	
	- Full-blown ORM(EF) vs Micro-ORM(Dapper)
	- ORM
		- Maps DB and C# objects
		- Generates SQL based on C# objects
		- Change Tracking
		
	- Micro-ORM
		- Just maps between DB and C# objects
		- We have to supply SQL
		- No support of change-tracking
		
	- Features of Micro-ORM
		- Lightweight
		- Fast
		- Simple and Easy to Use
			- Less methods to know
			
	- Key-features of Dapper
		- Query and Map
			- Does this through parameterized approach. Avoids SQL Injection
		- Performance
			- Extends IDbConnection interface
			- Gives us a closer low-level experience while interacting with DB
		- Simplified API
		- Works with any Database
			- MySQL
			- PostGreSQL
			- More..
			
	- Github - https://github.com/DapperLib/Dapper

	- 2. Dapper Basics
		- Created a console application.
		- Reading connection string from appsettings in console application (Something new!)
			- Inject IConfigurationRoot in Program
			- Install package "Microsoft.Extensions.Configurations.Abstractions"
			- Create ConfigurationBuilder object and set basepath and json file (Refer Program)
			- For resolving error on Setbasepath, add "Microsoft.Extensions.Configuration.Json"
			- Added appsettings.json to console app
			- Set DefaultConnection value under Connection string

		- Update Appsettings.json from local machine
			- Query<T>("select statement that maps with T")
				- Refer ContactRepository.GetAll() method
