
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace MusicDb.Services {

  public class ApiService {

    // ApiService is a service used for making async API calls.
    // Responses are returned in their original string format for parsing.

    // Inject appsettings config which stores secret API data.
    private readonly IConfiguration Config;
    public ApiService(IConfiguration config) => Config = config;

    // Fetch data from Genius, Twitter, Instagram, etc.

    async public Task<string> GetSearchResults(string text) {
      var Url = $"/search?q={text}&sort=popularity&per_page=50";
      return await GetAsync("Search", Url);
    }

    async public Task<string> GetArtistInfo(string id) {
      var Url = $"/artists/{id}";
      return await GetAsync("Search", Url);
    }

    async public Task<string> GetArtistSongs(string id) {
      var Url = $"/artists/{id}/songs?sort=popularity&per_page=10";
      return await GetAsync("Search", Url);
    }

    // Set request URI and optional bearer token from config.
    async Task<string> GetAsync(string service, string endpoint) {
      var Client = new HttpClient();
      var Host = Config.GetValue<string>($"{service}:host");
      Client.BaseAddress = new Uri($"{Host}{endpoint}");
      var Token = Config.GetValue<string>($"{service}:token");
      if (Token != null) Client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", Token);
      return await RequestDataAsync(Client);
    }

    async Task<string> RequestDataAsync(HttpClient client) {
      try {
        var Response = await client.GetAsync("");
        Response.EnsureSuccessStatusCode();
        return await Response.Content.ReadAsStringAsync();
      } catch (HttpRequestException error) {
        Console.WriteLine(error);
        return new {
          error = true,
          response = error.ToString()
        }.ToString();
      }
    }

  }
}