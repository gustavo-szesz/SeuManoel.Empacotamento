using br.seumanoel.empacotamento.api.Models;
using Microsoft.EntityFrameworkCore;


namespace br.seumanoel.empacotamento.api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }

        public DbSet<PackingResult> PackingResults { get; set; }
        public DbSet<PackedBox> PackedBoxes { get; set; }
        public DbSet<PackedProduct> PackedProducts { get; set; }

    }
}
