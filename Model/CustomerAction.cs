using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace yMoi.Model
{
    public class CustomerAction : BaseModel
    {
        [Column(TypeName = "VARCHAR")]
        [StringLength(100)]
        public string Action { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int? CustomerGroupId { get; set; }
        public int? CustomerId { get; set; }
        public virtual User User { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual CustomerGroup CustomerGroup { get; set; }
        public virtual ICollection<CustomerActionHistory> CustomerActionHistories { get; set; }
    }
}
