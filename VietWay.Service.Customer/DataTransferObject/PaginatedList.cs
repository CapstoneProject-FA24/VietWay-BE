namespace VietWay.Service.Customer.DataTransferObject
{

    public class PaginatedList<T>
    {
        public int Total { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public List<T> Items { get; set; }
    }
}