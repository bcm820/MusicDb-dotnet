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

/*
Useful Annotations and Examples:
[Required]
[MinLength(100)]
[MaxLength(1000)]
[Range(5,10)] - Field must be between 5 and 10 characters.
[RegularExpression(@"[0-9]{0,}\.[0-9]{2}", ErrorMessage = "error Message")]
[EmailAddress] - Field must contain an @ symbol, followed by a word and a period.
[DataType(DataType.Password)] - Ensures field conforms to specific DataType

PostGres Migrations:
dotnet ef migrations add FirstMigration - Creates migration. Requires one model in advance.
dotnet ef database update - Applies migrations much like Django's "migrate" command.
*/
