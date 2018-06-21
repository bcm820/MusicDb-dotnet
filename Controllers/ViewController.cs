using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Http;
using MusicDb.Utilities;

// using System;
// using System.Dynamic;
// using Newtonsoft.Json;
// using Newtonsoft.Json.Converters;

using Microsoft.EntityFrameworkCore;
// using System.Collections.Generic;
using System.Linq;
using MusicDb.Models;

namespace MusicDb.Controllers {

  public class ViewController : Controller {

    private Context Db;
    public ViewController(Context context) => Db = context;

    // TODO: Get songs from DB too
    [Route("")]
    public IActionResult Index() {
      ViewBag.Artists = Db.Artists.OrderBy(artist => artist.Name).ToList();
      return View();
    }

    [Route("results/{text}")]
    public IActionResult Results(string text) {
      var Results = HttpContext.Session.GetDynamic($"search{text}");
      if (Results == null)
        return RedirectToAction("GetSearchResults", "Api", new { text = text });
      ViewBag.Results = Results;
      return View("results");
    }

    // TODO: Add DB songs matching artist
    // TODO: Fetch Twitter & Instagram data
    [Route("artists/{id}/profile")]
    public IActionResult Artist(string id) {
      var Artist = HttpContext.Session.GetDynamic($"artistprofile{id}");
      if (Artist == null)
        return RedirectToAction("GetArtistInfo", "Api", new { id = id });
      ViewBag.Artist = Artist;
      return View("artist");
    }

  }
}