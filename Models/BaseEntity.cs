using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

/* Useful Annotations and Examples:
[Required]
[MinLength(100)]
[MaxLength(1000)]
[Range(5,10)] - Field must be between 5 and 10 characters.
[RegularExpression(@"[0-9]{0,}\.[0-9]{2}", ErrorMessage = "error Message")]
[EmailAddress] - Field must contain an @ symbol, followed by a word and a period.
[DataType(DataType.Password)] - Ensures field conforms to specific DataType

Migrations:
dotnet ef migrations add first - Creates migration. Requires one model in advance.
dotnet ef database update - Creates and updates DB. */

namespace MusicDb.Models {

  public abstract class BaseEntity {

    // Key annotation required to explicitly identify id
    // Otherwise, EF looks for {ClassName}Id/id
    [Key] public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    protected BaseEntity() {
      CreatedAt = DateTime.Now;
      UpdatedAt = DateTime.Now;
    }

  }

}