namespace yMoi.Dto.Unit
{
    public class CreateUnitDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Note { get; set; }
        public bool Status { get; set; }
    }
}