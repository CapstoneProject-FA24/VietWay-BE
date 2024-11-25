namespace VietWay.API.Customer.ResponseModel
{
    public class DefaultResponseModel<T>
    {
        public int StatusCode { get; set; }
        public required string Message { get; set; }
        public T? Data { get; set; }
    }
}
