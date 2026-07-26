using Microsoft.EntityFrameworkCore;
using ReserveMyRoom.API.Data;
using ReserveMyRoom.API.Middleware;
using ReserveMyRoom.API.Repository.Interface;
using ReserveMyRoom.API.Repository.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

builder.Services.AddOpenApi();

builder.Services.AddDbContext<ReserveMyRoomDbContext>(options =>
{
    var connectionString =
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException(
            "Database connection is missing.");

    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddScoped<IBookingService, BookingService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<ReserveMyRoomDbContext>();

    await context.Database.MigrateAsync();
    DatabaseSeeder.Seed(context);
}

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "ReserveMyRoom API");
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
