using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicDb.Models {

  public abstract class BaseEntity {

    [Key] public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

  }

}