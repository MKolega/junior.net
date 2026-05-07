using System.ComponentModel.DataAnnotations;

namespace AbySalto.Junior.Controllers.Requests
{
    public class CreateOrderItemRequest
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = null!;

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
