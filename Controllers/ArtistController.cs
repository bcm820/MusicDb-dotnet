using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MusicDb.Utilities;

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

    private Context Db;
    public ArtistController(Context context) => Db = context;

    // Data fetched from API proxy (stored in session in ApiController)
    // will be deserialized into an ExpandoObject which is a unique C#
    // type that bypasses compile time type checks (assumes any op works).
    // This allows free access to deep-nested values via dot notation.

    [Route("songs")]
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
        Id = song.result.id,
        Title = song.result.title_with_featured,
        Url = song.result.url,
        ArtistName = song.result.primary_artist.name,
        ArtistUID = song.result.primary_artist.id
      });
      HttpContext.Session.SetDynamic($"search{text}", Songs);
      return RedirectToAction("Results", "View", new { text = text });
    }

    [Route("artists/{id}")]
    public IActionResult ShowArtist(string id) {
      var DbArtists = Db.Artists
        .Where(a => a.Id == Convert.ToInt64(id))
        .Include(a => a.Songs)
        .Include(a => a.Likes)
        .ToList();
      if (DbArtists.Count != 0) {
        var FoundArtist = DbArtists.Single();
        HttpContext.Session.SetDynamic($"artistprofile{id}", FoundArtist);
        return RedirectToAction("Artist", "View", new { id = id });
      }
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
        Name = ArtistData.name,
        Url = ArtistData.url,
        Image = ArtistData.image_url,
        Instagram = ArtistData.instagram_name,
        Twitter = ArtistData.twitter_name
      };
      Db.Artists.Add(Artist);
      Db.SaveChanges();
      return RedirectToAction("ShowArtist", new { id = id });
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
        // var DbArtist = Db.Artists
        //   .Where(a => a.Id == songData.primary_artist.id)
        //   .Include(a => a.Songs)
        //   .ToList();
        var DbSongs = Db.Songs
          .Where(s => s.Id == songId)
          .Include(s => s.Artist)
          .Include(s => s.Likes)
          .ToList();
        if (DbSongs.Count != 0) Songs.Add(DbSongs.Single());
        else {
          var Song = new Song {
            Id = songData.id,
            Title = songData.title_with_featured,
            Url = songData.url,
            ArtistName = songData.primary_artist.name,
            ArtistUID = songData.primary_artist.id
          };
          Songs.Add(Song);
          Db.Songs.Add(Song);
          Db.SaveChanges();
        }
      }
      return View("songs");
      // return Json(Songs);
    }

  }
}