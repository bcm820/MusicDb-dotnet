using System;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using MusicDb.Models;

namespace MusicDb.Controllers {

  public class ArtistController : Controller {

    // Inject EF Core DB context
    // Usage: Db.{Model}.ToList();
    private Context Db;

    public ArtistController(Context context) {
      Db = context;
    }

    [Route("")]
    public IActionResult Index() {
      return View();
    }

    [Route("searchResults")]
    public IActionResult ShowSongResults() {
      // Genius: Search for songs
      // 1. Make async call to API
      // 2. Filter through data with any Db info
      //    e.g. Your likes, other user's likes
      // 3. Send data to other route via TempData
      // 4. Render data on other route into ViewBag
      return View("searchResults");
    }

    [Route("artist")]
    public IActionResult ShowArtistPage() {
      // Genius: Get artist info (i.e. their songs)
      // Filter through data with any Db info (e.g. likes)
      return View("artist");
    }

    [Route("songs")]
    public IActionResult ListSongsByArtist() {
      // Genius: Get artist info (i.e. their songs)
      // Filter through data with any Db info (e.g. likes)
      return View("songs");
    }

  }
}