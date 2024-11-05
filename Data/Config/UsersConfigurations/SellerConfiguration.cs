using e_commerce.Models.Entities;
using e_commerce.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace e_commerce.Data.Config.UsersConfigurations
{
    public class SellerConfiguration : IEntityTypeConfiguration<Seller>
    {
        public void Configure(EntityTypeBuilder<Seller> builder)
        {
            builder.HasOne(x => x.Store)
                .WithOne(x => x.Seller)
                .HasForeignKey<Store>(x => x.SellerID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Sellers")
                .HasBaseType<ApplicationUser>();
        }
    }
}
