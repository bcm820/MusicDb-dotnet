using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MusicDb.Utilities;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using MusicDb.Models;

namespace MusicDb.Controllers {

  public class ViewController : Controller {

    private Context Db;
    public ViewController(Context context) => Db = context;

    [Route("")]
    public IActionResult Index() {
      ViewBag.Artists = Db.Artists.OrderBy(artist => artist.Name).ToList();
      return View();
    }

    [Route("register")] public IActionResult RegisterGet() => View("register");

    [HttpPost]
    [Route("register")]
    public IActionResult RegisterPost(Account account) {
      if (!ModelState.IsValid) return RedirectToAction("RegisterGet");
      var NewUser = new User {
        Username = account.Username,
        Password = account.Password
      };
      HttpContext.Session.SetString("username", NewUser.Username);
      return RedirectToAction("Index");
    }

    [Route("results/{text}")]
    public IActionResult Results(string text) {
      var Results = HttpContext.Session.GetDynamic($"search{text}");
      if (Results == null)
        return RedirectToAction("GetSearchResults", "Api", new { text = text });
      ViewBag.Results = Results;
      return View("results");
    }

    // TODO: Fetch Twitter & Instagram data
    [Route("artists/{id}/profile")]
    public IActionResult Artist(string id) {
      var Artist = HttpContext.Session.GetDynamic($"artistprofile{id}");
      var Songs = HttpContext.Session.GetDynamic($"artistsongs{id}");
      if (Artist == null)
        return RedirectToAction("GetArtistInfo", "Api", new { id = id });
      if (Songs == null)
        return RedirectToAction("GetArtistSongs", "Api", new { id = id });
      ViewBag.Artist = Artist;
      ViewBag.Songs = Songs;
      return View("artist");
    }

  }
}