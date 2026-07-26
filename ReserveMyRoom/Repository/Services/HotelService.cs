using Microsoft.EntityFrameworkCore;
using ReserveMyRoom.API.Data;
using ReserveMyRoom.API.DTO.Hotels;
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

}
