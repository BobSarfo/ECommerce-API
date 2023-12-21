using HubtelCommerce.Dtos;
using HubtelCommerce.Dtos.Items;
using HubtelCommerce.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HubtelCommerce.Controllers
{
    [Route("api/v1/items")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AllItemsResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response))]
        public async Task<IActionResult> GetAllItemsAsync()
        {
            var resp = await _itemService.GetAllItemsAsync();

            return Ok(resp);
        }


        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response))]
        public async Task<IActionResult> AddNewItemItemsAsync(ItemDto itemDto)
        {
            var resp = await _itemService.AddNewItemAsync(itemDto);
            if (!resp.Succeeded) 
            {
                return BadRequest(resp);
            }

            return Created("/",resp);
        }

        [HttpDelete("{itemId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response))]
        public async Task<IActionResult> AddNewItemAsync(int itemId)
        {

            var resp = await _itemService.RemoveItemAsync(itemId);
            if (!resp.Succeeded)
            {
                return NotFound(resp);
            }

            return Ok(resp);
        }
    }
}
