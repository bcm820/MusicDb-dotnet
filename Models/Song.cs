using System;
using System.Collections.Generic;

namespace MusicDb.Models {

  public class Song : BaseEntity { // response has "songs" array
    public string Title { get; set; } // title
    public string Url { get; set; } // url

    public int ArtistId { get; set; }
    public Artist Artist { get; set; }

    public List<User> Users { get; set; }

    public Song() {
      Users = new List<User>();
    }

  }

}

