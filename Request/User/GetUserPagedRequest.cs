public class GetUserPagedRequest
{
    public Dictionary<string, object> QueryParams { get; set; } = new Dictionary<string, object>();
    public List<string> Roles { get; set; } = new List<string>();  
}