using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ReserveMyRoom.API.Data;
using ReserveMyRoom.API.DTO.Booking;
using ReserveMyRoom.API.Models;
using ReserveMyRoom.API.Repository.Interface;

namespace ReserveMyRoom.API.Repository.Services;

public class BookingService : IBookingService
{
    private readonly ReserveMyRoomDbContext _context;

    public BookingService(ReserveMyRoomDbContext context)
    {
        _context = context;
    }

    public async Task<BookingResponse> CreateBookingAsync(
        RequestBooking request,
        CancellationToken cancellationToken = default)
    {
        ValidateRequest(request);

        IDbContextTransaction? transaction = null;
        if (_context.Database.IsRelational())
        {
            //Prevent race conditions by making the check and insert serializable
            transaction = await _context.Database.BeginTransactionAsync(
                IsolationLevel.Serializable,
                cancellationToken);
        }

        try
        {
            var room = await _context.Rooms
                .Include(candidate => candidate.Hotel)
                .SingleOrDefaultAsync(
                    candidate => candidate.Id == request.RoomId,
                    cancellationToken);

            if (room is null)
            {
                throw new KeyNotFoundException($"Room {request.RoomId} was not found.");
            }

            if (request.NumberOfGuests > room.Capacity)
            {
                throw new ArgumentException(
                    $"Room {room.RoomNumber} can accommodate a maximum of " +
                    $"{room.Capacity} guests.");
            }

            var overlapsExistingBooking = await _context.Bookings.AnyAsync(
                booking =>
                    booking.RoomId == request.RoomId &&
                    request.CheckInDate < booking.CheckOutDate &&
                    request.CheckOutDate > booking.CheckInDate,
                cancellationToken);

            if (overlapsExistingBooking)
            {
                throw new InvalidOperationException(
                    "The selected room is not available for these dates.");
            }

            var booking = new Booking
            {
                BookingReference = $"BK-{Guid.NewGuid():N}",
                GuestName = request.GuestName.Trim(),
                NumberOfGuests = request.NumberOfGuests,
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                RoomId = request.RoomId
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync(cancellationToken);

            if (transaction is not null)
            {
                await transaction.CommitAsync(cancellationToken);
            }

            return MapBooking(booking, room);
        }
        finally
        {
            if (transaction is not null)
            {
                await transaction.DisposeAsync();
            }
        }
    }

    public async Task<BookingResponse?> GetBookingByReferenceAsync(
        string bookingReference,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(bookingReference))
        {
            throw new ArgumentException("Booking reference is required.");
        }

        var normalizedReference = bookingReference.Trim().ToUpperInvariant();

        return await _context.Bookings
            .AsNoTracking()
            .Where(booking => booking.BookingReference == normalizedReference)
            .Select(booking => new BookingResponse
            {
                BookingReference = booking.BookingReference,
                GuestName = booking.GuestName,
                NumberOfGuests = booking.NumberOfGuests,
                CheckInDate = booking.CheckInDate,
                CheckOutDate = booking.CheckOutDate,
                HotelName = booking.Room.Hotel.Name,
                RoomNumber = booking.Room.RoomNumber,
                RoomType = booking.Room.RoomType
            })
            .SingleOrDefaultAsync(cancellationToken);
    }

    private static void ValidateRequest(RequestBooking request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.GuestName))
        {
            throw new ArgumentException("Guest name is required.");
        }

        if (request.GuestName.Trim().Length > 150)
        {
            throw new ArgumentException("Guest name cannot exceed 150 characters.");
        }

        if (request.NumberOfGuests is < 1 or > 4)
        {
            throw new ArgumentException("Number of guests must be between 1 and 4.");
        }

        if (request.CheckInDate == default || request.CheckOutDate == default)
        {
            throw new ArgumentException("Check-in and check-out dates are required.");
        }

        if (request.CheckOutDate <= request.CheckInDate)
        {
            throw new ArgumentException("Check-out date must be after check-in date.");
        }

        if (request.CheckInDate < DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new ArgumentException("Check-in date cannot be in the past.");
        }
    }

    private static BookingResponse MapBooking(Booking booking, Room room)
    {
        return new BookingResponse
        {
            BookingReference = booking.BookingReference,
            GuestName = booking.GuestName,
            NumberOfGuests = booking.NumberOfGuests,
            CheckInDate = booking.CheckInDate,
            CheckOutDate = booking.CheckOutDate,
            HotelName = room.Hotel.Name,
            RoomNumber = room.RoomNumber,
            RoomType = room.RoomType
        };
    }
}
