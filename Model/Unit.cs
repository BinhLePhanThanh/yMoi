using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace yMoi.Model
{
    public class Unit : BaseModel
    {
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string Name { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string Code { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(1000)]
        public string? Note { get; set; }
        public bool Status { get; set; }
        public int CreatedById { get; set; }

        public virtual User CreatedBy { get; set; }
    }
}