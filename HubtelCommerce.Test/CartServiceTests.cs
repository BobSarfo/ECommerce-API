using HubtelCommerce.DAL;
using HubtelCommerce.Dtos.Carts;
using HubtelCommerce.Services;
using HubtelCommerce.Utils;
using Microsoft.EntityFrameworkCore;

namespace HubtelCommerce.Test
{

    public class CartServiceTests
    {
        private readonly CommerceDbContext context;

        public CartServiceTests()
        {
            var options = new DbContextOptionsBuilder<CommerceDbContext>()
                 .UseInMemoryDatabase(databaseName: "HubtelCommerceDb")
                 .Options;

            context = new CommerceDbContext(options);
        }

        [Fact]
        public async Task SearchCartItems_WithValidInput_ReturnsSuccessResponse()
        {

            var cartSearchDto = new CartSearchFilterDto
            {
                PhoneNumbers = new List<string> { "123456789" },
                ItemName = "ExampleItem",
                Date = DateTime.Now,
                QuantityOperand = MathOperandContants.LessThan,
                Quantity = 10
            };

            context.Carts.AddRange(new List<Cart>
            {
                new Cart
                {
                    PhoneNumber = "123456789",
                    ItemName = "ExampleItem",
                    DateUpdated = DateTime.Now,
                    Quantity = 5
                }
            });

            context.SaveChanges();

            var service = new CartService(context);


            var result = await service.SearchCartItems(cartSearchDto);

            Assert.NotNull(result);
            Assert.Equal("Success", result.Status);
            Assert.True(result.Succeeded);
            Assert.NotNull(result.Data);
        }
    }
}