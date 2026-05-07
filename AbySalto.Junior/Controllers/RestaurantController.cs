using AbySalto.Junior.Controllers.Requests;
using AbySalto.Junior.Infrastructure.Database;
using AbySalto.Junior.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AbySalto.Junior.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantController : ControllerBase
    {
        private readonly IApplicationDbContext _dbContext;

        public RestaurantController(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

       [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders(string? sort = null, CancellationToken cancellationToken = default)
        {
            var baseQuery = _dbContext.Orders
                .AsNoTracking()
                .Include(o => o.Items);


            
            var orderedQuery = sort?.ToLower() switch
            {
                "amount_asc" => baseQuery.OrderBy(o => o.TotalAmount),
                "amount_desc" => baseQuery.OrderByDescending(o => o.TotalAmount),
                _ => baseQuery.OrderByDescending(o => o.OrderDate)
            };

            var orders = await orderedQuery.ToListAsync(cancellationToken);

            return Ok(orders);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Order>> GetOrderById(int id, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders
                .AsNoTracking()
                .Include(o => o.Items)
                .FirstOrDefaultAsync(order => order.Id == id, cancellationToken);

            if (order is null)
            {
                return NotFound();
            }

            return Ok(order);
        }


        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
        {
            if(request?.Items == null || request.Items.Count == 0)
                return BadRequest("Order items are required.");
            
            var order = new Order
            {
                CustomerName = request.CustomerName,
                PaymentMethod = request.PaymentMethod,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                Notes = request.Notes,
                Currency = request.Currency,
                Status = OrderStatus.NaCekanju,
                OrderDate = DateTime.UtcNow,
                TotalAmount = request.Items.Sum(i => i.Price * i.Quantity),
                Items = request.Items.Select(i => new OrderItem { Name = i.Name, Price = i.Price, Quantity = i.Quantity }).ToList()
            };

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }

        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusRequest request, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
            if (order is null)
                return NotFound();

            order.Status = request.Status;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}
