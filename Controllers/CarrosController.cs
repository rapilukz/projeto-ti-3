using Maio11_Best.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Maio11_Best.Controllers
{
    public class CarrosController : Controller
    {
        // GET: Carros
        public ActionResult ListaCarros(String msg)
        {
            ViewBag.msg= msg;
            using (bdbestEntities db = new bdbestEntities())
            {
                List<v_carros> carros = db.v_carros.ToList();
                return View(carros);
            }
                
        }

        public ActionResult inserircarro()
        {
            using (bdbestEntities db = new bdbestEntities())
            {
                List<marca>marcas = db.marcas.ToList();
                ViewBag.marcas = new SelectList(marcas, "marca1", "marca1");
                carro novo = new carro();
                return View(novo);
            }
        }
        [HttpPost]
        public ActionResult inserircarro(carro novo, HttpPostedFileBase fich)
        {
            using (bdbestEntities db = new bdbestEntities())
            {
                db.carros.Add(novo);
                db.SaveChanges();
                if (fich != null && fich.FileName.Length >0 && fich.ContentType.Contains("image"))
                {
                    string caminho = Server.MapPath("~/fotos/");
                    string ficheiro = novo.idcar.ToString() + System.IO.Path.GetExtension(fich.FileName);
                    novo.fotopath = ficheiro;
                    caminho += ficheiro;
                    fich.SaveAs(caminho);
                    db.SaveChanges();
                }
               
                return RedirectToAction("ListaCarros",new { msg="Inserido com sucesso"});
            }
        }

        public ActionResult apagarcarro(int? id)
        {
            using (bdbestEntities db = new bdbestEntities())
            {
                carro morto = db.carros.Find(id ?? -1);
                if (morto != null)
                {
                    return View(morto);
                }
                else {

                    return RedirectToAction("ListaCarros", new { msg = "Registo não existe" });
                }
            }
        }
        [HttpPost]
        [ActionName("apagarcarro")]
        public ActionResult delcarro (int? id)
        {
            using (bdbestEntities db = new bdbestEntities())
            {
                carro morto = db.carros.Find(id ?? -1);
                if (morto != null)
                {
                   
                    if (morto.fotopath != null)
                    {
                        string camnovo = Server.MapPath("~/fotos/") + morto.fotopath;
                        if (System.IO.File.Exists(camnovo)) System.IO.File.Delete(camnovo);
                    }
                    db.carros.Remove(morto);
                    db.SaveChanges();
                    return RedirectToAction("ListaCarros", new { msg = "Registo Eliminado com sucesso" });
                }
                else
                {

                    return RedirectToAction("ListaCarros", new { msg = "Registo não existe" });
                }
            }
        }

        public ActionResult editarcarro(int ? id)
        {
            using (bdbestEntities db = new bdbestEntities())
            {
                List<marca> marcas = db.marcas.ToList();
                ViewBag.marcas = new SelectList(marcas, "marca1", "marca1");
                carro editado = db.carros.Find(id ?? -1);
                if (editado != null) {
                    return View(editado);
                }
                return RedirectToAction("ListaCarros", new { msg = "Registo não encontrado" });

            }
        }
        [HttpPost]
        public ActionResult editarcarro(carro editado, HttpPostedFileBase fich)
        {
            using (bdbestEntities db = new bdbestEntities())
            {
                carro alterado = db.carros.Find(editado.idcar);
                if (alterado != null)
                {
                    alterado.modelo = editado.modelo;
                    alterado.phora=editado.phora;
                    alterado.marca = editado.marca;
                    alterado.fotopath = editado.fotopath;
                    if (fich != null && fich.FileName.Length > 0 && fich.ContentType.Contains("image"))
                    {

                        string caminho = Server.MapPath("~/fotos/");
                        if (alterado.fotopath != null) { 
                          string camnovo =caminho +  alterado.fotopath;
                          if(System.IO.File.Exists(camnovo)) System.IO.File.Delete(camnovo);
                        }
                        string ficheiro = alterado.idcar.ToString() + System.IO.Path.GetExtension(fich.FileName);
                        alterado.fotopath = ficheiro;
                        caminho += ficheiro;
                        fich.SaveAs(caminho);
                        
                    }
                    db.SaveChanges();
                    return RedirectToAction("ListaCarros", new { msg = "Registo editado com sucesso" });

                }
                else {
                    return RedirectToAction("ListaCarros", new { msg = "Registo não encontrado" });
                }
            }

        }

    }
}