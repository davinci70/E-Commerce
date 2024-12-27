using e_commerce.Models.Entities;
using e_commerce.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace e_commerce.Data.Config.UsersConfigurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasOne(x => x.Cart)
                .WithOne(x => x.Customer)
                .HasForeignKey<Cart>(x => x.CustomerID)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.ToTable("Customers")
                .HasBaseType<ApplicationUser>();
        }
    }
}
