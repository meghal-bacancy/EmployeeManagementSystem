using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagementSystem.Configurations
{
    public class LeaveConfigurations : IEntityTypeConfiguration<Leave>
    {
        public void Configure(EntityTypeBuilder<Leave> builder)
        {
            builder.HasKey(l => l.LeaveID);

            builder.Property(l => l.LeaveID)
                   .UseIdentityColumn(1, 1);

            builder.Property(l => l.StartDate)
                   .IsRequired();

            builder.Property(l => l.EndDate)
                   .IsRequired();

            builder.Property(l => l.LeaveType)
               .HasColumnType("VARCHAR(50)")
               .IsRequired();

            // ? 
            builder.ToTable(l =>
            {
                l.HasCheckConstraint("CK_Leave_LeaveType", "[LeaveType] IN ('Sick', 'Casual', 'Vacation', 'Other')");
                l.HasCheckConstraint("CK_Leave_Status", "[Status] IN ('Pending', 'Approved', 'Rejected')");
            });
            //

            builder.Property(l => l.Status)
                   .HasDefaultValue("Pending");

            builder.Property(l => l.Reason)
                   .HasColumnType("TEXT")
                   .IsRequired(false);

            builder.Property(l => l.LeaveType)
                   .HasColumnType("VARCHAR(20)")
                   .IsRequired();

            builder.Property(l => l.AppliedAt)
                   .HasDefaultValueSql("GETDATE()");
        }
    }
}
