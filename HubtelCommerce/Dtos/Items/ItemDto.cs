namespace HubtelCommerce.Dtos.Items
{
    public class ItemDto
    {
        public string Brand { get; set; }
        public string UOM { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; } = -1;
        public string Description { get; set; }
    }
}
