using ReserveMyRoom.API.DTO.Booking;

namespace ReserveMyRoom.API.Repository.Interface
{
    public interface IBookingService
    {
        Task<BookingResponse> CreateBookingAsync(
            RequestBooking request,
            CancellationToken cancellationToken = default);
        Task<BookingResponse?> GetBookingByReferenceAsync(
            string bookingReference,
            CancellationToken cancellationToken = default);
    }
}
