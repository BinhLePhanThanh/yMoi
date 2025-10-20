using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace yMoi.Dto.Service
{
    public class CreateServiceDto
    {
        public string? Name { get; set; }
        public string Code { get; set; }
        public string? Note { get; set; }
        public bool Status { get; set; }
        public int? UnitId { get; set; }
        public string? Type { get; set; }
        public int? DepartmentId { get; set; }
        public long? ImportPrice { get; set; }
        public int? ImportPriceUnitId { get; set; }
        public long? OfficialPrice { get; set; }
        public int? OfficialPriceUnitId { get; set; }
        public string? ReferenceLimit1 { get; set; }
        public string? ReferenceLimit2 { get; set; }
        public string? TurnaroundTime { get; set; }
        public string? ToolFeatures { get; set; }
        public List<CreateServiceFileDto> Files { get; set; }
    }

    public class CreateServiceFileDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
