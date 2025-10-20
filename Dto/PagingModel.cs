namespace yMoi.Dto
{
    public class PagingModel
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public int TotalItemCount { get; set; }
        public object Extras { get; set; } = null;
    }
}