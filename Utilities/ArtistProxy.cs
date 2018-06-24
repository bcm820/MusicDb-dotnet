using System.Threading.Tasks;

namespace MusicDb.Utilities {

  // ArtistProxy uses the API Proxier utility class to make async API calls.
  // Responses retain their string format and are parsed in the Artist Controller.

  public class ArtistProxy {

    private readonly ApiProxier Proxier;
    public ArtistProxy(ApiProxier proxier) => Proxier = proxier;

    async public Task<string> GetSearchResults(string text) {
      var Url = $"/search?q={text}&sort=popularity&per_page=50";
      return await Proxier.GetAsync("Search", Url);
    }

    async public Task<string> GetArtistInfo(string id) {
      var Url = $"/artists/{id}";
      return await Proxier.GetAsync("Search", Url);
    }

    async public Task<string> GetArtistSongs(string id) {
      var Url = $"/artists/{id}/songs?sort=popularity&per_page=50";
      return await Proxier.GetAsync("Search", Url);
    }

  }

}