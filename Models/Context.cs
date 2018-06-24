using Microsoft.EntityFrameworkCore;

namespace MusicDb.Models {

  public class Context : DbContext {

    // Db Tables
    public DbSet<Artist> Artists { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ArtistLike> ArtistLikes { get; set; }

    // Context calls parent class' constructor
    // passing in the "options" parameter
    public Context(DbContextOptions<Context> options) : base(options) { }

    // This can be used to add additional config options
    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
      options.EnableSensitiveDataLogging();

    // OnModelCreating can be used to override the FluentApi
    // protected override void OnModelCreating(ModelBuilder mb) {}

  }

}