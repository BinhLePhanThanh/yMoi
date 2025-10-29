using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace yMoi.Model
{
    public class WardV2 : BaseModel
    {
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string Name { get; set; }

        public int? ProvinceV2Id { get; set; }

        public virtual ProvinceV2 ProvinceV2 { get; set; }
    }
}