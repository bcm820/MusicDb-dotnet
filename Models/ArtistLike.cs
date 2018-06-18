namespace MusicDb.Models {

  public class ArtistLike : BaseEntity {

    public long UserId { get; set; }
    public User User { get; set; }

    public long ArtistId { get; set; }
    public Artist Artist { get; set; }

  }

}