using Microsoft.EntityFrameworkCore;

namespace MusicDb.Models {

  public class Context : DbContext {

    // Db Tables
    public DbSet<User> Users { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Song> Songs { get; set; }

    // Context calls parent class' constructor
    // passing in the "options" parameter
    public Context(DbContextOptions<Context> options) : base(options) { }
  }

}