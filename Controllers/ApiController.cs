using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicDb.Utilities;
using MusicDb.Models;

namespace MusicDb.Controllers {

  public class ApiController : Controller {

    private ApiProxier Proxy;
    public ApiController(ApiProxier proxy) => Proxy = proxy;

    [HttpPost] // form-data
    [Route("/api/search")]
    // public IActionResult SearchForSong(string song) {
    async public Task<JsonResult> SearchSongs(string searchText) {
      dynamic ResponseObj = await Proxy.Get("Genius", $"search?q={searchText}");
      return Json(ResponseObj.response);
      // return RedirectToAction("ShowSongResults", "Artist");
    }

    [Route("api/artists/{artistId}")]
    // async public Task<IActionResult> GetArtistInfo(string artistId) {
    async public Task<JsonResult> GetArtistInfo(string artistId) {
      dynamic ResponseObj = await Proxy.Get("Genius", $"/artists/{artistId}");
      // TempData["ArtistResponse"] = ProxyResponse.response;
      // return RedirectToAction("ShowArtistPage", "Artist");
      return Json(ResponseObj.response);
    }

    [Route("api/artists/{artistId}/songs&sort=popularity")]
    // async public Task<IActionResult> GetArtistInfo(string artistId) {
    async public Task<JsonResult> GetArtistSongs(string artistId) {
      dynamic ResponseObj = await Proxy.Get("Genius", $"/artists/{artistId}/songs");
      return Json(ResponseObj.response);
      // return RedirectToAction("ListSongsByArtist", "Artist");
    }

    // Like: Check DB for artist; if not, write to DB
    // Like: Check DB for song; if not, write to DB

    // Create associations with users
    // - Many-to-many users & artists
    // - Many-to-many users & songs
    // - Many-to-many users as friends (self-join)

  }

}