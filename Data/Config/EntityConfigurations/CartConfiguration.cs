using e_commerce.Models.Entities;
using e_commerce.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace e_commerce.Data.Config.EntityConfigurations
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(x => x.CartId);

            builder.Property(c => c.CustomerID)
                .IsRequired();

            builder.HasMany(x => x.cartItems)
                .WithOne(x => x.Cart)
                .HasForeignKey(x => x.CartID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.CreatedDate)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            builder.ToTable("Carts");
        }
    }
}
