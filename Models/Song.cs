using System;
using System.Collections.Generic;

namespace MusicDb.Models {

  public class Song : BaseEntity {
    public string Title { get; set; }
    public string Url { get; set; }
    public string ArtistName { get; set; }
    public long ArtistUID { get; set; }
    public Artist Artist { get; set; }
    public List<SongLike> Likes { get; set; }

    public Song() {
      Likes = new List<SongLike>();
    }

  }

}

