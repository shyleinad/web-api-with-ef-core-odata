# web-api-with-ef-core-odata

This project is a simple ASP.NET Core Web API that demonstrates the use of **Entity Framework Core (InMemory)** with **OData endpoints**.  
It provides CRUD operations for `Products` and `Categories`, including OData query support such as `$filter`, `$orderby`, `$expand`, and `$select`.

## How to Run the API

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- Visual Studio / VS Code (or any IDE with .NET support)

### Steps
1. Clone or download the repository.
2. Open a terminal in the project root.
3. Run the following command:
```
dotnet run
```
4. Once started, open your browser at: 
`https://localhost:<port>/swagger`
or test OData endpoints directly under:
`https://localhost:<port>/odata`

## Example OData Queries

### Products
| Description									| OData Query															|
|-----------------------------------------------|-----------------------------------------------------------------------|
| Get all products:								| /odata/Products														|
| Get product by ID:							| odata/Products(1)														|
| Filter products with price greater than 100:	| /odata/Products?$filter=Price gt 100									|
| Order by product name:						| /odata/Products?$orderby=Name											|
| Expand category details:						| /odata/Products?$expand=Category										|
| Select only name and price:					| /odata/Products?$select=Name,Price									|
| Combined example:								| /odata/Products?$filter=Price gt 100&$orderby=Name&$expand=Category	|

### Categories
| Description							| OData Query												|
|---------------------------------------|-----------------------------------------------------------|
| Get all categories:					| /odata/Categories											|
| Get category by ID:					| /odata/Categories(1)										|
| Expand products for each category:	| odata/Categories?$expand=Products							|
| Filter by name:						| /odata/Categories?$filter=contains(Name,'Electronics')	|

## Design Decisions and Assumptions

### Architecture
- Controller → Service → DbContext layered design
	- Controllers handle HTTP/OData endpoints and validation.
	- Services contain business logic and database interaction.
	- DbContext manages persistence using EF Core InMemory provider.

### Async Implementation
- All service and controller methods use async / await for non-blocking database operations.

### Logging
- Minimal built-in logging with ILogger<T> to console, logging key operations (Create, Update, Delete, Get).

### Database
- In-memory database (no external setup required).
- Seed data automatically created on startup in ProjectDbContext.OnModelCreating().

### OData Configuration
OData features enabled in `Program.cs`:
```
[...]
.Select()
.Filter()
.OrderBy()
.SetMaxTop(100)
.Count()
.SkipToken()
.Expand());
```

### Swagger
<ins>IMPORTANT! I recommend try it with either using directly the provided URLs in a browser or with the `web-api-with-ef-core-odata.http` file in the root folder of the project. The reason is that Swagger does not support OData properly by itself and it shows duplicated endpoints. They do work, but look weird.</ins>
- Swagger UI enabled only in development mode.
- Provides easy testing for both OData and standard API operations.

## Example CRUD Operations (via HTTP requests)
For an extended example, see the provided `web-api-with-ef-core-odata.http` file in the project root.
- Create product
```
POST /odata/Products
Content-Type: application/json

{
  "name": "Monitor",
  "price": 300,
  "categoryId": 1
}
```
- Update a Category
```
PUT /odata/Categories(2)
Content-Type: application/json

{
  "name": "Office Furniture"
}
```
- Delete a Product
```
DELETE /odata/Products(3)
```

## Techcnologies used
- .NET 8
- ASP.NET Core OData 8
- Entity Framework Core (InMemory)
- Swagger / Swashbuckle
- Dependency Injection
- Logging (Microsoft.Extensions.Logging)