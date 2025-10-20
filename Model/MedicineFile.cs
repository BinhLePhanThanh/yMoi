using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace yMoi.Model
{
    public class MedicineFile : BaseModel
    {
        public int MedicineId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(500)]
        public string Name { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(500)]
        public string Url { get; set; }

        public virtual Medicine Medicine { get; set; }
    }
}