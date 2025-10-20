namespace yMoi.Dto.Medicine
{
    public class GetListMedicine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? UnitId { get; set; }
        public string UnitName { get; set; }
        public bool Status { get; set; }
        public int CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public long? Quantity { get; set; }
    }
}
