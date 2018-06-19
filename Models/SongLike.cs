namespace MusicDb.Models {

  public class SongLike : BaseEntity {
    public User User { get; set; }
    public Song Song { get; set; }
  }

}