using System.ComponentModel.DataAnnotations;

namespace HubtelCommerce.DAL
{
    public class Cart : BaseEntity
    {
        [DataType(DataType.PhoneNumber)]
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(128)]
        public string ItemName { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
