namespace VietWay.API.Customer.ResponseModel
{
    public class DefaultResponseModel<T> where T : class
    {
        public int StatusCode { get; set; }
        public required string Message { get; set; }
        public T? Data { get; set; }
    }
}
