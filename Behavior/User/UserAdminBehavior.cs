// Behaviors/User/UserAdminBehavior.cs
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

public class UserAdminBehavior : IUserBehavior
{
    private readonly ApplicationDbContext _context;

    public UserAdminBehavior(ApplicationDbContext context)
    {
        _context = context;
    }

    public string RoleName => "Admin";

    public async Task ExecuteAsync(UpdateUserRequest request)
    {
        var user = await _context.Users.FindAsync(request.Id);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        // Cập nhật toàn bộ field (ngoại trừ CreatedAt, Id, Password nếu muốn giữ)
        _context.Entry(user).CurrentValues.SetValues(request);
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        Console.WriteLine($"[Admin] Updated user {user.Id} successfully.");
    }
    public async Task ExecuteCreateAsync(CreateUserRequest request)
    {
        if (await _context.Users.AnyAsync(u => u.Username == request.Username))
        {
            throw new Exception("Username already exists");
        }
        var newUser = new User();
        _context.Entry(newUser).CurrentValues.SetValues(request);
        newUser.CreatedAt = DateTime.UtcNow;
        newUser.UpdatedAt = DateTime.UtcNow;

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        Console.WriteLine($"[Admin] Created user {newUser.Id} successfully.");
    }
    public async Task<PagedResult<User>> ExecuteGetPagedAsync(Dictionary<string, object> filters)
    {
        int page = filters.TryGetValue("Page", out var pg) && pg is JsonElement pElem && pElem.TryGetInt32(out var pVal) ? pVal : 1;
        int pageSize = filters.TryGetValue("PageSize", out var ps) && ps is JsonElement sElem && sElem.TryGetInt32(out var sVal) ? sVal : 10;

        var query = await _context.Users
        .AsQueryable().ApplyDynamicFilter(filters);
        return await query.ToPagedResultAsync(page, pageSize);
    }
    public async Task<User> ExecuteGetByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        return user;
    }
    public async Task ExecuteDeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        Console.WriteLine($"[Admin] Deleted user {id} successfully.");
    }

}
