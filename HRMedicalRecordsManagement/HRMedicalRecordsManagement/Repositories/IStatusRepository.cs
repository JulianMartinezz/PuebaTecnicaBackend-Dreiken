public interface IStatusRepository
{
    Task<bool> StatusExistsAsync(int statusId);
}