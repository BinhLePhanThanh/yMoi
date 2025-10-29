namespace yMoi.Dto.Customer
{
    public class GetListCustomerModel
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int? Gender { get; set; }
        public DateTime? Dob { get; set; }
        public int? CreatedById { get; set; }
        public string? CreatedByName { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string MyProperty { get; set; }
        public string? IdentityCardNumber { get; set; }
        public string? BankCode { get; set; }
        public string? AccountNumber { get; set; }
        public string? AccountHolder { get; set; }
    }
}
