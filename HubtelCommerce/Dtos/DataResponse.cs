namespace HubtelCommerce.Dtos
{
    public class DataResponse<T> : Response
    {
        public T Data { get; set; }
    }
}
