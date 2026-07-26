using Microsoft.AspNetCore.Mvc;
using ReserveMyRoom.API.DTO.Hotels;
using ReserveMyRoom.API.DTO.Rooms;
using ReserveMyRoom.API.Repository.Interface;

namespace ReserveMyRoom.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelsController : ControllerBase
{
    private readonly IHotelService _hotelService;

    public HotelsController(IHotelService hotelService)
    {
        _hotelService = hotelService;
    }

    [HttpGet]
    public async Task<IActionResult> GetHotels(
        CancellationToken cancellationToken)
    {
        var hotels = await _hotelService.GetAllHotelsAsync(cancellationToken);
        return Ok(hotels);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchHotels(
        [FromQuery] string name,
        CancellationToken cancellationToken)
    {
        var hotels = await _hotelService.GetHotelsByNameAsync(
            name,
            cancellationToken);

        return Ok(hotels);
    }

}
