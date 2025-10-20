namespace yMoi.Dto.Service
{
    public class GetServiceDetailsModel : CreateServiceDto
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedById { get; set; }
        public string CreatedByName { get; set; }
    }
}
