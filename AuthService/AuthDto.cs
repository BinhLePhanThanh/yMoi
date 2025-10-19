public class LoginRequestDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
public class LoginResponseDto
{
    public int EmployeeId { get; set; }
    public string Token { get; set; }
    public List<string> Roles { get; set; }
}
