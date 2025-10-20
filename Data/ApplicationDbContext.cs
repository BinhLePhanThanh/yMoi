using Microsoft.EntityFrameworkCore;
using yMoi.Model;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Unit> Units { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<ServiceFile> ServiceFiles { get; set; }
    public DbSet<Medicine> Medicines { get; set; }
    public DbSet<MedicineFile> MedicineFiles { get; set; }
}