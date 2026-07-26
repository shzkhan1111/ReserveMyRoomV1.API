using ReserveMyRoom.API.DTO.Hotels;

namespace ReserveMyRoom.API.Repository.Interface;

public interface IHotelService
{
    Task<IReadOnlyList<HotelResponse>> GetAllHotelsAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HotelResponse>> GetHotelsByNameAsync(
        string name,
        CancellationToken cancellationToken = default);

}
