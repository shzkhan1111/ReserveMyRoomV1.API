namespace ReserveMyRoom.API.Repository.Interface
{
    public interface IDataService
    {
        Task ResetDatabaseAsync(CancellationToken cancellationToken = default);
        Task SeedDatabaseAsync(CancellationToken cancellationToken = default);
    }
}
