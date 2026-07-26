using Microsoft.AspNetCore.Mvc;
using Moq;
using ReserveMyRoom.API.Controllers;
using ReserveMyRoom.API.DTO.Booking;
using ReserveMyRoom.API.DTO.Hotels;
using ReserveMyRoom.API.DTO.Rooms;
using ReserveMyRoom.API.Repository.Interface;

namespace ReserveMyRoom.API.Tests.Controllers;

public class ControllerTests
{
    [Fact]
    public async Task GetAvailableRooms_AllowsHotelFilterToBeOmitted()
    {
        var rooms = new[]
        {
            new AvailableRoomResponse
            {
                HotelId = 1,
                HotelName = "Test Hotel",
                RoomId = 10,
                RoomNumber = "101",
                Capacity = 2
            }
        };
        var service = new Mock<IRoomService>();
        var checkIn = new DateOnly(2026, 8, 1);
        var checkOut = new DateOnly(2026, 8, 2);
        service.Setup(candidate => candidate.GetAvailableRoomsAsync(
                checkIn,
                checkOut,
                2,
                null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(rooms);
        var controller = new RoomsController(service.Object);

        var result = await controller.GetAvailableRooms(
            checkIn,
            checkOut,
            2,
            null,
            CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(rooms, ok.Value);
    }

    [Fact]
    public async Task CreateBooking_ReturnsCreatedWithLookupRoute()
    {
        var response = new BookingResponse { BookingReference = "BK-123" };
        var service = new Mock<IBookingService>();
        service.Setup(candidate => candidate.CreateBookingAsync(
                It.IsAny<RequestBooking>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new BookingsController(service.Object);

        var result = await controller.CreateBooking(
            new RequestBooking(),
            CancellationToken.None);

        var created = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(
            nameof(BookingsController.GetBookingByReference),
            created.ActionName);
        Assert.Equal("BK-123", created.RouteValues!["bookingReference"]);
        Assert.Same(response, created.Value);
    }

    [Fact]
    public async Task GetBookingByReference_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<IBookingService>();
        service.Setup(candidate => candidate.GetBookingByReferenceAsync(
                "BK-MISSING",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((BookingResponse?)null);
        var controller = new BookingsController(service.Object);

        var result = await controller.GetBookingByReference(
            "BK-MISSING",
            CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task SearchHotels_ReturnsEmptyCollection_WhenNoHotelsMatch()
    {
        var service = new Mock<IHotelService>();
        service.Setup(candidate => candidate.GetHotelsByNameAsync(
                "missing",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<HotelResponse>());
        var controller = new HotelsController(service.Object);

        var result = await controller.SearchHotels(
            "missing",
            CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Empty(Assert.IsAssignableFrom<
            IReadOnlyList<HotelResponse>>(ok.Value));
    }

    [Fact]
    public async Task ResetDatabase_ReturnsNoContent_AndCallsService()
    {
        var service = new Mock<IDataService>();
        var controller = new DataController(service.Object);

        var result = await controller.ResetDatabase(CancellationToken.None);

        Assert.IsType<NoContentResult>(result);
        service.Verify(candidate => candidate.ResetDatabaseAsync(
            CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task SeedDatabase_ReturnsNoContent_AndCallsService()
    {
        var service = new Mock<IDataService>();
        var controller = new DataController(service.Object);

        var result = await controller.SeedDatabase(CancellationToken.None);

        Assert.IsType<NoContentResult>(result);
        service.Verify(candidate => candidate.SeedDatabaseAsync(
            CancellationToken.None), Times.Once);
    }
}
