using Microsoft.EntityFrameworkCore;
using ReserveMyRoom.API.Data;
using ReserveMyRoom.API.DTO.Rooms;
using ReserveMyRoom.API.Repository.Interface;

namespace ReserveMyRoom.API.Repository.Services;

public class RoomService : IRoomService
{
    private readonly ReserveMyRoomDbContext _context;

    public RoomService(ReserveMyRoomDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<AvailableRoomResponse>>
        GetAvailableRoomsAsync(
            DateOnly checkInDate,
            DateOnly checkOutDate,
            int numberOfGuests,
            int? hotelId = null,
            CancellationToken cancellationToken = default)
    {
        StayValidator.Validate(
            checkInDate,
            checkOutDate,
            numberOfGuests);

        if (hotelId.HasValue &&
            !await _context.Hotels
                .AsNoTracking()
                .AnyAsync(
                    hotel => hotel.Id == hotelId.Value,
                    cancellationToken))
        {
            throw new KeyNotFoundException(
                $"Hotel {hotelId.Value} was not found.");
        }

        return await _context.Rooms
            .AsNoTracking()
            .Where(room =>
                (!hotelId.HasValue || room.HotelId == hotelId.Value) &&
                room.Capacity >= numberOfGuests &&
                !room.Bookings.Any(booking =>
                    checkInDate < booking.CheckOutDate &&
                    checkOutDate > booking.CheckInDate))
            .OrderBy(room => room.Hotel.Name)
            .ThenBy(room => room.RoomNumber)
            .Select(room => new AvailableRoomResponse
            {
                HotelId = room.HotelId,
                HotelName = room.Hotel.Name,
                RoomId = room.Id,
                RoomNumber = room.RoomNumber,
                RoomType = room.RoomType,
                Capacity = room.Capacity
            })
            .ToListAsync(cancellationToken);
    }
}
