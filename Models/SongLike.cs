namespace MusicDb.Models {

  public class SongLike : BaseEntity {

    public int UserId { get; set; }
    public User User { get; set; }

    public int ArtistId { get; set; }
    public Song Song { get; set; }

  }

}