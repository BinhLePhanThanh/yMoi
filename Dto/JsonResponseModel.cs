namespace yMoi.Dto
{
    public class JsonResponseModel
    {
        public int status { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public object data { get; set; }
        public object paging { get; set; }
        public int error { get; set; } = 0;

        public JsonResponseModel(int status, int code, string message, object data, object? paging = null)
        {
            this.status = status;
            this.code = code;
            this.message = message;
            this.data = data;
            this.paging = paging;
        }

    }
}