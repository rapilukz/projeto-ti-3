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

        public ActionResult InsertPlayer()
        {
            using (DbModel db = new DbModel())
            {
                var teams = db.Teams.Select(t => new { t.team_id, t.team_name }).ToList();
                ViewBag.TeamNames = new SelectList(teams, "team_id", "team_name");
                player player = new player();
                return View(player);
            }
        }

        [HttpPost]
        public ActionResult InsertPlayer(player newPlayer)
        {
            
            using (DbModel db = new DbModel())
            {
                db.players.Add(newPlayer);
                db.SaveChanges();
                return RedirectToAction("PlayerList", new { msg="Inserido com sucesso" });
            }

        }
    }
}