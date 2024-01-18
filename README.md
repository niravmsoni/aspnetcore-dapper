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
	- Basic CRUD Ops
		- Refer ContactRepository for basic CRUD Ops
			- List
				- Query<T>("select statement that maps with T")
					- Refer ContactRepository.GetAll() method
		
			- List with objects that do not match column names
				- Use the same Query<T> method
				- In Select statement, make sure to use Alias. 
				- Refer ContactRepository.GetAll()

			- Saving record to DB
				- Ideally we should use Command method but since we want to return the ID that we have asked here - SELECT CAST(SCOPE_IDENTITY() as int)
				- We are using Query method

			- Updating record to DB
				- Passing object and mapping them in Update SQL statement
				- We are using Execute method

			- Delete record from DB
				- Using Execute method to delete record from database by passing in PK

	- Using DapperContrib
		- Add-on library for enhancing Dapper
		- Provides methods to enable us to write less code for basic CRUD ops
		- Allows us to do basic CRUD without having to type entire SQL
		- See GitHub link - https://github.com/DapperLib/Dapper.Contrib
		- Adding DapperContrib to data layer
		- Refer ContactRepositoryUsingDapperContrib for CRUD Ops using DapperContrib
		- Compare and contrast changes with ContactRepository
		- In a nutshell DapperContrib is useful when we have simple straight-forward object mappings. Benefit - We can avoid typing in SQL statements
		- Important
			- Usecase when we have a few property in data model that are computed and not present in DB
				- Refer Contact
					- IsNew - Not present in DB
						- Marking it as Computed

					- Address - No column present in DB
						- Marking Write(false) since we do not want DapperContrib to generate field in insert statement


		
	- Implementing 1..* relations 
		- 1 Contact can have 1..* addresses
		- For retriving such records, we can use QueryMultiple() method from Dapper.
		- See ContactRepository GetFullContact() method
		- Implemented Save method that takes care of both Insert/Update using IsNew property

	- Issue - While using transaction scope locally, getting this error - UPDATES
		- This platform does not support distributed transactions.
		- Maybe it is because SQL Server I am using does not support this usecase
		- This issue is resolved. It was occurring because within transaction scope, there were different SQL connections getting opened up. After sharing the connection from within the TxScope, it resolves the issue
		- Refer answer on StackOverflow here - https://stackoverflow.com/questions/56387125/two-dbcontexts-on-same-server-throws-this-platform-does-not-support-distribute/77841154#77841154

-3. Beyond Basics
	- Stored Procedures and Dynamic Params
		- Dapper does support interacting to database via SPs

		- Implementation	
			- Refer Stored procs present under Database project
			- Refer ContactRepositoryUsingStoredProc for studying interactions with DB
			- Most of the code is pretty much the same. We need to set the CommandType parameter in Execute() or Query() or QueryMultiple() method as CommandType.StoredProcedure
	
	- Working with Arrays that go as IN statement in WHERE clause
		- Dapper is able to convert [] into comma separated values required in IN statement in SQL
		- See ContactRepositoryAdditionalOperations -> GetContactsById method