using HubtelCommerce.DAL;
using HubtelCommerce.Dtos;
using HubtelCommerce.Dtos.Carts;
using HubtelCommerce.Interfaces;
using HubtelCommerce.Utils;
using Microsoft.EntityFrameworkCore;

namespace HubtelCommerce.Services
{
    public class CartService : ICartService
    {
        private readonly CommerceDbContext _db;

        public CartService(CommerceDbContext db)
        {
            _db = db;
        }

        public async Task<CartItemResponse> GetCartItemByItemId(int itemId, string phoneNumber)
        {
            var resp = new CartItemResponse { };

            Cart? foundCartItem = await _db.Carts.FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber && c.ItemId == itemId);
            if (foundCartItem is null)
            {
                resp.Succeeded = false;
                resp.Message = "Cart Item not found";
                resp.Status = "Not Found";
                return resp;
            }

            resp.Message = "";
            resp.Succeeded = false;
            resp.Status = "Success";
            resp.Data = foundCartItem;

            return resp;
        }

        public async Task<SearchCartItemsResponse> SearchCartItems(CartSearchFilterDto cartSearchDto)
        {
            var resp = new SearchCartItemsResponse { Status = "Error", Succeeded = false };
            var query = _db.Carts.AsQueryable();

            if (cartSearchDto.PhoneNumbers?.Any() == true)
            {
                query = query.Where(cart => cartSearchDto.PhoneNumbers.Contains(cart.PhoneNumber));
            }

            if (!string.IsNullOrEmpty(cartSearchDto.ItemName))
            {
                query = query.Where(cart => cart.ItemName.ToLower().Contains(cartSearchDto.ItemName));
            }

            if (cartSearchDto.Date != default)
            {
                query = query.Where(cart => cart.DateUpdated == cartSearchDto.Date);
            }

            if (!string.IsNullOrEmpty(cartSearchDto.QuantityOperand) && MathOperandContants.All.Contains(cartSearchDto.QuantityOperand))
            {
                if (cartSearchDto.Quantity == -1)
                {
                    resp.Message = "Please provide valid input for quantity";
                    return resp;
                }

                if ((cartSearchDto.QuantityOperand == MathOperandContants.BetweenInclusive ||
                     cartSearchDto.QuantityOperand == MathOperandContants.BetweenExclusive) && cartSearchDto.QuantityTo == -1)
                {
                    resp.Message = "Please provide quantity for quantityTo";
                    return resp;
                }

                if (!MathOperandContants.All.Contains(cartSearchDto.QuantityOperand))
                {
                    resp.Message = $"Please provide a valid operator: {MathOperandContants.ToStringList()}";
                    return resp;
                }

                query = cartSearchDto.QuantityOperand switch
                {
                    MathOperandContants.LessThan => query.Where(cart => cart.Quantity < cartSearchDto.Quantity),
                    MathOperandContants.LessThanOrEqual => query.Where(cart => cart.Quantity <= cartSearchDto.Quantity),
                    MathOperandContants.GreaterThan => query.Where(cart => cart.Quantity > cartSearchDto.Quantity),
                    MathOperandContants.GreaterThanOrEqual => query.Where(cart => cart.Quantity >= cartSearchDto.Quantity),
                    MathOperandContants.NotEqual => query.Where(cart => cart.Quantity != cartSearchDto.Quantity),
                    MathOperandContants.Equal => query.Where(cart => cart.Quantity == cartSearchDto.Quantity),
                    MathOperandContants.BetweenInclusive => query.Where(cart => cart.Quantity >= cartSearchDto.Quantity && cart.Quantity <= cartSearchDto.QuantityTo),
                    MathOperandContants.BetweenExclusive => query.Where(cart => cart.Quantity > cartSearchDto.Quantity && cart.Quantity < cartSearchDto.QuantityTo),
                    _ => query
                };
            }

            var queryResult = await query.OrderByDescending(cart=>cart.DateUpdated).ToListAsync();

            resp.Data = queryResult;
            resp.Status = "Success";
            resp.Succeeded = true;
            return resp;
        }

        public async Task<Response> AddItemToCart(CartItemDto cartItemDto, string phoneNumber)
        {
            var res = new Response { };
            var cartItem = await _db.Carts.FirstOrDefaultAsync(cart => cart.PhoneNumber == phoneNumber && cart.ItemId == cartItemDto.ItemId);

            if (cartItem is null)
            {
                var item = await _db.Items.FirstOrDefaultAsync(item => item.Id == cartItemDto.ItemId);
                if (item is null)
                {
                    res.Succeeded = false;
                    res.Message = "Cart item not found or does not exist";
                    return res;
                }

                cartItem = new Cart
                {
                    ItemId = cartItemDto.ItemId,
                    PhoneNumber = phoneNumber,
                    Quantity = cartItemDto.Quantity,
                    ItemName = item.Name,
                    UnitPrice = item.UnitPrice,
                };

                _db.Carts.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += cartItemDto.Quantity;
                _db.Carts.Update(cartItem);
            }

            await _db.SaveChangesAsync();

            res.Succeeded = true;
            res.Status = "Success";
            res.Message = "Cart successfully updated";

            return res;
        }

        public async Task<Response> RemoveCartItem(string phoneNumber, int cartItemId)
        {
            var resp = new Response { };
            Cart? foundCartItem = await _db.Carts.FirstOrDefaultAsync(cart => cart.PhoneNumber == phoneNumber && cart.ItemId == cartItemId);

            if (foundCartItem is null)
            {
                resp.Succeeded = false;
                resp.Status = "Error";
                resp.Message = "Item not found in cart. Unable to delete. Please try again";
                return resp;
            }

            _db.Carts.Remove(foundCartItem);
            await _db.SaveChangesAsync();

            resp.Succeeded = true;
            resp.Message = $"{foundCartItem} has been successfully deleted";
            return resp;
        }


        public async Task<Response> RemoveMultipleCartItems(string phoneNumber, List<int> cartItemIds)
        {
            var resp = new Response { };

            List<Cart>? foundCartItems = await _db.Carts.Where(cart => cart.PhoneNumber == phoneNumber && cartItemIds.Contains(cart.ItemId)).ToListAsync();

            if (foundCartItems is null || foundCartItems.Count == 0)
            {
                resp.Succeeded = false;
                resp.Status = "Error";
                resp.Message = "Unable to delete any cart. Please try again";
                return resp;
            }

            _db.Carts.RemoveRange(foundCartItems);
            await _db.SaveChangesAsync();

            resp.Status = "Success";
            resp.Succeeded = true;
            resp.Message = $"{foundCartItems.Count} cart items has been successfully deleted";
            return resp;
        }
    }
}
