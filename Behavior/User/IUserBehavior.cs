public interface IUserBehavior
{
    string RoleName { get; }
    Task ExecuteAsync(UpdateUserRequest request);
    Task ExecuteCreateAsync(CreateUserRequest request);
    Task<PagedResult<User>> ExecuteGetPagedAsync(Dictionary<string, object> queryParams);
    Task<User> ExecuteGetByIdAsync(int id);
    Task ExecuteDeleteAsync(int id);
    
}