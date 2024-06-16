using Maio11_Best.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PagedList.Mvc;
using PagedList;
using OfficeOpenXml;

namespace Maio11_Best.Controllers
{
    public class TrainerController : Controller
    {
        // GET: Player
        public ActionResult TrainerList(string msg)
        {
            using (DbModel db = new DbModel())
            {
                ViewBag.msg = msg;
                List<trainer> trainers = db.trainers.Include(p => p.Team).ToList();
                return View(trainers);
            }

        }

        public ActionResult GetTrainers()
        {
            using (DbModel db = new DbModel())
            {
                List<MyTrainer> trainers = db.trainers
                     .Include(p => p.Team)
                     .Select(m => new MyTrainer
                     {
                         Id = m.trainer_id,
                         Name = m.trainer_name,
                         CoachingLicense = m.coaching_license,
                         TeamName = m.Team.team_name,
                         PhotoPath = m.photo_path
                     }).ToList();

                return Json(new { data = trainers }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DetailsTrainer(int id)
        {
            using(DbModel db = new DbModel())
            {
                var trainer = db.trainers.Include(p =>p.Team).FirstOrDefault(p => p.trainer_id == (id));

                if (trainer != null)
                {
                    return View(trainer);
                }
                else
                {
                    return RedirectToAction("TrainerList", new { msg = "Registo não existe" });
                }
            }
        }

        public ActionResult InsertTrainer()
        {
            using (DbModel db = new DbModel())
            {
                var teams = db.Teams.Select(t => new { t.team_id, t.team_name }).ToList();
                ViewBag.TeamNames = new SelectList(teams, "team_id", "team_name");
                trainer trainer = new trainer();
                return View(trainer);
            }
        }

        [HttpPost]
        public ActionResult InsertTrainer(trainer newTrainer, HttpPostedFileBase fich)
        {

            using (DbModel db = new DbModel())
            {
                db.trainers.Add(newTrainer);
                db.SaveChanges();
                if (fich != null && fich.FileName.Length > 0 && fich.ContentType.Contains("image"))
                {
                    string path = Server.MapPath("~/fotos/coachs/");
                    string file = newTrainer.trainer_id.ToString() + System.IO.Path.GetExtension(fich.FileName);
                    newTrainer.photo_path = file;
                    path += file;
                    fich.SaveAs(path);
                    db.SaveChanges();
                }
                return RedirectToAction("TrainerList", new { msg = "Inserido com sucesso" });
            }

        }

        public ActionResult DeleteTrainer(int? id)
        {
            using (DbModel db = new DbModel())
            {
                trainer deletedCoach = db.trainers
                                         .Include(p => p.Team)
                                         .FirstOrDefault(p => p.trainer_id == (id ?? -1));

                if (deletedCoach != null)
                {
                    return View(deletedCoach);
                }
                else
                {
                    return RedirectToAction("TrainerList", new { msg = "Registo não existe" });
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DelTrainer(int trainer_id)
        {
            using (DbModel db = new DbModel())
            {
                try
                {
                    trainer trainerToDelete = db.trainers.Find(trainer_id);

                    if (trainerToDelete == null)
                    {
                        return HttpNotFound();
                    }

                    db.trainers.Remove(trainerToDelete);
                    db.SaveChanges();

                    return RedirectToAction("TrainerList");
                }
                catch (Exception ex)
                {
                    // Handle any errors that occur during deletion
                    ViewBag.ErrorMsg = "Error deleting Trainer: " + ex.Message;
                    return View(); // Optionally, you can return a view with an error message
                }
            }
        }

        public ActionResult EditTrainer(int? id)
        {
            using (DbModel db = new DbModel())
            {
                var teams = db.Teams.Select(t => new { t.team_id, t.team_name }).ToList();
                ViewBag.TeamNames = new SelectList(teams, "team_id", "team_name");
                trainer editedTrainer = db.trainers.Find(id ?? -1);
                if (editedTrainer != null)
                {
                    return View(editedTrainer);
                }
                return RedirectToAction("TrainerList", new { msg = "Registo não encontrado" });

            }
        }

        [HttpPost]
        public ActionResult EditTrainer(trainer trainer, HttpPostedFileBase fich)
        {
            using (DbModel db = new DbModel())
            {
                trainer editedTrainer = db.trainers.Find(trainer.trainer_id);
                if (editedTrainer != null)
                {
                    editedTrainer.trainer_name = trainer.trainer_name;
                    editedTrainer.coaching_license = trainer.coaching_license;
                    editedTrainer.team_id = trainer.team_id;
                    if (trainer.photo_path != null && fich == null)
                    {
                        editedTrainer.photo_path = trainer.photo_path;
                    }
                    if (fich != null && fich.FileName.Length > 0 && fich.ContentType.Contains("image"))
                    {

                        string path = Server.MapPath("~/fotos/coachs/");
                        if (editedTrainer.photo_path != null)
                        {
                            string newPath = path + editedTrainer.photo_path;
                            if (System.IO.File.Exists(newPath)) System.IO.File.Delete(newPath);
                        }
                        string ficheiro = editedTrainer.trainer_id.ToString() + System.IO.Path.GetExtension(fich.FileName);
                        editedTrainer.photo_path = ficheiro;
                        path += ficheiro;
                        fich.SaveAs(path);

                    }
                    db.SaveChanges();
                    return RedirectToAction("TrainerList", new { msg = "Registo editado com sucesso" });

                }
                else
                {
                    return RedirectToAction("TrainerList", new { msg = "Registo não encontrado" });
                }
            }
        }

        public void ExportToExcel()
        {
            using (DbModel db = new DbModel())
            {
                List<trainer> trainers = db.trainers.Include(p => p.Team).ToList();

                // Set the license context for EPPlus
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage())
                {

                    var worksheet = package.Workbook.Worksheets.Add("Trainers");
                    // Add headers
                    worksheet.Cells[1, 1].Value = "Trainer ID";
                    worksheet.Cells[1, 2].Value = "Trainer Name";
                    worksheet.Cells[1, 3].Value = "Coaching License";
                    worksheet.Cells[1, 4].Value = "Team ID";
                    worksheet.Cells[1, 5].Value = "Team Name";
                    worksheet.Cells[1, 6].Value = "Photo";

                    for (int i = 0; i < trainers.Count; i++)
                    {
                        worksheet.Cells[i + 2, 1].Value = trainers[i].trainer_id;
                        worksheet.Cells[i + 2, 2].Value = trainers[i].trainer_name;
                        worksheet.Cells[i + 2, 3].Value = trainers[i].coaching_license;
                        worksheet.Cells[i + 2, 4].Value = trainers[i].team_id;
                        worksheet.Cells[i + 2, 5].Value = trainers[i].Team.team_name;
                        worksheet.Cells[i + 2, 6].Value = trainers[i].photo_path;
                    }

                    // Convert to byte array
                    var excelData = package.GetAsByteArray();

                    // Send the file to the browser for download
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=Trainers.xlsx");
                    Response.BinaryWrite(excelData);
                    Response.End();
                }
            }
        }
    }
}