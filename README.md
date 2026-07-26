

A hotel room booking API.


Deployed on 'https://reservemyroom-api-2026-htaygdh8hwgkhgc9.westcentralus-01.azurewebsites.net/swagger/index.html'

Requirements:

- .NET 10 SDK
- SQL Server

Setup:

1. Update the connection string in ReserveMyRoom/appsettings.json.
2. Apply the database migrations:

   dotnet ef database update --project ReserveMyRoom/ReserveMyRoom.API.csproj
   (If any problem run database.sql In Sql Server)
3. Run the API:

   dotnet run --project ReserveMyRoom/ReserveMyRoom.API.csproj

Swagger is available in Development at `/swagger`.

Run tests:

dotnet test ReserveMyRoom.slnx

Main endpoints:

- GET /api/hotels
- GET /api/hotels/search?name={name}
- GET /api/hotels/{hotelId}/rooms/available
- POST /api/bookings
- GET /api/bookings/{bookingReference}
- POST /api/data/seed
- DELETE /api/data


Assumptions
-Each Hotel has 6 rooms
-2 single, 2 Double, 2 Deluxe rooms carrying 1, 2, 4 Guest respectively 
-A booking of more than 4 people is not allowed 

