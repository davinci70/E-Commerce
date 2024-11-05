using e_commerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace e_commerce.Data.Config.EntityConfigurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(x => x.ShippingAddress)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(200)
                .IsRequired();
            
            builder.Property(x => x.Status)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(30)
                .IsRequired();
            
            builder.Property(x => x.TotalPrice)
                .HasPrecision(15, 2)
                .IsRequired();

            builder.HasMany(x => x.OrderItems)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Orders");
        }
    }
}
