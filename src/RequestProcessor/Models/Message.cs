namespace RequestProcessor.Models
{
    public class Message
    {
        public string Payload { get; set; }
        public HeadersInput Headers { get; set; }
    }
}