using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagementSystem.Configurations
{
    public class TimesheetConfigurations : IEntityTypeConfiguration<Timesheet>
    {
        public void Configure(EntityTypeBuilder<Timesheet> builder)
        {
            builder.HasKey(t => t.TimesheetID);

            builder.Property(t => t.TimesheetID)
                   .UseIdentityColumn(1, 1);

            builder.Property(t => t.Date)
                   .IsRequired();

            builder.Property(t => t.StartTime)
                   .IsRequired();

            builder.Property(t => t.EndTime)
                   .IsRequired(false);

            builder.Property(t => t.TotalHoursWorked)
                   .HasColumnType("DECIMAL(5,2)")
                   .IsRequired();

            builder.Property(t => t.Description)
                   .HasColumnType("TEXT")
                   .IsRequired(false);

            builder.Property(t => t.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");
        }
    }
}
