using e_commerce.Models.Entities.RefEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace e_commerce.Data.Config.RefEntitiesConfigurations
{
    public class RefProductTypeConfiguration : IEntityTypeConfiguration<RefProductType>
    {
        public void Configure(EntityTypeBuilder<RefProductType> builder)
        {
            builder.HasKey(x => x.ProductTypeID);

            builder.Property(x => x.ProductTypeName)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(100)
                .IsRequired();

            builder.HasMany(x => x.Products)
                .WithOne(x => x.ProductType)
                .HasForeignKey(x => x.ProductTypeID)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.ToTable("RefProductTypes");
        }
    }
}
