using System;
using System.Collections.Generic;

namespace MusicDb.Models {

  public class User : BaseEntity {
    public string Username { get; set; }
    public string Password { get; set; }

    public List<ArtistLike> Artists { get; set; }
    public List<SongLike> Songs { get; set; }

    public User() {
      Artists = new List<ArtistLike>();
      Songs = new List<SongLike>();
    }

  }

}