using BackEnd.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Lead> Leads { get; set; }
    public DbSet<Event> Events { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var timeOnlyConverter = new ValueConverter<TimeOnly, TimeSpan>(
            t => t.ToTimeSpan(),
            t => TimeOnly.FromTimeSpan(t)
        );

        var dateOnlyConverter = new ValueConverter<DateOnly, DateTime>(
            d => d.ToDateTime(TimeOnly.MinValue),
            d => DateOnly.FromDateTime(d)
        );

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(TimeOnly))
                {
                    property.SetValueConverter(timeOnlyConverter);
                    property.SetColumnType("time");
                }

                if (property.ClrType == typeof(DateOnly))
                {
                    property.SetValueConverter(dateOnlyConverter);
                    property.SetColumnType("date");
                }
            }
        }
    }

}
