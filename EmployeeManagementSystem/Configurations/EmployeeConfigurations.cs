using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagementSystem.Configurations
{
    public class EmployeeConfigurations: IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.EmployeeID);

            builder.Property(e => e.EmployeeID)
                   .UseIdentityColumn(1, 1);

            builder.Property(e => e.FirstName)
                   .HasColumnType("VARCHAR(100)")
                   .IsRequired();

            builder.Property(e => e.LastName)
                   .HasColumnType("VARCHAR(100)")
                   .IsRequired();

            builder.Property(e => e.Email)
                   .HasColumnType("VARCHAR(255)")
                   .IsRequired();

            builder.HasIndex(e => e.Email)
                   .IsUnique();

            builder.Property(e => e.PhoneNumber)
                   .HasColumnType("VARCHAR(15)")
                   .IsRequired();

            builder.Property(e => e.DateofBirth)
                    .HasColumnType("DATE")
                   .IsRequired(false);

            builder.Property(e => e.Address)
                   .HasColumnType("TEXT")
                   .IsRequired(false);

            builder.Property(e => e.TechStack)
                   .HasColumnType("TEXT")
                   .IsRequired(false);

            builder.Property(e => e.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(e => e.UpdatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.HasMany(e => e.Timesheets)
                   .WithOne(d => d.Employee)
                   .HasForeignKey(e => e.EmployeeID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Leaves)
                   .WithOne(d => d.Employee)
                   .HasForeignKey(e => e.EmployeeID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
