using System.Data.Common;
using AbySalto.Junior.Models;
using Microsoft.EntityFrameworkCore;
namespace AbySalto.Junior.Infrastructure.Database
{
    public interface IApplicationDbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
