
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MusicDb.Utilities {

  public class ApiProxier {

    // Inject config from appsettings.json which stores external API data.
    private readonly IConfiguration Config;
    public ApiProxier(IConfiguration configuration) => Config = configuration;

    // Set request URI and optional bearer token from config.
    public async Task<ExpandoObject> Get(string service, string endpoint) {
      var Client = new HttpClient();
      var Host = Config.GetValue<string>($"{service}:host");
      Client.BaseAddress = new Uri($"{Host}{endpoint}");
      var Token = Config.GetValue<string>($"{service}:token");
      if (Token != null) Client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", Token);
      return await GetAsync(Client);
    }

    // Parse into an ExpandoObject, which supports type "dynamic"
    // (compiler ignores type and assumes it supports any operation),
    // allowing access to deep-nested values freely.
    async Task<ExpandoObject> GetAsync(HttpClient client) {
      try {
        var Response = await client.GetAsync("");
        Response.EnsureSuccessStatusCode();
        var ResponseString = await Response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ExpandoObject>
          (ResponseString, new ExpandoObjectConverter());
      } catch (HttpRequestException error) {
        Console.WriteLine(error);
        dynamic errorObj = new ExpandoObject();
        errorObj.response = new { error = error.ToString() };
        return errorObj;
      }
    }

  }
}