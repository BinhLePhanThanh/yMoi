using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace yMoi.Model
{
    public class ServiceFile : BaseModel
    {
        public int ServiceId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(500)]
        public string Name { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(500)]
        public string Url { get; set; }

        public virtual Service Service { get; set; }
    }
}