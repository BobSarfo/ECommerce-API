using HubtelCommerce.Dtos;
using HubtelCommerce.Dtos.Carts;

namespace HubtelCommerce.Interfaces
{
    public interface ICartService
    {
        Task<Response> AddItemToCart(CartItemDto cartItemDto, string phoneNumber);
        Task<CartItemResponse> GetCartItemByItemId(int itemId, string phoneNumber);
        Task<Response> RemoveCartItem(string phoneNumber, int cartItemId);
        Task<Response> RemoveMultipleCartItems(string phoneNumber, List<int> cartItemIds);
        Task<SearchCartItemsResponse> SearchCartItems(CartSearchFilterDto cartSearchDto);
    }
}