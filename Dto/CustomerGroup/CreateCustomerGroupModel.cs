using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace yMoi.Dto.CustomerGroup
{
    public class CreateCustomerGroupModel
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? BankCode { get; set; }
        public string? AccountNumber { get; set; }
        public string? AccountHolder { get; set; }
    }
}
