using Ecom.Core.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecom.infrastructure.Data.Congig;
public class OrderConfiguration : IEntityTypeConfiguration<Orders>
{
    public void Configure(EntityTypeBuilder<Orders> builder)
    {
        builder.OwnsOne(orders => orders.ShippingAderess,
              n => { n.WithOwner(); });

        builder.HasMany(orders => orders.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);

        builder.Property(orders => orders.Status).HasConversion(
             status => status.ToString(),
             status => (Status)Enum.Parse(typeof(Status), status));

        builder.Property(m => m.SubTotal).HasColumnType("decimal(18,2)");
    }
}

