using Microsoft.EntityFrameworkCore;
using ReserveMyRoom.API.Data;
using ReserveMyRoom.API.DTO.Booking;
using ReserveMyRoom.API.Enums;
using ReserveMyRoom.API.Models;
using ReserveMyRoom.API.Repository.Services;

namespace ReserveMyRoom.API.Tests.Services
{
    public class BookingServiceTests
    {
        private ReserveMyRoomDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ReserveMyRoomDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ReserveMyRoomDbContext(options);
        }

        [Fact]
        public async Task CreateBookingAsync_ShouldThrowException_WhenGuestCountExceedsRoomCapacity()
        {
            await using var context = CreateContext();

            var hotel = new Hotel
            {
                Name = "Test Hotel",
                Address = "London"
            };

            var room = new Room
            {
                RoomNumber = "101",
                RoomType = RoomType.Single,
                Capacity = 1,
                Hotel = hotel
            };

            context.Rooms.Add(room);
            await context.SaveChangesAsync();

            var bookingService = new BookingService(context);

            var request = new RequestBooking
            {
                RoomId = room.Id,
                GuestName = "Test Guest",
                NumberOfGuests = 2,
                CheckInDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                CheckOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(2))
            };

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => bookingService.CreateBookingAsync(request));

            Assert.Equal(
                "Room 101 can accommodate a maximum of 1 guests.",
                exception.Message);
        }

        [Fact]
        public async Task CreateBookingAsync_ShouldCreateBooking_WhenRequestIsValid()
        {
            await using var context = CreateContext();

            var hotel = new Hotel
            {
                Name = "Test Hotel",
                Address = "London"
            };

            var room = new Room
            {
                RoomNumber = "101",
                RoomType = RoomType.Double,
                Capacity = 2,
                Hotel = hotel
            };

            context.Rooms.Add(room);
            await context.SaveChangesAsync();

            var bookingService = new BookingService(context);

            var request = new RequestBooking
            {
                RoomId = room.Id,
                GuestName = "Alex Bales",
                NumberOfGuests = 2,
                CheckInDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                CheckOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(3))
            };

            var result = await bookingService.CreateBookingAsync(request);

            Assert.NotNull(result);
            Assert.Equal("Alex Bales", result.GuestName);
            Assert.Equal(room.RoomNumber, result.RoomNumber);
            Assert.Equal(room.RoomType, result.RoomType);
            Assert.Equal(hotel.Name, result.HotelName);
            Assert.Equal(request.CheckInDate, result.CheckInDate);
            Assert.Equal(request.CheckOutDate, result.CheckOutDate);

            Assert.StartsWith("BK-", result.BookingReference);
            Assert.Equal(
                result.BookingReference.ToUpperInvariant(),
                result.BookingReference);

            Assert.Equal(1, context.Bookings.Count());
        }

        [Fact]
        public async Task CreateBookingAsync_ShouldThrowException_WhenRoomIsAlreadyBooked()
        {
            await using var context = CreateContext();

            var hotel = new Hotel
            {
                Name = "Test Hotel",
                Address = "London"
            };

            var room = new Room
            {
                RoomNumber = "101",
                RoomType = RoomType.Double,
                Capacity = 2,
                Hotel = hotel
            };

            context.Rooms.Add(room);
            await context.SaveChangesAsync();

            context.Bookings.Add(new Booking
            {
                BookingReference = "BK-TEST-001",
                GuestName = "Existing Guest",
                NumberOfGuests = 2,
                CheckInDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5)),
                CheckOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(8)),
                RoomId = room.Id
            });

            await context.SaveChangesAsync();

            var bookingService = new BookingService(context);

            var request = new RequestBooking
            {
                RoomId = room.Id,
                GuestName = "Ronaldo Portugal",
                NumberOfGuests = 2,
                CheckInDate = DateOnly.FromDateTime(DateTime.Today.AddDays(6)),
                CheckOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(9))
            };

            
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => bookingService.CreateBookingAsync(request));

            Assert.Equal(
                "The selected room is not available for these dates.",
                exception.Message);
        }

        [Fact]
        public async Task GetBookingByReferenceAsync_ShouldReturnBooking_WhenBookingExists()
        {
            await using var context = CreateContext();

            var hotel = new Hotel
            {
                Name = "Test Hotel",
                Address = "London"
            };

            var room = new Room
            {
                RoomNumber = "201",
                RoomType = RoomType.Double,
                Capacity = 2,
                Hotel = hotel
            };

            var booking = new Booking
            {
                BookingReference = "BK-TEST-001",
                GuestName = "Santa Claws",
                NumberOfGuests = 2,
                CheckInDate = DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
                CheckOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(4)),
                Room = room
            };

            context.Bookings.Add(booking);
            await context.SaveChangesAsync();

            var bookingService = new BookingService(context);

            var result = await bookingService
                .GetBookingByReferenceAsync("bk-test-001");

            Assert.NotNull(result);
            Assert.Equal("BK-TEST-001", result.BookingReference);
            Assert.Equal("Santa Claws", result.GuestName);
            Assert.Equal(2, result.NumberOfGuests);
            Assert.Equal("Test Hotel", result.HotelName);
            Assert.Equal("201", result.RoomNumber);
            Assert.Equal(RoomType.Double, result.RoomType);
        }

        [Fact]
        public async Task GetBookingByReferenceAsync_ShouldReturnNull_WhenBookingDoesNotExist()
        {
            await using var context = CreateContext();

            var bookingService = new BookingService(context);

            var result = await bookingService
                .GetBookingByReferenceAsync("BK-Book not found");

            Assert.Null(result);
        }
        [Fact]
        public async Task CreateBookingAsync_ShouldThrowException_WhenRoomDoesNotExist()
        {
            await using var context = CreateContext();

            var bookingService = new BookingService(context);

            var request = new RequestBooking
            {
                RoomId = 258,
                GuestName = "Shahzaib Ahmed Khan",
                NumberOfGuests = 2,
                CheckInDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                CheckOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(3))
            };

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => bookingService.CreateBookingAsync(request));

            Assert.Equal("Room 258 was not found.", exception.Message);
        }

        [Fact]
        public async Task GetHotelsByNameAsync_ShouldReturnMatchingHotels()
        {
            await using var context = CreateContext();

            context.Hotels.AddRange(
                new Hotel
                {
                    Name = "London Hotel",
                    Address = "London"
                },
                new Hotel
                {
                    Name = "Manchester Hotel",
                    Address = "Manchester"
                });

            await context.SaveChangesAsync();

            var hotelService = new HotelService(context);

            var result = await hotelService.GetHotelsByNameAsync("London");

            Assert.Single(result);
            Assert.Equal("London Hotel", result.First().Name);
        }

        [Fact]
        public async Task GetAvailableRoomsAsync_ShouldUseConfiguredRoomCapacity()
        {
            await using var context = CreateContext();

            var hotel = new Hotel
            {
                Name = "Test Hotel",
                Address = "London"
            };

            var higherCapacityRoom = new Room
            {
                RoomNumber = "101",
                RoomType = RoomType.Single,
                Capacity = 3,
                Hotel = hotel
            };

            var lowerCapacityRoom = new Room
            {
                RoomNumber = "102",
                RoomType = RoomType.Double,
                Capacity = 1,
                Hotel = hotel
            };

            context.Rooms.AddRange(higherCapacityRoom, lowerCapacityRoom);
            await context.SaveChangesAsync();

            var roomService = new RoomService(context);

            var result = await roomService.GetAvailableRoomsAsync(
                DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
                2,
                hotel.Id);

            Assert.Single(result);
            Assert.Equal("101", result.First().RoomNumber);
            Assert.Equal(3, result.First().Capacity);
        }

        [Fact]
        public async Task GetAvailableRoomsAsync_ShouldReturnRoom_WhenExistingBookingEndsOnCheckInDate()
        {
            // Arrange
            await using var context = CreateContext();

            var hotel = new Hotel
            {
                Name = "Test Hotel",
                Address = "London"
            };

            var room = new Room
            {
                RoomNumber = "101",
                RoomType = RoomType.Double,
                Capacity = 2,
                Hotel = hotel
            };

            context.Rooms.Add(room);
            await context.SaveChangesAsync();

            context.Bookings.Add(new Booking
            {
                BookingReference = "BK-TEST-001",
                GuestName = "Existing Guest",
                NumberOfGuests = 2,
                CheckInDate = DateOnly.FromDateTime(DateTime.Today.AddDays(3)),
                CheckOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5)),
                RoomId = room.Id
            });

            await context.SaveChangesAsync();

            var roomService = new RoomService(context);

            var requestedCheckIn =
                DateOnly.FromDateTime(DateTime.Today.AddDays(5));

            var requestedCheckOut =
                DateOnly.FromDateTime(DateTime.Today.AddDays(7));

            // Act
            var result = await roomService.GetAvailableRoomsAsync(
                requestedCheckIn,
                requestedCheckOut,
                2,
                hotel.Id);

            // Assert
            Assert.Single(result);
            Assert.Equal("101", result.First().RoomNumber);
        }

        [Fact]
        public async Task CreateBookingAsync_ShouldAllowAdjacentBooking()
        {
            await using var context = CreateContext();
            var room = AddRoom(context);
            var existingCheckIn = FutureDate(5);

            context.Bookings.Add(new Booking
            {
                BookingReference = "BK-EXISTING",
                GuestName = "Existing Guest",
                NumberOfGuests = 1,
                CheckInDate = existingCheckIn,
                CheckOutDate = FutureDate(8),
                Room = room
            });
            await context.SaveChangesAsync();

            var service = new BookingService(context);
            var result = await service.CreateBookingAsync(new RequestBooking
            {
                RoomId = room.Id,
                GuestName = "Next Guest",
                NumberOfGuests = 1,
                CheckInDate = FutureDate(3),
                CheckOutDate = existingCheckIn
            });

            Assert.Equal(2, await context.Bookings.CountAsync());
            Assert.Equal(existingCheckIn, result.CheckOutDate);
        }

        [Fact]
        public async Task CreateBookingAsync_ShouldRejectBookingContainedWithinExistingStay()
        {
            await using var context = CreateContext();
            var room = AddRoom(context);

            context.Bookings.Add(new Booking
            {
                BookingReference = "BK-EXISTING",
                GuestName = "Existing Guest",
                NumberOfGuests = 1,
                CheckInDate = FutureDate(3),
                CheckOutDate = FutureDate(8),
                Room = room
            });
            await context.SaveChangesAsync();

            var service = new BookingService(context);
            var request = new RequestBooking
            {
                RoomId = room.Id,
                GuestName = "Overlapping Guest",
                NumberOfGuests = 1,
                CheckInDate = FutureDate(4),
                CheckOutDate = FutureDate(5)
            };

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.CreateBookingAsync(request));
        }

        [Theory]
        [InlineData(0)]
        public async Task CreateBookingAsync_ShouldRejectInvalidGuestCount(
            int numberOfGuests)
        {
            await using var context = CreateContext();
            var room = AddRoom(context);
            await context.SaveChangesAsync();
            var service = new BookingService(context);

            var request = new RequestBooking
            {
                RoomId = room.Id,
                GuestName = "Test Guest",
                NumberOfGuests = numberOfGuests,
                CheckInDate = FutureDate(1),
                CheckOutDate = FutureDate(2)
            };

            await Assert.ThrowsAsync<ArgumentException>(
                () => service.CreateBookingAsync(request));
        }

        [Fact]
        public async Task CreateBookingAsync_ShouldRejectBlankGuestName()
        {
            await using var context = CreateContext();
            var room = AddRoom(context);
            await context.SaveChangesAsync();
            var service = new BookingService(context);

            var request = new RequestBooking
            {
                RoomId = room.Id,
                GuestName = "   ",
                NumberOfGuests = 1,
                CheckInDate = FutureDate(1),
                CheckOutDate = FutureDate(2)
            };

            await Assert.ThrowsAsync<ArgumentException>(
                () => service.CreateBookingAsync(request));
        }

        [Fact]
        public async Task CreateBookingAsync_ShouldAllowMoreThanFourGuests_WhenRoomCapacityAllows()
        {
            await using var context = CreateContext();
            var room = AddRoom(context);
            await context.SaveChangesAsync();
            var service = new BookingService(context);

            var result = await service.CreateBookingAsync(new RequestBooking
            {
                RoomId = room.Id,
                GuestName = "Large Group",
                NumberOfGuests = 5,
                CheckInDate = FutureDate(1),
                CheckOutDate = FutureDate(2)
            });

            Assert.Equal(5, result.NumberOfGuests);
        }

        [Fact]
        public async Task GetAvailableRoomsAsync_ShouldThrow_WhenHotelDoesNotExist()
        {
            await using var context = CreateContext();
            var service = new RoomService(context);

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                service.GetAvailableRoomsAsync(
                    FutureDate(1),
                    FutureDate(2),
                    1,
                    999));
        }

        [Fact]
        public async Task GetAvailableRoomsAsync_ShouldSearchAcrossAllHotels_WhenFilterIsOmitted()
        {
            await using var context = CreateContext();
            context.Rooms.AddRange(
                new Room
                {
                    RoomNumber = "101",
                    RoomType = RoomType.Single,
                    Capacity = 2,
                    Hotel = new Hotel
                    {
                        Name = "London Hotel",
                        Address = "London"
                    }
                },
                new Room
                {
                    RoomNumber = "201",
                    RoomType = RoomType.Deluxe,
                    Capacity = 5,
                    Hotel = new Hotel
                    {
                        Name = "Paris Hotel",
                        Address = "Paris"
                    }
                });
            await context.SaveChangesAsync();
            var service = new RoomService(context);

            var result = await service.GetAvailableRoomsAsync(
                FutureDate(1),
                FutureDate(2),
                2);

            Assert.Equal(2, result.Count);
            Assert.Collection(
                result,
                room =>
                {
                    Assert.Equal("London Hotel", room.HotelName);
                    Assert.True(room.HotelId > 0);
                },
                room =>
                {
                    Assert.Equal("Paris Hotel", room.HotelName);
                    Assert.True(room.HotelId > 0);
                });
        }

        [Fact]
        public async Task GetHotelsByNameAsync_ShouldThrow_WhenNameIsBlank()
        {
            await using var context = CreateContext();
            var service = new HotelService(context);

            await Assert.ThrowsAsync<ArgumentException>(
                () => service.GetHotelsByNameAsync("  "));
        }

        private static Room AddRoom(ReserveMyRoomDbContext context)
        {
            var room = new Room
            {
                RoomNumber = "101",
                RoomType = RoomType.Deluxe,
                Capacity = 6,
                Hotel = new Hotel
                {
                    Name = "Test Hotel",
                    Address = "London"
                }
            };

            context.Rooms.Add(room);
            return room;
        }

        private static DateOnly FutureDate(int days)
        {
            return DateOnly.FromDateTime(DateTime.UtcNow.AddDays(days));
        }
    }
}
