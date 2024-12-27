using e_commerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace e_commerce.Data.Config.EntityConfigurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(x => x.ReviewId);

            builder.Property(b => b.Body)
                .HasColumnType("nvarchar(max)")
                .IsRequired();
            
            builder.Property(b => b.ImagePath)
                .HasColumnType("nvarchar(256)")
                .IsRequired(false);

            builder.HasOne(x => x.Customer)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.CustomerID)
                .OnDelete(DeleteBehavior.Cascade);                

            builder.HasOne(x => x.Product)
                .WithMany(x => x.Reviews)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(x => x.ProductID);

            builder.ToTable("Reviews");
        }
    }
}
