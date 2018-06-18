using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using System;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
      ViewBag["artists"] = Db.Artists.OrderBy(artist => artist.Name).ToList();
      return View();
    }

    // 1. Get artist id
    // 2. Check db for artist
    // 3. If none found, fetch from API and convert to artist class
    // 4. Render on page
    // 5. TODO: If user likes, store in DB
    // 6. TODO: If user likes a song, store in DB

    // Data fetched from API proxy (stored in session in ApiController)
    // will be deserialized into an ExpandoObject which supports type
    // "dynamic" (bypass compile time type check and assume any op works),
    // allowing free access to deep-nested values via dot notation.

    [Route("search/{text}")] // TODO: Change to POST for form-data
    public IActionResult ShowSearchResults(string text) {
      var ResponseString = HttpContext.Session.GetString($"search{text}");
      if (ResponseString == null)
        return RedirectToAction("GetSearchResults", "Api", new { text = text });
      dynamic ResponseObj = JsonConvert.DeserializeObject<ExpandoObject>
        (ResponseString, new ExpandoObjectConverter());
      if (((IDictionary<string, object>)ResponseObj).ContainsKey("error"))
        return Json("Unable to fulfill request.");
      dynamic SongObjects = ResponseObj.response.hits;
      var Songs = new List<Song>();
      foreach (var song in SongObjects) Songs.Add(new Song {
        Id = song.id,
        Title = song.title_with_featured,
        Url = song.url,
        ArtistName = song.primary_artist.name
      });
      // return View("results");
      return Json(Songs);
    }

    [Route("artists/{id}")]
    public IActionResult ShowArtist(string id) {
      var DbArtist = Db.Artists.SingleOrDefault(a => a.Id == Convert.ToInt64(id));
      if (DbArtist != null) return Json(DbArtist);
      var ResponseString = HttpContext.Session.GetString($"artist{id}");
      if (ResponseString == null)
        return RedirectToAction("GetArtistInfo", "Api", new { id = id });
      dynamic ResponseObj = JsonConvert.DeserializeObject<ExpandoObject>
        (ResponseString, new ExpandoObjectConverter());
      if (((IDictionary<string, object>)ResponseObj).ContainsKey("error"))
        return Json("Unable to fulfill request.");
      var ArtistData = ResponseObj.response.artist;
      var Artist = new Artist {
        Id = ArtistData.id,
        Name = ArtistData.user.name,
        Image = ArtistData.image_url,
        Url = ArtistData.url,
        Genius = ArtistData.user.login,
        Instagram = ArtistData.instagram_name,
        Twitter = ArtistData.twitter_name,
        Facebook = ArtistData.facebook_name
      };
      Db.Artists.Add(Artist);
      Db.SaveChanges();
      // return View("artist");
      return Json(Artist);
    }

    [Route("artists/{id}/songs")]
    public IActionResult ShowArtistSongs(string id) {
      var ResponseString = HttpContext.Session.GetString($"songs{id}");
      if (ResponseString == null)
        return RedirectToAction("GetArtistSongs", "Api", new { id = id });
      dynamic ResponseObj = JsonConvert.DeserializeObject<ExpandoObject>
        (ResponseString, new ExpandoObjectConverter());
      if (((IDictionary<string, object>)ResponseObj).ContainsKey("error"))
        return Json("Unable to fulfill request.");
      dynamic SongObjects = ResponseObj.response.songs;
      var Songs = new List<Song>();
      foreach (var songData in SongObjects) {
        long songId = songData.id;
        var DbSong = Db.Songs.SingleOrDefault(s => s.Id == songId);
        if (DbSong != null) Songs.Add(DbSong);
        else {
          var Song = new Song {
            Id = songData.id,
            Title = songData.title_with_featured,
            Url = songData.url,
            ArtistName = songData.primary_artist.name
          };
          Songs.Add(Song);
          // TODO: Store artist before storing song.
          // Song depends on having an actual artist stored.
          // Db.Songs.Add(Song);
          // Db.SaveChanges();
        }
      }
      // return View("songs");
      return Json(Songs);
    }

  }
}