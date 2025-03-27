using EmployeeManagementSystem.Configurations;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Admin> Admins { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Leave> Leaves { get; set; }
    public DbSet<Timesheet> Timesheets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}

//base.OnModelCreating(modelBuilder);
//modelBuilder.ApplyConfiguration(new AdminConfigurations());
//modelBuilder.ApplyConfiguration(new DepartmentConfigurations());
//modelBuilder.ApplyConfiguration(new EmployeeConfigurations());
//modelBuilder.ApplyConfiguration(new LeaveConfigurations());
//modelBuilder.ApplyConfiguration(new TimesheetConfigurations());