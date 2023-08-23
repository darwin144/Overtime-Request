using APIs.Model;
using APIs.Utilities.Enum;
using Microsoft.EntityFrameworkCore;

namespace APIs.Context
{
    public class PayrollOvertimeContext : DbContext
    {
        public PayrollOvertimeContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeLevel> EmployeeLevels { get; set; }
        public DbSet<Overtime> Overtimes { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //set data role
            builder.Entity<Role>().HasData(new Role
            {
                Id = Guid.Parse("f147a695-1a4f-4960-bffc-08db60bf618f"),
                Name = nameof(RoleLevel.Employee)
            },
              new Role
              {
                  Id = Guid.Parse("c22a20c5-0149-41fd-bffd-08db60bf618f"),
                  Name = nameof(RoleLevel.Manager)
              },
              new Role
              {
                  Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                  Name = nameof(RoleLevel.Admin)
              }
            );

            builder.Entity<Employee>().HasIndex(e => new { e.NIK, e.Email, e.PhoneNumber }).IsUnique();

            // add foreign key
            builder.Entity<Overtime>().HasOne(u => u.Employee).WithMany(e => e.Overtimes)
                .HasForeignKey(e => e.Employee_id);

            builder.Entity<Payroll>().HasOne(u => u.Employee).WithMany(e => e.Payrolls)
                .HasForeignKey(e => e.Employee_id);

            builder.Entity<Department>().HasMany(u => u.Employee).WithOne(e => e.Department)
                .HasForeignKey(e => e.Department_id);

            builder.Entity<EmployeeLevel>().HasMany(u => u.Employees).WithOne(e => e.EmployeeLevel)
                .HasForeignKey(e => e.EmployeeLevel_id);

            builder.Entity<Role>().HasMany(u => u.AccountRoles).WithOne(e => e.Role)
                .HasForeignKey(e => e.Role_id);

            builder.Entity<AccountRole>().HasOne(u => u.Account).WithMany(e => e.AccountRoles)
                .HasForeignKey(e => e.Account_id);

            builder.Entity<Account>().HasOne(u => u.Employee).WithOne(e => e.Account)
                .HasForeignKey<Account>(e => e.Employee_id);
        }
    }
}