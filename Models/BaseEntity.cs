using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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