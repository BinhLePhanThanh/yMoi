using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace yMoi.Model
{
    public class Service : BaseModel
    {
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? Name { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string Code { get; set; }
        public bool Status { get; set; }
        public int? UnitId { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? Type { get; set; }
        public int? DepartmentId { get; set; }
        public long? ImportPrice { get; set; }
        public int? ImportPriceUnitId { get; set; }

        public long? OfficialPrice { get; set; }
        public int? OfficialPriceUnitId { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? ReferenceLimit1 { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? ReferenceLimit2 { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? TurnaroundTime { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? ToolFeatures { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(1000)]
        public string? Note { get; set; }

        public int CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual Department Department { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual Unit ImportPriceUnit { get; set; }
        public virtual Unit OfficialPriceUnit { get; set; }
        public virtual ICollection<ServiceFile> ServiceFiles { get; set; }
    }
}