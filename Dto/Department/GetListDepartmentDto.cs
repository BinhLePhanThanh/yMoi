namespace yMoi.Dto.Department
{
    public class GetListDepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Note { get; set; }
        public bool Status { get; set; }
        public int CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}