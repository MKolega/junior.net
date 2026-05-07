using System.ComponentModel.DataAnnotations;

namespace AbySalto.Junior.Controllers.Requests
{
    public class CreateOrderRequest
    {
        [Required]
        [StringLength(200)]
        public string CustomerName { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; } = null!;

        [Required]
        [StringLength(500)]
        public string Address { get; set; } = null!;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        public string? Notes { get; set; }

        [Required]
        [StringLength(10)]
        public string Currency { get; set; } = null!;

        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        public List<CreateOrderItemRequest>? Items { get; set; }
    }
}
