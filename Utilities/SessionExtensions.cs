using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using MusicDb.Models;

namespace MusicDb.Utilities {

  // SessionExtensions adds additional static methods to the Http.ISession class.
  public static class SessionExtensions {

    // Serializes dynamic to store as a string.
    public static void SetDynamic(this ISession session, string key, dynamic data) {
      string serialized = JsonConvert.SerializeObject(data);
      session.SetString(key, serialized);
    }

    // Gets stored string and deserializes into dynamic.
    public static dynamic GetDynamic(this ISession session, string key) {
      string data = session.GetString(key);
      return data == null ? null : JsonConvert.DeserializeObject<dynamic>(data);
    }

  }

}