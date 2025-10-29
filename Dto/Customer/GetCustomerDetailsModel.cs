namespace yMoi.Dto.Customer
{
    public class GetCustomerDetailsModel : CreateCustomerModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
