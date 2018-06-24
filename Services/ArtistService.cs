using System;
using System.Threading.Tasks;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

using MusicDb.Models;
using MusicDb.Extensions;

namespace MusicDb.Services {

  public class ArtistService {

    private Context _context;
    private ApiService _as;

    public ArtistService(Context context, ApiService artistService) {
      _context = context;
      _as = artistService;
    }

    // Data fetched from API proxy is parsed into an ExpandoObject,
    // a unique C# type that bypasses compile time type checks,
    // allowing free access to deep-nested values via dot notation.

    async public Task<dynamic> ShowSearchResults(string text) {

      // Get song data from search API call
      var ResponseString = await _as.GetSearchResults(text);

      // Parse into ExpandoObject, check for an error
      dynamic ResponseObj = JsonConvert.DeserializeObject<ExpandoObject>
        (ResponseString, new ExpandoObjectConverter());
      if (((IDictionary<string, object>)ResponseObj).ContainsKey("error"))
        return ResponseObj;

      // Map song data to Song model, map to list and return
      return GetSongsList(ResponseObj.response.hits, true);
    }

    async public Task<dynamic> ShowArtist(string id) {

      // Check database for artist
      var DbArtists = _context.Artists
        .Where(a => a.Id == Convert.ToInt64(id))
        .Include(a => a.Likes)
        .ToList();
      if (DbArtists.Count != 0) return DbArtists.Single();

      // Get data from artist API call
      var ResponseString = await _as.GetArtistInfo(id);

      // Parse into ExpandoObject, check for an error
      dynamic ResponseObj = JsonConvert.DeserializeObject<ExpandoObject>
        (ResponseString, new ExpandoObjectConverter());
      if (((IDictionary<string, object>)ResponseObj).ContainsKey("error"))
        return ResponseObj;

      // Map artist data to Artist model, store in DB, return artist
      var ArtistData = ResponseObj.response.artist;
      var Artist = new Artist {
        Id = ArtistData.id,
        Name = ArtistData.name,
        Url = ArtistData.url,
        Image = ArtistData.image_url,
        Instagram = ArtistData.instagram_name,
        Twitter = ArtistData.twitter_name
      };
      _context.Artists.Add(Artist);
      _context.SaveChanges();
      return Artist;
    }

    async public Task<dynamic> ShowArtistSongs(string id) {

      // Get data from artist's songs API call
      var ResponseString = await _as.GetArtistSongs(id);

      // Deserialize into ExpandoObject, check for an error
      dynamic ResponseObj = JsonConvert.DeserializeObject<ExpandoObject>
        (ResponseString, new ExpandoObjectConverter());
      if (((IDictionary<string, object>)ResponseObj).ContainsKey("error"))
        return ResponseObj;

      // Map song data to Song model, map to list and return
      return GetSongsList(ResponseObj.response.songs, false);
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

  }

}