using System.ComponentModel.DataAnnotations;

namespace HubtelCommerce.DAL
{
    public class Item : BaseEntity
    {
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(1024)]
        public string Description { get; set; }

        [MaxLength(6)]
        public string UOM {  get; set; }

        [MaxLength(128)]
        public string Brand {  get; set; }

        [Required]
        public decimal UnitPrice { get; set; }
    }
}
