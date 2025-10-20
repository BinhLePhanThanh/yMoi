namespace yMoi.Dto.Department
{
    public class CreateDepartmentDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Note { get; set; }
        public bool Status { get; set; }
    }
}