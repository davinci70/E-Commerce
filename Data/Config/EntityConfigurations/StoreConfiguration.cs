using e_commerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace e_commerce.Data.Config.EntityConfigurations
{
    public class StoreConfiguration : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.HasKey(x => x.StoreID);

            builder.Property(x => x.StoreName)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(150)
                .IsRequired();
            
            builder.Property(x => x.Description)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(1500)
                .IsRequired();

            builder.ToTable("Stores");
        }
    }
}
