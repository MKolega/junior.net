namespace AbySalto.Junior.Models
{
    public class Order
    {
        public int Id { get; set; }
        
         public required string CustomerName { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.NaCekanju;

        public required string PaymentMethod { get; set; }

        public required string Address { get; set; }

        public required string PhoneNumber { get; set; }

        public string? Notes { get; set; }

        public DateTime OrderDate { get; set; }

        public required string Currency { get; set; }

        public decimal TotalAmount { get; set; }

        public List<OrderItem> Items { get; set; } = new List<OrderItem>();





    }
}