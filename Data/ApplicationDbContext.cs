using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
    public DbSet<CustomerGroup> CustomerGroups { get; set; }
    public DbSet<CustomerAction> CustomerActions { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerActionHistory> CustomerActionHistories { get; set; }

    #region Required
    protected override void OnModelCreating(ModelBuilder builder)
    {
        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            v => v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
            v => v.HasValue ? v.Value.ToUniversalTime() : v,
            v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (entityType.IsKeyless)
            {
                continue;
            }

            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime))
                {
                    property.SetValueConverter(dateTimeConverter);
                }
                else if (property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(nullableDateTimeConverter);
                }
            }
        }
    }
    #endregion
}