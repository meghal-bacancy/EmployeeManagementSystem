using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagementSystem.Configurations
{
    public class AdminConfigurations : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.HasKey(a => a.AdminID);

            builder.Property(a => a.AdminID)
                   .UseIdentityColumn(1, 1);

            builder.Property(a => a.FirstName)
                   .HasColumnType("VARCHAR(100)")
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.LastName)
                   .HasColumnType("VARCHAR(100)")
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.Email)
                   .HasColumnType("VARCHAR(255)")
                   .IsRequired();

            builder.HasIndex(a => a.Email)
                   .IsUnique();

            builder.Property(e => e.PhoneNumber)
                   .HasColumnType("VARCHAR(15)")
                   .IsRequired();

            builder.Property(a => a.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(a => a.UpdatedAt)
                   .HasDefaultValueSql("GETDATE()");
        }
    }
}
