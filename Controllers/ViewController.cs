using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;

using MusicDb.Services;
using MusicDb.Models;
using MusicDb.Extensions;

namespace MusicDb.Controllers {

  public class ViewController : Controller {

    private Context _context;
    private ArtistService _as;

    public ViewController(Context context, ArtistService artistService) {
      _context = context;
      _as = artistService;
    }

    [Route("")]
    public IActionResult Index() {
      ViewBag.Artists = _context.Artists.OrderBy(artist => artist.Name).ToList();
      ViewBag.User = HttpContext.Session.GetString("username");
      return View();
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
      return RedirectToAction("Index");
    }

    [Route("results/{text}")]
    async public Task<IActionResult> Results(string text) {
      ViewBag.Results = await _as.ShowSearchResults(text);
      return View("results");
    }

    // TODO: Fetch Twitter & Instagram data 
    [Route("artists/{id}/profile")]
    async public Task<IActionResult> Artist(string id) {
      var Artist = await _as.ShowArtist(id);
      var Songs = await _as.ShowArtistSongs(id);
      ViewBag.Artist = Artist;
      ViewBag.Songs = Songs;
      return View("artist");
    }

  }
}