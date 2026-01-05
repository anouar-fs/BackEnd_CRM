using Microsoft.EntityFrameworkCore;
using BackEnd.Entities;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Lead> Leads { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    { }

}
