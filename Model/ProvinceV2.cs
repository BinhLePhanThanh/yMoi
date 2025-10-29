using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace yMoi.Model
{
    public class ProvinceV2 : BaseModel
    {
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string Code { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string Name { get; set; }
    }
}