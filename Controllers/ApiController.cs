using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using MusicDb.Utilities;

namespace MusicDb.Controllers {

  // API Controller uses the API Proxier utility class to make async API calls.
  // Responses retain their string format and are parsed in the Artist Controller.

  public class ApiController : Controller {

    private readonly ApiProxier Proxy;
    public ApiController(ApiProxier proxy) => Proxy = proxy;

    [Route("api/songs/{text}")]
    async public Task<IActionResult> GetSearchResults(string text) {
      var Url = $"/search?q={text}&sort=popularity&per_page=50";
      var ResponseString = await Proxy.GetAsync("Search", Url);
      HttpContext.Session.SetString($"search{text}", ResponseString);
      return RedirectToAction("ShowSearchResults", "Artist", new { text = text });
    }

    [Route("api/artists/{id}")]
    async public Task<IActionResult> GetArtistInfo(string id) {
      var Url = $"/artists/{id}";
      var ResponseString = await Proxy.GetAsync("Search", Url);
      HttpContext.Session.SetString($"artist{id}", ResponseString);
      return RedirectToAction("ShowArtist", "Artist", new { id = id });
    }

    [Route("api/artists/{id}/songs")]
    async public Task<IActionResult> GetArtistSongs(string id) {
      var Url = $"/artists/{id}/songs?sort=popularity&per_page=50";
      var ResponseString = await Proxy.GetAsync("Search", Url);
      HttpContext.Session.SetString($"songs{id}", ResponseString);
      return RedirectToAction("ShowArtistSongs", "Artist", new { id = id });
    }

  }

}