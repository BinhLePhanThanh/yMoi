namespace yMoi.Dto.Medicine
{
    public class GetMedicineDetailsModel : CreateMedicineDto
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedById { get; set; }
        public string CreatedByName { get; set; }
    }
}
