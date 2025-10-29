using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace yMoi.Model
{
    public class Customer : BaseModel
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
        public DateTime? Dob { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? Email { get; set; }
        public bool Status { get; set; } = true;

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? Job { get; set; }
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? WardId { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? Address { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? PostalCode { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? Country { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? Language { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? Religion { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? Nationality { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? IdentityCardNumber { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? EducationalLevel { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? MaritalStatus { get; set; }
        public int? Gender { get; set; }

        #region 
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? RelativeName { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? Relationship { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? RelationshipAddress { get; set; }
        public int? RelativeProvinceId { get; set; }
        public int? RelativeDistrictId { get; set; }
        public int? RelativeWardId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? RelativeCountry { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? RelativePostalCode { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? RelativePhone { get; set; }
        #endregion



        #region
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? BankCode { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? AccountNumber { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string? AccountHolder { get; set; }
        #endregion

        public int? CustomerGroupId { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        public int? CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual CustomerGroup CustomerGroup { get; set; }

        public Customer DeepCopy()
        {
            Customer other = (Customer)this.MemberwiseClone();
            return other;
        }
    }
}
