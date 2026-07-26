

A hotel room booking API.


Deployed on 'https://reservemyroom-api-2026-htaygdh8hwgkhgc9.westcentralus-01.azurewebsites.net/swagger/index.html'

Requirements:

- .NET 10 SDK
- SQL Server

Setup:

1. Update the connection string in ReserveMyRoom/appsettings.json.
2. Apply the database migrations:

   dotnet ef database update --project ReserveMyRoom/ReserveMyRoom.API.csproj

   Alternatively, run database.sql in SQL Server Management Studio.
3. Run the API:

   dotnet run --project ReserveMyRoom/ReserveMyRoom.API.csproj

Swagger is available in Development at `/swagger`.

Run tests:

dotnet test ReserveMyRoom.slnx

Main endpoints:

- GET /api/hotels
- GET /api/hotels/search?name={name}
- GET /api/rooms/available?checkInDate={date}&checkOutDate={date}&numberOfGuests={guests}
- POST /api/bookings
- GET /api/bookings/{bookingReference}
- POST /api/data/seed
- DELETE /api/data

Room availability searches all hotels by default. Pass `hotelId` as an optional
query parameter to restrict the results to one hotel.


Business rules and assumptions:

- Each hotel has exactly 6 rooms.
- Hotels and rooms are immutable reference data populated through the seed
  endpoint; the API does not provide hotel or room management endpoints.
- Single, Double, and Deluxe are room categories only.
- Capacity is configured separately for every room.
- There is no fixed maximum booking size. The selected room must have enough
  capacity for the number of guests.
- The room distribution and capacities in the seed data are examples, not
  business rules.


Used AI to review and refactor the code, focusing on improvements that remain within the business requirements.

To Test the endpoint on swagger refer to TEST_GUIDE.md file 