namespace HubtelCommerce.Dtos
{
    public class PagedDataResponse<T> : DataResponse<T>
    {
        public int PageNumber { get; set; } = 0;
        public int PageSize { get; set; } = 20;
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }

        public PagedDataResponse(T data, int pageNumber, int pageSize) : base()
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Message = string.Empty;
            Succeeded = true;
            base.Data = data;
        }
        public PagedDataResponse()
        {
        }
    }
}
