using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;

using MusicDb.Services;
using MusicDb.Models;
using MusicDb.Extensions;

namespace MusicDb.Controllers {

  public class UserController : Controller {

    private Context _context;
    private ArtistService _as;

    public UserController(Context context, ArtistService artistService) {
      _context = context;
      _as = artistService;
    }

    [Route("register")] public IActionResult RegisterGet() => View("register");

    [HttpPost]
    [Route("register")]
    public IActionResult RegisterPost(AccountVM account) {
      if (!ModelState.IsValid) return RedirectToAction("RegisterGet");
      var NewUser = new User {
        Username = account.Username,
        Password = account.Password
      };
      _context.Users.Add(NewUser);
      _context.SaveChanges();
      HttpContext.Session.SetString("username", NewUser.Username);
      return RedirectToAction("Index", "Artist");
    }

  }
}