using HubtelCommerce.DAL;
using HubtelCommerce.Dtos;
using HubtelCommerce.Dtos.Carts;
using HubtelCommerce.Interfaces;
using HubtelCommerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HubtelCommerce.Controllers
{
    [Route("api/v1/cart")]
    [ApiController]
    [Authorize]
   // [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddItemToCart(CartItemDto cartItemDto)
        {
            var loggedInUserPhone = User.Identity?.Name;

            var resp = await _cartService.AddItemToCart(cartItemDto, loggedInUserPhone);
            if (!resp.Succeeded) 
            {
                return BadRequest(resp);
            }

            return Ok(resp);    
        }

   
        [HttpDelete("{itemId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes. Status404NotFound, Type = typeof(Response))]
        public async Task<IActionResult> RemoveCartItemAsync(int itemId)
        {
            var loggedInUserPhoneNumber = User.Identity?.Name;

            var resp = await _cartService.RemoveCartItem(loggedInUserPhoneNumber, itemId);
            if (!resp.Succeeded) 
            {
                return NotFound(resp);
            }

            return Ok(resp);    
        }


        [HttpPost("global/search")] 
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchCartItemsResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> SearchAllCartItemsAsync([FromBody] CartSearchFilterDto cartSearchFilterDto)
        {
            var resp = await _cartService.SearchCartItems(cartSearchFilterDto);

            return Ok(resp);
        }
    }
}
