namespace VietWay.API.Customer.RequestModel
{
    public class ChatRequest
    {
        public bool IsUser { get; set; }
        public required string Text { get; set; }
    }
}
