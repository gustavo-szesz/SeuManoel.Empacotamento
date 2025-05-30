using br.seumanoel.empacotamento.api.Models;
using Microsoft.EntityFrameworkCore;


namespace br.seumanoel.empacotamento.api.Data
{
    /// <summary>
    /// DbContext of the Aplication
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Initialize the DbContext with the specified options.
        /// </summary>
        /// <param name="options"></param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        #region DbSets Users
        /// <summary>
        /// DbSet for Users
        /// </summary>
        public DbSet<User> Users { get; set; }
        #endregion

        #region DbSets Packing
        /// <summary>
        /// DbSet for Packing 
        /// </summary>
        public DbSet<PackingResult> PackingResults { get; set; }
        public DbSet<PackedBox> PackedBoxes { get; set; }
        public DbSet<PackedProduct> PackedProducts { get; set; }
        #endregion

    }
}
