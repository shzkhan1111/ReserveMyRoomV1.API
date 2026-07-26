namespace ReserveMyRoom.API.Repository.Services;

internal static class StayValidator
{
    public static void Validate(
        DateOnly checkInDate,
        DateOnly checkOutDate,
        int numberOfGuests)
    {
        if (checkInDate == default || checkOutDate == default)
        {
            throw new ArgumentException(
                "Check-in and check-out dates are required.");
        }

        if (checkOutDate <= checkInDate)
        {
            throw new ArgumentException(
                "Check-out date must be after check-in date.");
        }

        if (numberOfGuests < 1)
        {
            throw new ArgumentException(
                "Number of guests must be at least 1.");
        }

        if (checkInDate < DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new ArgumentException(
                "Check-in date cannot be in the past.");
        }
    }
}
