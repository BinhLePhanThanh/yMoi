namespace yMoi.Dto.Service
{
    public class GetListService
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string Type { get; set; }
    }
}
