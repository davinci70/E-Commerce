using e_commerce.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace e_commerce.Data.Config.UsersConfigurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasOne(x => x.Address)
                .WithMany()
                .HasForeignKey(x => x.AddressID)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
