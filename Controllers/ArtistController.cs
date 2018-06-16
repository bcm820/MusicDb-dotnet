using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
      return View();
    }

  }
}