namespace HubtelCommerce.Dtos.Carts
{
    public class CartSearchFilterDto
    {
        public List<string>? PhoneNumbers { get; set; }
        public DateTime Date { get; set; }
        public string? QuantityOperand { get; set; }
        public int QuantityTo { get; set; } = -1;
        public int Quantity { get; set; } = -1;
        public string? ItemName { get; set; }

    }
}
