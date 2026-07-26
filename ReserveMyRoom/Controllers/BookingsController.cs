using Microsoft.AspNetCore.Mvc;
using ReserveMyRoom.API.DTO.Booking;
using ReserveMyRoom.API.Repository.Interface;

namespace ReserveMyRoom.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking(
        RequestBooking request,
        CancellationToken cancellationToken)
    {
        var booking = await _bookingService.CreateBookingAsync(
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetBookingByReference),
            new { bookingReference = booking.BookingReference },
            booking);
    }

    [HttpGet("{bookingReference}")]
    public async Task<IActionResult> GetBookingByReference(
        string bookingReference,
        CancellationToken cancellationToken)
    {
        var booking = await _bookingService.GetBookingByReferenceAsync(
            bookingReference,
            cancellationToken);

        return booking is null ? NotFound() : Ok(booking);
    }
}
