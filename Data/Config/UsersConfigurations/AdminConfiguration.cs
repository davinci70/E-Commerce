using e_commerce.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace e_commerce.Data.Config.UsersConfigurations
{
    public class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {

            builder.Property(x => x.AdminNotes)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(300)
                .IsRequired();

            builder.ToTable("Admins")
                .HasBaseType<ApplicationUser>();
        }
    }
}
