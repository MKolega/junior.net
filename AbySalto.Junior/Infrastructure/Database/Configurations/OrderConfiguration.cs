using AbySalto.Junior.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AbySalto.Junior.Infrastructure.Database.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.CustomerName).HasMaxLength(200);
            builder.Property(o => o.PaymentMethod).HasMaxLength(50);
            builder.Property(o => o.Address).HasMaxLength(500);
            builder.Property(o => o.PhoneNumber).HasMaxLength(20);
            builder.Property(o => o.Notes).HasMaxLength(1000);
            builder.Property(o => o.Currency).HasMaxLength(10);
            builder.Property(o => o.TotalAmount).HasPrecision(18, 2);

            builder.HasMany(o => o.Items)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
