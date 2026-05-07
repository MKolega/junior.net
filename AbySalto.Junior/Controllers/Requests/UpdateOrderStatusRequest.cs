using System.ComponentModel.DataAnnotations;
using AbySalto.Junior.Models;

namespace AbySalto.Junior.Controllers.Requests
{
    public class UpdateOrderStatusRequest
    {
        [Required]
        [EnumDataType(typeof(OrderStatus))]
        public OrderStatus Status { get; set; }
    }
}
