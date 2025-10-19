
using Microsoft.EntityFrameworkCore;

public class UserRoleService
{
    private readonly ApplicationDbContext _context;
    private readonly IEnumerable<IUserRoleBehavior> _behaviors;

    public UserRoleService(ApplicationDbContext context, IEnumerable<IUserRoleBehavior> behaviors)
    {
        _context = context;
        _behaviors = behaviors;
    }

    public async Task HandleAsync(
        UpdateUserRoleRequest request,
        int callerUserId,
        IEnumerable<string> requestedRoles)
    {
        // 🔹 1. Lấy danh sách role thực tế của caller
        var callerRoles = await _context.UserRoles
            .Where(ur => ur.UserId == callerUserId)
            .Select(ur => ur.Role.Name)
            .ToListAsync();

        // 🔹 2. Kiểm tra caller có đủ role được yêu cầu không
        bool hasAll = requestedRoles.All(r => callerRoles.Contains(r));

        if (!hasAll)
        {
            Console.WriteLine("Caller does not have all the required roles.");
            return;
        }

        // 🔹 3. Resolve behavior theo requested roles
        var matchedBehaviors = _behaviors
            .Where(b => requestedRoles.Contains(b.RoleName))
            .ToList();

        if (!matchedBehaviors.Any())
        {
            Console.WriteLine("No matching behavior found for requested roles.");
            return;
        }

        // 🔹 4. Thực thi từng behavior tương ứng
        foreach (var behavior in matchedBehaviors)
        {
            await behavior.ExecuteAsync(request);
        }
    }
    public async Task HandleAsync(
        CreateUserRoleRequest request,
        int callerUserId,
        IEnumerable<string> requestedRoles)
    {
        // 🔹 1. Lấy danh sách role thực tế của caller
        var callerRoles = await _context.UserRoles
            .Where(ur => ur.UserId == callerUserId)
            .Select(ur => ur.Role.Name)
            .ToListAsync();

        // 🔹 2. Kiểm tra caller có đủ role được yêu cầu không
        bool hasAll = requestedRoles.All(r => callerRoles.Contains(r));

        if (!hasAll)
        {
            throw new Exception("Caller does not have all the required roles.");
        }

        // 🔹 3. Resolve behavior theo requested roles
        var matchedBehaviors = _behaviors
            .Where(b => requestedRoles.Contains(b.RoleName))
            .ToList();

        if (!matchedBehaviors.Any())
        {
            throw new Exception("No matching behavior found for requested roles.");
        }

        // 🔹 4. Thực thi từng behavior tương ứng
        foreach (var behavior in matchedBehaviors)
        {
            await behavior.ExecuteCreateAsync(request);
        }
    }


}