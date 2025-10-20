using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace yMoi.Model
{
    public class Medicine : BaseModel
    {
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? Name { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string Code { get; set; }
        public bool Status { get; set; }
        public int? UnitId { get; set; }

        public long? ImportPrice { get; set; }
        public int? ImportPriceUnitId { get; set; }

        public long? OfficialPrice { get; set; }
        public int? OfficialPriceUnitId { get; set; }
        public long? Quantity { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? Supplier { get; set; }


        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? PackingSpecification { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? Manufacturer { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(1000)]
        public string? Note { get; set; }

        public int CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual Unit OfficialPriceUnit { get; set; }
        public virtual Unit ImportPriceUnit { get; set; }
        public virtual ICollection<MedicineFile> MedicineFiles { get; set; }
    }
}