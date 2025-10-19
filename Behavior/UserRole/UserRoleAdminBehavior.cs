public class UserRoleAdminBehavior : IUserRoleBehavior
{
    private readonly ApplicationDbContext _context;

    public UserRoleAdminBehavior(ApplicationDbContext context)
    {
        _context = context;
    }

    public string RoleName => "Admin";

    public async Task ExecuteAsync(UpdateUserRoleRequest request)
    {
        var userRole = await _context.UserRoles.FindAsync(request.Id);
        if (userRole == null)
        {
            throw new Exception("UserRole not found");
        }

        userRole.UserId = request.UserId;
        userRole.RoleId = request.RoleId;

        await _context.SaveChangesAsync();
    }

    public async Task ExecuteCreateAsync(CreateUserRoleRequest request)
    {
        var userRole = new UserRole
        {
            UserId = request.UserId,
            RoleId = request.RoleId
        };

        _context.UserRoles.Add(userRole);
        await _context.SaveChangesAsync();
    }
}