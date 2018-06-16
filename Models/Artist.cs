using System;
using System.Collections.Generic;

namespace MusicDb.Models {

  public class Artist : BaseEntity {
    public string Name { get; set; } // artist.user.name
    public string Url { get; set; } // artist.url
    public string Genius { get; set; } // artist.user.login
    public string Instagram { get; set; } // artist.instagram_name
    public string Facebook { get; set; } // artist.facebook_name
    public string Twitter { get; set; } // artist.twitter_name

    public List<Song> Songs { get; set; }
    public List<User> Users { get; set; }

    public Artist() {
      Songs = new List<Song>();
      Users = new List<User>();
    }

  }

}