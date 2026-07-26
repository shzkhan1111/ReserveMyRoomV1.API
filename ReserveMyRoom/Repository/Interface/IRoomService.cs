using ReserveMyRoom.API.DTO.Rooms;

namespace ReserveMyRoom.API.Repository.Interface;

public interface IRoomService
{
    Task<IReadOnlyList<AvailableRoomResponse>> GetAvailableRoomsAsync(
        DateOnly checkInDate,
        DateOnly checkOutDate,
        int numberOfGuests,
        int? hotelId = null,
        CancellationToken cancellationToken = default);
}
