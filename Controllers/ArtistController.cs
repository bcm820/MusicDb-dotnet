using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;

using MusicDb.Services;
using MusicDb.Models;
using MusicDb.Extensions;

namespace MusicDb.Controllers {

  public class ArtistController : Controller {

    private Context _context;
    private ArtistService _as;

    public ArtistController(Context context, ArtistService artistService) {
      _context = context;
      _as = artistService;
    }

    [Route("")]
    public IActionResult Index() {
      ViewBag.Artists = _context.Artists.OrderBy(artist => artist.Name).ToList();
      ViewBag.User = HttpContext.Session.GetString("username");
      return View();
    }

    [HttpPost]
    [Route("search")]
    async public Task<IActionResult> Results(string search) {
      ViewBag.Results = await _as.ShowSearchResults(search);
      return View("search");
    }

    // TODO: Fetch Twitter & Instagram data 
    [Route("artists/{id}")]
    async public Task<IActionResult> Artist(string id) {
      var Artist = await _as.ShowArtist(id);
      var Songs = await _as.ShowArtistSongs(id);
      ViewBag.Artist = Artist;
      ViewBag.Songs = Songs;
      return View("artist");
    }

  }
}