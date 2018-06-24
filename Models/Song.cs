using System;
using System.Collections.Generic;

namespace MusicDb.Models {

  public class Song : BaseEntity {
    public string Title { get; set; }
    public string Url { get; set; }
    public string ArtistName { get; set; }
    public long ArtistUID { get; set; }

  }

}

