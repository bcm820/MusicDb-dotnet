using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicDb.Models {

  public abstract class BaseEntity {

    // Key annotation required to explicitly identify id
    // Otherwise, EF defaults key to {ClassName}Id/id
    [Key] public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

  }

}