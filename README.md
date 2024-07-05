# Simple shopping cart

This is a simple ASP.Net core web api application where users create new shopping carts and add items to the shopping carts.

Each shopping cart has a name associated with it when it is created. An ID is assigned to it when it is added to the database. Each shopping cart can have a collection of items. Each item has a `Name`, `Quantity` and `Price`. 

Shopping carts can be created, viewed and deleted. Inside each shopping cart, items can be added, removed, viewed and updated.

## Dependencies

The project is developed using .Net 8. The web framework is Asp.Net core, data access is done through EF Core and associated libraries.

### Running the project
To run the web api, please have dotnet 8 sdk installed on the machine. Run the following command from the root of the repository
```bash
dotnet run --project ShoppingCartWeb
```
This will start the Asp.Net web API application running on port `5105`. It should also open a browser and open the swagger UI.


### Testing

Simple tests of the data access layer is done in the test project. Test project also has a `.http` file that shows some basic interactions with the web API.

The tests use basic mocking to mock the logging library and uses EF core's in memory database to test CRUD operations.


### Assumptions made

* There is no authentication and authorization mechanism in place.
* All shopping carts can be viewed.
* This project uses in memory database.
* No optimizations or considerations for query performance is made here.
* Nested resources for creation of shopping carts and insertion of items into the shopping cart.

#### Publishing the project

This project has been configured to be published as a docker container. Run the following command to publish a container

```bash
dotnet publish --os linux --arch x64 /t:PublishContainer
```

> Note: Docker should be running on the machine. For troubleshooting, please use: https://learn.microsoft.com/en-us/dotnet/core/docker/publish-as-container?pivots=dotnet-8-0