using HubtelCommerce.DAL;
using HubtelCommerce.Dtos;
using HubtelCommerce.Dtos.Items;
using HubtelCommerce.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HubtelCommerce.Services
{
    public class ItemService : IItemService
    {
        private readonly CommerceDbContext _db;

        public ItemService(CommerceDbContext db)
        {
            _db = db;
        }


        public async Task<AllItemsResponse> GetAllItemsAsync()
        {
            var resp = new AllItemsResponse { Status ="Success", Succeeded=true };

            var allItems = await _db.Items.ToListAsync();

            resp.Data = allItems;

            return resp;                        
        }


        public async Task<Response> AddNewItemAsync(ItemDto itemDto)
        {
            var resp = new Response { };

            if (itemDto.UnitPrice is -1 || itemDto.Name is null)
            {
                resp.Status = "Invalid Input";
                resp.Message = "Invalid Submission please enter values for Item name and unit price";
                resp.Succeeded = false;
                return resp;
            }

            var newItem = new Item
            {
                Brand = itemDto.Brand,
                UOM = itemDto.UOM,
                Name = itemDto.Name,
                UnitPrice = itemDto.UnitPrice,
                Description = itemDto.Description,
            };

            _db.Items.Add(newItem);
            await _db.SaveChangesAsync();

            resp.Status = "Success";
            resp.Message = $"New Item {itemDto.Name} added successfully";
            resp.Succeeded = true;
            return resp;

        }

        public async Task<Response> RemoveItemAsync(int itemId)
        {
            var resp = new Response { };
            var foundItem = await _db.Items.FirstOrDefaultAsync(item => item.Id == itemId);

            if (foundItem is null)
            {
                resp.Status = "Not Found";
                resp.Message = "Unable to delete item. Item not found";
                resp.Succeeded = false;
                return resp;
            }

            _db.Items.Remove(foundItem);    
            await _db.SaveChangesAsync();   

            resp.Status = "Success";
            resp.Message = $"Item {foundItem.Name} deleted successfully";
            resp.Succeeded = true;
            return resp;
        }

    }
}
