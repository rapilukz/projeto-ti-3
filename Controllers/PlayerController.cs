using Maio11_Best.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using OfficeOpenXml;

namespace Maio11_Best.Controllers
{

    public class PlayerController : Controller
    {
        // GET: Player
        public ActionResult PlayerList(string msg)
        {
            ViewBag.msg = msg;
            using (DbModel db = new DbModel())
            {
                List<player> players = db.players.Include(p => p.Team).ToList();
                return View(players);
            }

        }

        public ActionResult GetPlayers()
        {
            using (DbModel db = new DbModel())
            {
                List<MyPlayer> players = db.players
                     .Include(p => p.Team)
                     .Select(m => new MyPlayer
                     {
                         Id = m.player_id,
                         Name = m.player_name,
                         Position = m.position,
                         Birthdate = m.birthdate,
                         TeamName = m.Team.team_name,
                         PhotoPath = m.photo_path
                     }).ToList();

                return Json(new { data = players }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DetailsPlayer(int id)
        {
            using (DbModel db = new DbModel())
            {
                var player = db.players.Include(p => p.Team).FirstOrDefault(p => p.player_id == id);

                if (player != null)
                {
                    return View(player);
                }
                else
                {
                    return RedirectToAction("PlayerList", new { msg = "Registo não existe" });
                }
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
        public ActionResult InsertPlayer(player newPlayer, HttpPostedFileBase fich)
        {

            using (DbModel db = new DbModel())
            {
                db.players.Add(newPlayer);
                db.SaveChanges();
                if (fich != null && fich.FileName.Length > 0 && fich.ContentType.Contains("image"))
                {
                    string path = Server.MapPath("~/fotos/players/");
                    string file = newPlayer.player_id.ToString() + System.IO.Path.GetExtension(fich.FileName);
                    newPlayer.photo_path = file;
                    path += file;
                    fich.SaveAs(path);
                    db.SaveChanges();
                }
                return RedirectToAction("PlayerList", new { msg = "Inserido com sucesso" });
            }

        }

        public ActionResult DeletePlayer(int? id)
        {
            using (DbModel db = new DbModel())
            {
                player deletedPlayer = db.players
                                         .Include(p => p.Team)
                                         .FirstOrDefault(p => p.player_id == (id ?? -1));
                if (deletedPlayer != null)
                {
                    return View(deletedPlayer);
                }
                else
                {
                    return RedirectToAction("ListaCarros", new { msg = "Registo não existe" });
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DelPlayer(int player_id)
        {
            using (DbModel db = new DbModel())
            {
                try
                {
                    // Find the player by player_id
                    player playerToDelete = db.players.Find(player_id);

                    if (playerToDelete == null)
                    {
                        return HttpNotFound();
                    }

                    // Remove the player from the context
                    db.players.Remove(playerToDelete);
                    db.SaveChanges();

                    // Redirect to PlayerList with a success message
                    return RedirectToAction("PlayerList");
                }
                catch (Exception ex)
                {
                    // Handle any errors that occur during deletion
                    ViewBag.ErrorMsg = "Error deleting player: " + ex.Message;
                    return View(); // Optionally, you can return a view with an error message
                }
            }
        }

        public ActionResult EditPlayer(int? id)
        {
            using (DbModel db = new DbModel())
            {
                var teams = db.Teams.Select(t => new { t.team_id, t.team_name }).ToList();
                ViewBag.TeamNames = new SelectList(teams, "team_id", "team_name");
                player editedPlayer = db.players.Find(id ?? -1);
                if (editedPlayer != null)
                {
                    return View(editedPlayer);
                }
                return RedirectToAction("PlayerList", new { msg = "Registo não encontrado" });

            }
        }

        [HttpPost]
        public ActionResult EditPlayer(player player, HttpPostedFileBase fich)
        {
            using (DbModel db = new DbModel())
            {
                player editedPlayer = db.players.Find(player.player_id);
                if (editedPlayer != null)
                {
                    editedPlayer.player_name = player.player_name;
                    editedPlayer.position = player.position;
                    editedPlayer.birthdate = player.birthdate;
                    editedPlayer.team_id = player.team_id;
                    if (player.photo_path != null && fich == null)
                    {
                        editedPlayer.photo_path = player.photo_path;
                    }
                    if (fich != null && fich.FileName.Length > 0 && fich.ContentType.Contains("image"))
                    {

                        string path = Server.MapPath("~/fotos/players/");
                        if (editedPlayer.photo_path != null)
                        {
                            string newPath = path + editedPlayer.photo_path;
                            if (System.IO.File.Exists(newPath)) System.IO.File.Delete(newPath);
                        }
                        string ficheiro = editedPlayer.player_id.ToString() + System.IO.Path.GetExtension(fich.FileName);
                        editedPlayer.photo_path = ficheiro;
                        path += ficheiro;
                        fich.SaveAs(path);

                    }
                    db.SaveChanges();
                    return RedirectToAction("PlayerList", new { msg = "Registo editado com sucesso" });

                }
                else
                {
                    return RedirectToAction("PlayerList", new { msg = "Registo não encontrado" });
                }
            }
        }

        public void ExportToExcel()
        {
            using (DbModel db = new DbModel())
            {
                List<player> players = db.players.Include(p => p.Team).ToList();

                // Set the license context for EPPlus
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage())
                {

                    var worksheet = package.Workbook.Worksheets.Add("Players");
                    // Add headers
                    worksheet.Cells[1, 1].Value = "Player ID";
                    worksheet.Cells[1, 2].Value = "Player Name";
                    worksheet.Cells[1, 3].Value = "Birthdate";
                    worksheet.Cells[1, 4].Value = "Team ID";
                    worksheet.Cells[1, 5].Value = "Team Name";
                    worksheet.Cells[1, 6].Value = "Photo";

                    for (int i = 0; i < players.Count; i++)
                    {
                        worksheet.Cells[i + 2, 1].Value = players[i].player_id;
                        worksheet.Cells[i + 2, 2].Value = players[i].player_name;
                        worksheet.Cells[i + 2, 3].Value = players[i].birthdate.Value.ToString("yyyy-MM-dd");
                        worksheet.Cells[i + 2, 4].Value = players[i].team_id;
                        worksheet.Cells[i + 2, 5].Value = players[i].Team.team_name;
                        worksheet.Cells[i + 2, 6].Value = players[i].photo_path;
                    }

                    // Convert to byte array
                    var excelData = package.GetAsByteArray();

                    // Send the file to the browser for download
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=Players.xlsx");
                    Response.BinaryWrite(excelData);
                    Response.End();
                }
            }
        }
    }
}