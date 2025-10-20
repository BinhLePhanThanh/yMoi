using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace yMoi.Dto.Medicine
{
    public class CreateMedicineDto
    {
        public string? Name { get; set; }
        public string Code { get; set; }
        public bool Status { get; set; }
        public int? UnitId { get; set; }
        public long? ImportPrice { get; set; }
        public int? ImportPriceUnitId { get; set; }
        public long? OfficialPrice { get; set; }
        public int? OfficialPriceUnitId { get; set; }
        public long? Quantity { get; set; }
        public string? Supplier { get; set; }
        public string? PackingSpecification { get; set; }
        public string? Manufacturer { get; set; }
        public string? Note { get; set; }
        public List<CreateMedicineFileDto> Files { get; set; }
    }


    public class CreateMedicineFileDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
