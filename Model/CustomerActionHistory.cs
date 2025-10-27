using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace yMoi.Model
{
    public class CustomerActionHistory : BaseModel
    {
        public int CustomerActionId { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string Field { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? FieldType { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? FromValue { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? ToValue { get; set; }

        public virtual CustomerAction CustomerAction { get; set; }
    }
}
