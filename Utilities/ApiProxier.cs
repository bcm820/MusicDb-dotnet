
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace MusicDb.Utilities {

  public class ApiProxier {

    // Inject config from appsettings.json which stores external API data.
    private readonly IConfiguration Config;
    public ApiProxier(IConfiguration config) => Config = config;

    // Set request URI and optional bearer token from config.
    public async Task<string> GetAsync(string service, string endpoint) {
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