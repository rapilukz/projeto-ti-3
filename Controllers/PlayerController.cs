using Maio11_Best.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Maio11_Best.Controllers
{
    public class PlayerController : Controller
    {
        // GET: Player
        public ActionResult PlayerList(String msg)
        {
            ViewBag.msg = msg;
            using (DbModel db = new DbModel())
            {
                List<player> players = db.players.Include(p => p.Team).ToList();
                return View(players);
            }

        }
    }
}