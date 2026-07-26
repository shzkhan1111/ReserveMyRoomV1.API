using Microsoft.AspNetCore.Mvc;
using ReserveMyRoom.API.Repository.Interface;

namespace ReserveMyRoom.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DataController : ControllerBase
{
    private readonly IDataService _dataService;

    public DataController(IDataService dataService)
    {
        _dataService = dataService;
    }

    [HttpDelete]
    public async Task<IActionResult> ResetDatabase(
        CancellationToken cancellationToken)
    {
        await _dataService.ResetDatabaseAsync(cancellationToken);
        return NoContent();
    }

    [HttpPost("seed")]
    public async Task<IActionResult> SeedDatabase(
        CancellationToken cancellationToken)
    {
        await _dataService.SeedDatabaseAsync(cancellationToken);
        return NoContent();
    }
}
