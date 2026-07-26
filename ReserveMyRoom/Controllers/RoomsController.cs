using Microsoft.AspNetCore.Mvc;
using ReserveMyRoom.API.Repository.Interface;

namespace ReserveMyRoom.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomsController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableRooms(
        [FromQuery] DateOnly checkInDate,
        [FromQuery] DateOnly checkOutDate,
        [FromQuery] int numberOfGuests,
        [FromQuery] int? hotelId,
        CancellationToken cancellationToken)
    {
        var rooms = await _roomService.GetAvailableRoomsAsync(
            checkInDate,
            checkOutDate,
            numberOfGuests,
            hotelId,
            cancellationToken);

        return Ok(rooms);
    }
}
