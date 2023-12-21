namespace HubtelCommerce.Dtos
{
    public class Response
    {
        public bool Succeeded {  get; set; }
        public string Message { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
