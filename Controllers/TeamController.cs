using Maio11_Best.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Maio11_Best.Controllers
{
    public class TeamController : Controller
    {
        // GET: Team
        public ActionResult ListaCarros(String msg)
        {
            ViewBag.msg = msg;
            using (ProjetoBDEntities db = new ProjetoBDEntities())
            {
                List<Team> teams = db.Teams.ToList();
                return View(teams);
            }

        }
    }
}