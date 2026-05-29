using Microsoft.EntityFrameworkCore;
using Music_Management_System.Models;

namespace Music_Management_System.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Song> Songs { get; set; }
    public DbSet<Singer> Singers { get; set; }
    public DbSet<Composer> Composers { get; set; }
}