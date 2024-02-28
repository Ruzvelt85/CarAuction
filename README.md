# Car Auction Management System

This solution represents REST API for car auction management system.

It doesn't contain any user interface - requests can be tested via Swagger UI (or Postman).

InMemory Database is used for storing data.

###  Solution structure

The solution consists of 6 projects:

**WebApi**: Contains the API controllers, query handler, command handlers, validators, action filter, mapping configurations, etc.

**Domain**: Defines domain model and custom exception classes

**Dto**: Contains DTO models for requests and responses

**Patterns**: Contains core interfaces.

**Data**: Contains interaction with data layer (EF Core context, repositories, unit of work).

**Tests**: Contains unit tests for controllers, query handler, command handlers, validators, and mapping.

This structure can be a little bit extensive or even redundant for the scope of the given task, but it will be able to accomodate the following features well.

It was decided not to create separate entities for each type of vehicle - in order not to bring a redundant complexity within a scope of this task. But in case of appearing a specific business logics for different vehicles, they can be created.


###  Requests

**POST /Inventory**: Adds a vehicle to the auction inventory.

Each vehicle has its type, a unique identifier, and respective attributes based on its type.
In case of presence of unappropriate attribute (e.g., Load Capacity for Sedan type), the API will return BadRequest result.

**GET /Inventory**: Searches for vehicles by type, manufacturer, model, or year; the filter can be empty

**POST /Auctions**: Starts the auction for the specific vehicle

**PUT /Auctions**: Closes the auction for the specific vehicle

**POST /Bids**: Places a bid on the vehicle within an active auction


###  Build and Run

To build and run the solution, please perform the following steps:

1) Open the solution in MS Visual Studio
2) Execute command Build - Build solution
3) To run API press F5

API will be explored through Swagger at https://localhost:7116/swagger/index.html

Alternatively, the solution can be built and run through CLI:
1) Go to the solution folder and make build with command 'dotnet build'
2) Run API with command 'the dotnet run --project src/CarAuctionApi.WebApi' 

###  Ways of enhancement

1) Classes of specifications can be created for more flexible filtering of vehicles
2) Paging can be implemented for GET request
3) Vehicle model can be divided into separate children entities with a common base class - in case of appearing a specific business logic for each vehicle type
4) User authenticiation can be added; POST-request for placing a bid can be enriched with UserId within processing
5) More unit tests (for repositories, action filter, middleware) and integration tests should be added
6) A real database (with migrations and seeding) can be added to testing purposes within a docker container. The entire solution with a database can be run with Docker-Compose
7) A simple UI can be implemented
