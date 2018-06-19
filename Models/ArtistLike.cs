namespace MusicDb.Models {

  public class ArtistLike : BaseEntity {

    public User User { get; set; }
    public Artist Artist { get; set; }

  }

}