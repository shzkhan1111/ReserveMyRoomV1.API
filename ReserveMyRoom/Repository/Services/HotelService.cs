using Microsoft.EntityFrameworkCore;
using ReserveMyRoom.API.Data;
using ReserveMyRoom.API.DTO.Hotels;
using ReserveMyRoom.API.DTO.Rooms;
using ReserveMyRoom.API.Enums;
using ReserveMyRoom.API.Repository.Interface;

namespace ReserveMyRoom.API.Repository.Services;

public class HotelService : IHotelService
{
    private readonly ReserveMyRoomDbContext _context;

    public HotelService(ReserveMyRoomDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<HotelResponse>> GetAllHotelsAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Hotels
            .AsNoTracking()
            .OrderBy(hotel => hotel.Name)
            .Select(hotel => new HotelResponse
            {
                HotelId = hotel.Id,
                Name = hotel.Name,
                Address = hotel.Address
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AvailableRoomResponse>> GetAvailableRoomsAsync(
        int hotelId,
        DateOnly checkInDate,
        DateOnly checkOutDate,
        int numberOfGuests,
        CancellationToken cancellationToken = default)
    {
        ValidateStay(checkInDate, checkOutDate, numberOfGuests);

        if (!await _context.Hotels
                .AsNoTracking()
                .AnyAsync(hotel => hotel.Id == hotelId, cancellationToken))
        {
            throw new KeyNotFoundException($"Hotel {hotelId} was not found.");
        }

        return await _context.Rooms
            .AsNoTracking()
            .Where(room =>
                room.HotelId == hotelId &&
                ((room.RoomType == RoomType.Single && numberOfGuests <= 1) ||
                 (room.RoomType == RoomType.Double && numberOfGuests <= 2) ||
                 (room.RoomType == RoomType.Deluxe && numberOfGuests <= 4)) &&
                !room.Bookings.Any(booking =>
                    checkInDate < booking.CheckOutDate &&
                    checkOutDate > booking.CheckInDate))
            .OrderBy(room => room.RoomNumber)
            .Select(room => new AvailableRoomResponse
            {
                RoomId = room.Id,
                RoomNumber = room.RoomNumber,
                RoomType = room.RoomType,
                Capacity = room.RoomType == RoomType.Single
                    ? 1
                    : room.RoomType == RoomType.Double ? 2 : 4
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<HotelResponse>> GetHotelsByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Hotel name is required.", nameof(name));
        }

        var searchTerm = name.Trim();

        return await _context.Hotels
            .AsNoTracking()
            .Where(hotel => hotel.Name.Contains(searchTerm))
            .OrderBy(hotel => hotel.Name)
            .Select(hotel => new HotelResponse
            {
                HotelId = hotel.Id,
                Name = hotel.Name,
                Address = hotel.Address
            })
            .ToListAsync(cancellationToken);
    }

    private static void ValidateStay(
        DateOnly checkInDate,
        DateOnly checkOutDate,
        int numberOfGuests)
    {
        if (checkInDate == default || checkOutDate == default)
        {
            throw new ArgumentException("Check-in and check-out dates are required.");
        }

        if (checkOutDate <= checkInDate)
        {
            throw new ArgumentException("Check-out date must be after check-in date.");
        }

        if (numberOfGuests is < 1 or > 4)
        {
            throw new ArgumentException("Number of guests must be between 1 and 4.");
        }

        if (checkInDate < DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new ArgumentException("Check-in date cannot be in the past.");
        }
    }
}
