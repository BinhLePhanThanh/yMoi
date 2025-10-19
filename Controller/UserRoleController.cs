using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserRoleController : ControllerBase
{
    private readonly UserRoleService _userRoleService;
    public UserRoleController(UserRoleService userRoleService)
    {
        _userRoleService = userRoleService;
    }
    /// <summary>
    /// Cập nhật thông tin user role, hành vi phụ thuộc vào role của caller
    /// </summary>
    [HttpPost("update")]
    public async Task<IActionResult> UpdateUserRole([FromBody] UpdateUserRoleRequest request, [FromQuery] List<string> roles)
    {
        // 🔹 Lấy callerUserId từ Claim
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("Invalid token: missing NameIdentifier claim.");
        }
        int callerUserId = int.Parse(userIdClaim.Value);
        // 🔹 Gọi service
        await _userRoleService.HandleAsync(request, callerUserId, roles);
        return Ok(new { Message = "Update request handled successfully." }); 
    }
    [HttpPost("create")]
    public async Task<IActionResult> CreateUserRole([FromBody] CreateUserRoleRequest request, [FromQuery] List<string> roles)
    {
        // 🔹 Lấy callerUserId từ Claim
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("Invalid token: missing NameIdentifier claim.");
        }
        int callerUserId = int.Parse(userIdClaim.Value);
        // 🔹 Gọi service
        await _userRoleService.HandleAsync(request, callerUserId, roles);
        return Ok(new { Message = "Create request handled successfully." });
    }
}