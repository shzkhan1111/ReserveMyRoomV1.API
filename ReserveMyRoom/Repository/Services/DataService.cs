using Microsoft.EntityFrameworkCore;
using ReserveMyRoom.API.Data;
using ReserveMyRoom.API.Repository.Interface;

namespace ReserveMyRoom.API.Repository.Services;

public class DataService : IDataService
{
    private readonly ReserveMyRoomDbContext _context;

    public DataService(ReserveMyRoomDbContext context)
    {
        _context = context;
    }

    public async Task ResetDatabaseAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.Bookings.ExecuteDeleteAsync(cancellationToken);
        await _context.Rooms.ExecuteDeleteAsync(cancellationToken);
        await _context.Hotels.ExecuteDeleteAsync(cancellationToken);
    }

    public Task SeedDatabaseAsync(
        CancellationToken cancellationToken = default)
    {
        DatabaseSeeder.Seed(_context);
        return Task.CompletedTask;
    }
}
