using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly IAuthService _authService;

    public UserController(UserService userService, IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request);
        if (result == null)
            return Unauthorized(new { message = "Invalid username or password" });

        return Ok(result);
    }

    /// <summary>
    /// Cập nhật thông tin user, hành vi phụ thuộc vào role của caller
    /// </summary>
    [HttpPost("update")]
    [Authorize] // bắt buộc phải có JWT token
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request, [FromQuery] List<string> roles)
    {
        // 🔹 Lấy callerUserId từ Claim
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("Invalid token: missing NameIdentifier claim.");
        }

        int callerUserId = int.Parse(userIdClaim.Value);

        // 🔹 Gọi service
        await _userService.HandleAsync(request, callerUserId, roles);

        return Ok(new { Message = "Update request handled successfully." });
    }
    [HttpPost("create")]
    [Authorize] // bắt buộc phải có JWT token
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, [FromQuery] List<string> roles)
    {
        // 🔹 Lấy callerUserId từ Claim
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("Invalid token: missing NameIdentifier claim.");
        }
        int callerUserId = int.Parse(userIdClaim.Value);
        // 🔹 Gọi service
        request.CreatedById = callerUserId;
        await _userService.HandleAsync(request, callerUserId, roles);
        return Ok(new { Message = "Create request handled successfully." });
    }
    [HttpPost("paged")]
    [Authorize] // bắt buộc phải có JWT token
    public async Task<IActionResult> GetPaged(GetUserPagedRequest request)
    {
        // 🔹 Lấy callerUserId từ Claim
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("Invalid token: missing NameIdentifier claim.");
        }
        int callerUserId = int.Parse(userIdClaim.Value);
        // 🔹 Gọi service với role "Admin" để lấy danh sách user
        var result = await _userService.HandleGetPagedAsync(request.QueryParams, callerUserId, request.Roles);
        return result == null ? NotFound("No users found.") : Ok(result);
    }

    [HttpGet("details")]
    [Authorize] // bắt buộc phải có JWT token
    public async Task<IActionResult> GetDetails()
    {
        // 🔹 Lấy callerUserId từ Claim
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("Invalid token: missing NameIdentifier claim.");
        }
        int userId = int.Parse(userIdClaim.Value);

        var result = await _userService.GetUserDetails(userId);
        return result == null ? NotFound("User not found.") : Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize] // bắt buộc phải có JWT token
    public async Task<IActionResult> GetById(int id, string? roles)
    {
        // 🔹 Lấy callerUserId từ Claim
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("Invalid token: missing NameIdentifier claim.");
        }
        int callerUserId = int.Parse(userIdClaim.Value);

        var listRoles = !string.IsNullOrEmpty(roles) ? roles.Split(",").Select(a => a.Trim()) : new List<string>();

        var result = await _userService.HandleGetByIdAsync(id, callerUserId, listRoles);
        return result == null ? NotFound("User not found.") : Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize] // bắt buộc phải có JWT token
    public async Task<IActionResult> Delete(int id, [FromQuery] List<string> roles)
    {
        // 🔹 Lấy callerUserId từ Claim
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("Invalid token: missing NameIdentifier claim.");
        }
        int callerUserId = int.Parse(userIdClaim.Value);
        await _userService.HandleDeleteAsync(id, callerUserId, roles);
        return Ok(new { Message = "Delete request handled successfully." });
    }
}
