namespace yMoi.Dto.Customer
{
    public class CreateCustomerModel
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Phone { get; set; }
        public DateTime? Dob { get; set; }
        public string? Email { get; set; }
        public string? Job { get; set; }
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? WardId { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? Language { get; set; }
        public string? Religion { get; set; }
        public string? Nationality { get; set; }
        public string? IdentityCardNumber { get; set; }
        public string? EducationalLevel { get; set; }
        public string? MaritalStatus { get; set; }
        public int? Gender { get; set; }

        #region 
        public string? RelativeName { get; set; }
        public string? Relationship { get; set; }
        public string? RelationshipAddress { get; set; }
        public int? RelativeProvinceId { get; set; }
        public int? RelativeDistrictId { get; set; }
        public int? RelativeWardId { get; set; }

        public string? RelativeCountry { get; set; }
        public string? RelativePostalCode { get; set; }
        public string? RelativePhone { get; set; }
        #endregion



        #region
        public string? BankCode { get; set; }
        public string? AccountNumber { get; set; }
        public string? AccountHolder { get; set; }
        #endregion

        public int? CustomerGroupId { get; set; }
    }
}
