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

    static List<Song> GetSongsList(dynamic songs, bool results) {
      var SongsList = new List<Song>();
      foreach (var song in songs) {
        var s = results ? song.result : song;
        SongsList.Add(new Song {
          Id = s.id,
          Title = s.title_with_featured,
          Url = s.url,
          ArtistName = s.primary_artist.name,
          ArtistUID = s.primary_artist.id
        });
      }
      return SongsList;
    }

    // Data fetched from API proxy (stored in session in ApiController)
    // will be parsed into an ExpandoObject which is a unique C#
    // type that bypasses compile time type checks (assumes any op works).
    // This allows free access to deep-nested values via dot notation.

    [Route("songs")]
    public IActionResult ShowSearchResults(string text) {

      // Get song data from search API call
      var ResponseString = HttpContext.Session.GetString($"search{text}");
      if (ResponseString == null)
        return RedirectToAction("GetSearchResults", "Api", new { text = text });

      // Parse into ExpandoObject, check for an error
      dynamic ResponseObj = JsonConvert.DeserializeObject<ExpandoObject>
        (ResponseString, new ExpandoObjectConverter());
      if (((IDictionary<string, object>)ResponseObj).ContainsKey("error"))
        return Json(ResponseObj);

      // Map song data to Song model, map to list, store in session
      List<Song> Songs = GetSongsList(ResponseObj.response.hits, true);
      HttpContext.Session.SetDynamic($"search{text}", Songs);

      return RedirectToAction("Results", "View", new { text = text });
    }

    [Route("artists/{id}")]
    public IActionResult ShowArtist(string id) {

      // Check database for artist
      // If found, redirect to fetch artist's songs
      var DbArtists = Db.Artists
        .Where(a => a.Id == Convert.ToInt64(id))
        .Include(a => a.Likes)
        .ToList();
      if (DbArtists.Count != 0) {
        var FoundArtist = DbArtists.Single();
        HttpContext.Session.SetDynamic($"artistprofile{id}", FoundArtist);
        return RedirectToAction("GetArtistSongs", "Api", new { id = id });
      }

      // Get data from artist API call
      var ResponseString = HttpContext.Session.GetString($"artist{id}");
      if (ResponseString == null)
        return RedirectToAction("GetArtistInfo", "Api", new { id = id });

      // Parse into ExpandoObject, check for an error
      dynamic ResponseObj = JsonConvert.DeserializeObject<ExpandoObject>
        (ResponseString, new ExpandoObjectConverter());
      if (((IDictionary<string, object>)ResponseObj).ContainsKey("error"))
        return Json(ResponseObj);

      // Map artist data to Artist model, store in DB
      // Then redirect to fetch artist songs
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
      HttpContext.Session.SetDynamic($"artistprofile{id}", Artist);
      return RedirectToAction("GetArtistSongs", "Api", new { id = id });
    }

    [Route("artists/{id}/songs")]
    public IActionResult ShowArtistSongs(string id) {

      // Get data from artist's songs API call
      var ResponseString = HttpContext.Session.GetString($"songs{id}");
      if (ResponseString == null)
        return RedirectToAction("GetArtistSongs", "Api", new { id = id });

      // Deserialize into ExpandoObject, check for an error
      dynamic ResponseObj = JsonConvert.DeserializeObject<ExpandoObject>
        (ResponseString, new ExpandoObjectConverter());
      if (((IDictionary<string, object>)ResponseObj).ContainsKey("error"))
        return Json(ResponseObj);

      // Map song data to Song model, map to list, store in session
      List<Song> Songs = GetSongsList(ResponseObj.response.songs, false);
      HttpContext.Session.SetDynamic($"artistsongs{id}", Songs);

      return RedirectToAction("Artist", "View", new { id = id });
    }

  }
}