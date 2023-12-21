using HubtelCommerce.Dtos;
using HubtelCommerce.Dtos.Items;

namespace HubtelCommerce.Interfaces
{
    public interface IItemService
    {
        Task<AllItemsResponse> GetAllItemsAsync();
        Task<Response> AddNewItemAsync(ItemDto itemDto);
        Task<Response> RemoveItemAsync(int itemId);
    }
}