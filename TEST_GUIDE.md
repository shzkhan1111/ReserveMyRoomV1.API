# Test Guide

## 1) Find Hotels

Find all hotels:

`GET /api/Hotels`

Find a hotel by name:

`GET /api/hotels/search?name=Rosewood`

## 2) Find Available Rooms

Search across all hotels:

`GET /api/rooms/available?checkInDate=2026-08-10&checkOutDate=2026-08-12&numberOfGuests=2`

Or search within a specific hotel:

`GET /api/rooms/available?checkInDate=2026-08-10&checkOutDate=2026-08-12&numberOfGuests=2&hotelId=13`

## 3) Book a Room

Select an available room and copy its `roomId`.

Call:

`POST /api/bookings`

Example request:

```json
{
  "roomId": 45,
  "guestName": "Rahul",
  "numberOfGuests": 2,
  "checkInDate": "2026-08-10",
  "checkOutDate": "2026-08-12"
}
```

Ensure the number of guests is less than or equal to the room's capacity.

Example response:

```json
{
  "bookingReference": "BK-F7A01E10BB5941CBBE867B8EB6F5D5F5",
  "guestName": "Rahul",
  "numberOfGuests": 2,
  "checkInDate": "2026-08-10",
  "checkOutDate": "2026-08-12",
  "hotelName": "Rosewood",
  "roomNumber": "201",
  "roomType": "Double"
}
```

## 4) Find the Booking by Reference

Copy the `bookingReference` returned by the booking request.

Call:

`GET https://localhost:7222/api/Bookings/BK-F7A01E10BB5941CBBE867B8EB6F5D5F5`

Expected response:

```json
{
  "bookingReference": "BK-F7A01E10BB5941CBBE867B8EB6F5D5F5",
  "guestName": "Rahul",
  "numberOfGuests": 2,
  "checkInDate": "2026-08-10",
  "checkOutDate": "2026-08-12",
  "hotelName": "Rosewood",
  "roomNumber": "201",
  "roomType": "Double"
}
```

## 5) Confirm the Room Is No Longer Available

Repeat:

`GET /api/rooms/available?checkInDate=2026-08-10&checkOutDate=2026-08-12&numberOfGuests=2&hotelId=13`

The recently booked room should no longer appear in the available-room results.

## 6) Test Overlapping Dates

Submit another booking for the same room using overlapping dates.

Expected response:

`409 Conflict`
