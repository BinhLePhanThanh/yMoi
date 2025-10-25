// Services/UpdateUserService.cs
using Microsoft.EntityFrameworkCore;

public class UserService
{
    private readonly ApplicationDbContext _context;
    private readonly List<IUserBehavior> _behaviors;

    public UserService(ApplicationDbContext context, IEnumerable<IUserBehavior> behaviors)
    {
        _context = context;
        _behaviors = behaviors.ToList();
    }

    public async Task HandleAsync(
        UpdateUserRequest request,
        int callerUserId,               // id của user đang gọi service
        IEnumerable<string> requestedRoles) // danh sách roles mà caller muốn dùng
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
        CreateUserRequest request,
        int callerUserId,               // id của user đang gọi service
        IEnumerable<string> requestedRoles) // danh sách roles mà caller muốn dùng
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
    public async Task<PagedResult<User>> HandleGetPagedAsync(
        Dictionary<string, object> queryParams,
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

        // 🔹 4. Giả sử chỉ có 1 behavior hỗ trợ GetPaged
        var behavior = matchedBehaviors.First();
        return await behavior.ExecuteGetPagedAsync(queryParams);
    }
    public async Task<User> HandleGetByIdAsync(
        int id,
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

        // 🔹 4. Giả sử chỉ có 1 behavior hỗ trợ GetById
        var behavior = matchedBehaviors.First();
        return await behavior.ExecuteGetByIdAsync(id);
    }
    public async Task HandleDeleteAsync(
        int id,
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
        // 🔹 4. Giả sử chỉ có 1 behavior hỗ trợ Delete
        var behavior = matchedBehaviors.First();
        await behavior.ExecuteDeleteAsync(id);
    }

    public async Task<GetUserDetailsModel> GetUserDetails(int id)
    {
        var user = await _context.Users.Where(a => a.Id == id)
            .Select(a => new GetUserDetailsModel
            {
                Id = a.Id,
                Name = a.Name,
                Email = a.Email,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt,
                IsActive = a.IsActive,
                Username = a.Username,
                JobTitle = a.JobTitle,
                Department = a.Department,
                PhoneNumber = a.PhoneNumber,
                IdentificationNumber = a.IdentificationNumber,
                DateOfBirth = a.DateOfBirth,
                Gender = a.Gender,
                MarriageStatus = a.MarriageStatus,
                Language = a.Language,
                EducationLevel = a.EducationLevel,
                Regilion = a.Regilion,
                Country = a.Country,
                Notes = a.Notes,
                UserRoles = a.UserRoles.Select(n => new GetUserDetailsRolesModel
                {
                    RoleId = n.RoleId,
                    RoleName = n.Role.Name
                }).ToList(),
                CreatedById = a.CreatedById,
                CreatedByName = a.CreatedBy.Name
            })
            .FirstOrDefaultAsync();

        return user;
    }
}
