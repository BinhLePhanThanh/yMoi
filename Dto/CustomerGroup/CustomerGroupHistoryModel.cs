namespace yMoi.Dto.CustomerGroup
{
    public class CustomerGroupHistoryModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Action { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<CustomerGroupHistoryDetails> Details { get; set; }
    }

    public class CustomerGroupHistoryDetails
    {
        public string Field { get; set; }
        public string FieldType { get; set; }
        public string? FromValue { get; set; }
        public string? ToValue { get; set; }

    }
}
