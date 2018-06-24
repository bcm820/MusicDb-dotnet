using System.ComponentModel.DataAnnotations;

namespace MusicDb.Models {

  public class Account {

    [Required]
    [MinLength(2)]
    [RegularExpression(@"^[a-zA-Z]+$")]
    public string Username { get; set; }

    [Required]
    [MinLength(8)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Compare("Password",
    ErrorMessage = "Password and confirmation must match.")]
    public string Confirmation { get; set; }

  }

}