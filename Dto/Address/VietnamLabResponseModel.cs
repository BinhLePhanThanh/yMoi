namespace yMoi.Dto.Address
{
    public class VietnamLabResponseModel
    {
        public bool success { get; set; }
        public List<VietnamLabProvinceResponseModel> data { get; set; }
    }

    public class VietnamLabProvinceResponseModel
    {
        public string id { get; set; }
        public string province { get; set; }
        public List<VietnamLabWardResponseModel> wards { get; set; }
    }

    public class VietnamLabWardResponseModel
    {
        public string name { get; set; }
    }
}