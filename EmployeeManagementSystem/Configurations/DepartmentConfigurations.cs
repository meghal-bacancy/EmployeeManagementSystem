using System.Reflection.Emit;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagementSystem.Configurations
{
    public class DepartmentConfigurations : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.DepartmentID);

            builder.Property(d => d.DepartmentID)
                   .UseIdentityColumn(1, 1);

            builder.Property(d => d.DepartmentName)
                   .HasColumnType("VARCHAR(100)")
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasIndex(d => d.DepartmentName)
                   .IsUnique();

            builder.Property(d => d.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

            builder.HasMany(e => e.Employees)
                    .WithOne(d => d.Department)
                    .HasForeignKey(e => e.DepartmentID)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
