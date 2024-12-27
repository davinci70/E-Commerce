using e_commerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace e_commerce.Data.Config.EntityConfigurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.ProductID);

            builder.Property(x => x.Name)
                .HasColumnType("NVARCHAR")
                .HasColumnName("Name")
                .HasMaxLength(200)
                .IsRequired();
            
            builder.Property(x => x.Description)
                .HasColumnType("NVARCHAR")
                .HasColumnName("Description")
                .HasMaxLength(2000)
                .IsRequired();
            
            builder.Property(x => x.SmallDescription)
                .HasColumnType("NVARCHAR")
                .HasColumnName("SmallDescription")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.Price)
                .HasPrecision(15, 2)
                .IsRequired();
            
            builder.Property(x => x.Discount)
                .HasPrecision(15, 2)
                .IsRequired();

            builder.HasOne(x => x.Seller)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.SellerID);

            builder.HasMany(x => x.CartItems)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductID)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.ToTable("Products");
        }
    }
}
