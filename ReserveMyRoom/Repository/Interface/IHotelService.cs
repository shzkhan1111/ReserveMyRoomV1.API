using ReserveMyRoom.API.DTO.Hotels;
using ReserveMyRoom.API.DTO.Rooms;

namespace ReserveMyRoom.API.Repository.Interface;

public interface IHotelService
{
    Task<IReadOnlyList<HotelResponse>> GetAllHotelsAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HotelResponse>> GetHotelsByNameAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AvailableRoomResponse>> GetAvailableRoomsAsync(
        int hotelId,
        DateOnly checkInDate,
        DateOnly checkOutDate,
        int numberOfGuests,
        CancellationToken cancellationToken = default);
}
