using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace yMoi.Dto.CustomerGroup
{
    public class GetListCustomerGroupModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? BankCode { get; set; }
        public string? AccountNumber { get; set; }
        public string? AccountHolder { get; set; }
        public bool Status { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public int CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
