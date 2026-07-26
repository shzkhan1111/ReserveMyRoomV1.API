using Microsoft.EntityFrameworkCore;
using ReserveMyRoom.API.Models;

namespace ReserveMyRoom.API.Data
{
    public class ReserveMyRoomDbContext : DbContext
    {
        public ReserveMyRoomDbContext(DbContextOptions<ReserveMyRoomDbContext> options)
            : base(options)
        {}

        public DbSet<Hotel> Hotels { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Hotel>()
                .Property(hotel => hotel.Name)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<Hotel>()
                .Property(hotel => hotel.Address)
                .IsRequired()
                .HasMaxLength(250);

            modelBuilder.Entity<Room>()
                .Property(room => room.RoomNumber)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Room>()
                .HasIndex(room => new
                {
                    room.HotelId,
                    room.RoomNumber
                })
                .IsUnique();

            modelBuilder.Entity<Booking>()
                .Property(booking => booking.BookingReference)
                .IsRequired()
                .HasMaxLength(40);

            modelBuilder.Entity<Booking>()
                .HasIndex(booking => booking.BookingReference)
                .IsUnique();

            modelBuilder.Entity<Booking>()
                .HasIndex(booking => new
                {
                    booking.RoomId,
                    booking.CheckInDate,
                    booking.CheckOutDate
                });

            modelBuilder.Entity<Booking>()
                .Property(booking => booking.GuestName)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<Booking>()
                .ToTable(table => table.HasCheckConstraint(
                    "CK_Bookings_NumberOfGuests",
                    "[NumberOfGuests] BETWEEN 1 AND 4"));

            modelBuilder.Entity<Booking>()
                .ToTable(table => table.HasCheckConstraint(
                    "CK_Bookings_DateRange",
                    "[CheckOutDate] > [CheckInDate]"));

            modelBuilder.Entity<Hotel>()
                .HasMany(hotel => hotel.Rooms)
                .WithOne(room => room.Hotel)
                .HasForeignKey(room => room.HotelId);

            modelBuilder.Entity<Room>()
                .HasMany(room => room.Bookings)
                .WithOne(booking => booking.Room)
                .HasForeignKey(booking => booking.RoomId);

        }

    }
}
