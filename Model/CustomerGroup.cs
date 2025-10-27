using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace yMoi.Model
{
    public class CustomerGroup : BaseModel
    {
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? Name { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? Code { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? Phone { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? Address { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? BankCode { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? AccountNumber { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? AccountHolder { get; set; }

        public bool Status { get; set; } = true;


        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        public int CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }

        public CustomerGroup DeepCopy()
        {
            CustomerGroup other = (CustomerGroup)this.MemberwiseClone();
            return other;
        }
    }
}
