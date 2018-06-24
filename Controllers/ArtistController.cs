using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using System;
using System.Threading.Tasks;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using MusicDb.Models;
using MusicDb.Utilities;

namespace MusicDb.Controllers {

  public class ArtistController : Controller {

    private Context Db;
    private ArtistProxy Proxy;
    public ArtistController(Context context, ArtistProxy proxy) {
      Db = context;
      Proxy = proxy;
    }

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

    // Data fetched from API proxy is parsed into an ExpandoObject,
    // a unique C# type that bypasses compile time type checks,
    // allowing free access to deep-nested values via dot notation.

    [Route("songs")]
    async public Task<IActionResult> ShowSearchResults(string text) {

      // Get song data from search API call
      var ResponseString = await Proxy.GetSearchResults(text);

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
    async public Task<IActionResult> ShowArtist(string id) {

      // Check database for artist
      // If found, redirect to fetch artist's songs
      var DbArtists = Db.Artists
        .Where(a => a.Id == Convert.ToInt64(id))
        .Include(a => a.Likes)
        .ToList();
      if (DbArtists.Count != 0) {
        var FoundArtist = DbArtists.Single();
        HttpContext.Session.SetDynamic($"artistprofile{id}", FoundArtist);
        return RedirectToAction("ShowArtistSongs", new { id = id });
      }

      // Get data from artist API call
      var ResponseString = await Proxy.GetArtistInfo(id);

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
      return RedirectToAction("ShowArtistSongs", new { id = id });
    }

    [Route("artists/{id}/songs")]
    async public Task<IActionResult> ShowArtistSongs(string id) {

      // Get data from artist's songs API call
      var ResponseString = await Proxy.GetArtistSongs(id);

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