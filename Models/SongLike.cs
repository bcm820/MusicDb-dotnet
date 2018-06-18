namespace MusicDb.Models {

  public class SongLike : BaseEntity {

    public long UserId { get; set; }
    public User User { get; set; }

    public long ArtistId { get; set; }
    public Song Song { get; set; }

  }

}