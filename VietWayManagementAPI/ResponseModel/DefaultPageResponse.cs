namespace VietWay.API.Management.ResponseModel
{
    public class DefaultPageResponse<T>
    {
        public int Total { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public List<T> Items { get; set; }
    }
}