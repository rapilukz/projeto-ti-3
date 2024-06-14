using Maio11_Best.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Maio11_Best.Controllers
{
    public class TeamController : Controller
    {
        // GET: Team
        public ActionResult TeamList(String msg)
        {
            ViewBag.msg = msg;
            using (DbModel db = new DbModel())
            {
                List<Team> teams = db.Teams.ToList();
                return View(teams);
            }

        }

        public ActionResult InsertTeam()
        {
            using (DbModel model = new DbModel())
            {
                Team newTeam = new Team();
                return View(newTeam);
            }
        }

        [HttpPost]
        public ActionResult InsertTeam(Team newTeam, HttpPostedFileBase fich)
        {

            using (DbModel db = new DbModel())
            {
                db.Teams.Add(newTeam);
                db.SaveChanges();
                if (fich != null && fich.FileName.Length > 0 && fich.ContentType.Contains("image"))
                {
                    string path = Server.MapPath("~/fotos/teams/");
                    string file = newTeam.team_id.ToString() + System.IO.Path.GetExtension(fich.FileName);
                    newTeam.photo_path = file;
                    path += file;
                    fich.SaveAs(path);
                    db.SaveChanges();
                }

                return RedirectToAction("TeamList", new { msg = "Inserido com sucesso" });
            }

        }

        public ActionResult DeleteTeam(int? id)
        {
            using (DbModel db = new DbModel())
            {
                Team deletedTeam = db.Teams.Find(id ?? -1);
   
                if (deletedTeam != null)
                {
                    return View(deletedTeam);
                }
                else
                {
                    return RedirectToAction("TeamList", new { msg = "Registo não existe" });
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DelTeam(int team_id)
        {
            using (DbModel db = new DbModel())
            {
                try
                {
                    Team teamToDelete = db.Teams.Find(team_id);

                    if (teamToDelete == null)
                    {
                        return HttpNotFound();
                    }

                    db.Teams.Remove(teamToDelete);
                    db.SaveChanges();

                    return RedirectToAction("TeamList");
                }
                catch (Exception ex)
                {
                    // Handle any errors that occur during deletion
                    ViewBag.ErrorMsg = "Error deleting team: " + ex.Message;
                    return View(); // Optionally, you can return a view with an error message
                }
            }
        }

        public ActionResult EditTeam(int? id)
        {
            using (DbModel db = new DbModel())
            {
                Team editedTeam = db.Teams.Find(id ?? -1);
                if (editedTeam != null)
                {
                    return View(editedTeam);
                }
                return RedirectToAction("TeamList", new { msg = "Registo não encontrado" });

            }
        }

        [HttpPost]
        public ActionResult EditTeam(Team team, HttpPostedFileBase fich)
        {
            using (DbModel db = new DbModel())
            {
                Team editedTeam = db.Teams.Find(team.team_id);
                if (editedTeam != null)
                {
                    editedTeam.team_name = team.team_name;
                    editedTeam.foundation_year = team.foundation_year;
                    editedTeam.country = team.country;
                    if(team.photo_path != null && fich == null)
                    {
                        editedTeam.photo_path = team.photo_path;
                    }
                    if (fich != null && fich.FileName.Length > 0 && fich.ContentType.Contains("image"))
                    {

                        string path = Server.MapPath("~/fotos/teams/");
                        if (editedTeam.photo_path != null)
                        {
                            string newPath = path + editedTeam.photo_path;
                            if (System.IO.File.Exists(newPath)) System.IO.File.Delete(newPath);
                        }
                        string ficheiro = editedTeam.team_id.ToString() + System.IO.Path.GetExtension(fich.FileName);
                        editedTeam.photo_path = ficheiro;
                        path += ficheiro;
                        fich.SaveAs(path);

                    }
                    db.SaveChanges();
                    return RedirectToAction("TeamList", new { msg = "Registo editado com sucesso" });

                }
                else
                {
                    return RedirectToAction("TeamList", new { msg = "Registo não encontrado" });
                }
            }
        }

    }
}