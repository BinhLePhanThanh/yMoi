public interface IUserRoleBehavior
{
    string RoleName { get; }
    Task ExecuteAsync(UpdateUserRoleRequest request);
    Task ExecuteCreateAsync(CreateUserRoleRequest request);
}