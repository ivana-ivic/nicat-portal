using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NiCATPortal.Models;
using System.IO;
using System.Web.UI;

namespace NiCATPortal.Controllers
{
    public class CVsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CVs
        [Route("CV")]
        public ActionResult Index()
        {
            return View(db.CVs.ToList());
        }

        // GET: CVs/Details/5
        [Route("CV/Detaljnije/{id}")]
        public ActionResult Details(string studentId)
        {
            if (studentId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(studentId);
            if (student.CV == null)
            {
                return RedirectToAction("Create");
            }
            else
            {
                ViewBag.studentId = studentId;
                return View(student.CV);
            }
        }
        [Route("CV/Download/{id}")]
        public ActionResult DownloadCV(int id)
        {
            //UNCOMMENT
            //Student student = (Student)db.CVs.Where(c => c.Id == cvId).Select(c => c.Student);
            //CV cv = db.CVs.First(c => c.Student.Id == student.Id);
            CV cv = db.CVs.Find(id);
            string filename = cv.FileName;
            return File(Server.MapPath("~/Images/") + filename, MimeMapping.GetMimeMapping(filename), filename);
        }

        // GET: CVs/Create
        [Route("CV/Dodaj")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: CVs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("CV/Dodaj")]
        public ActionResult Create(/*[Bind(Include = "Id,FileName")] CV cV*/HttpPostedFileBase fileParameter)
        {
            //if (ModelState.IsValid)
            //{
            //    db.CVs.Add(cV);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                string sId = Request.Form["studentId"];
                Student student = db.Students.Find(sId);
                if (student.CV != null)
                {
                    CV oldCV = db.CVs.Find(student.CV.Id);
                    db.CVs.Remove(oldCV);
                }
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var newCV = new CV { FileName = fileName };
                    student.CV = newCV;
                    db.CVs.Add(newCV);
                    db.SaveChanges();
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    file.SaveAs(path);
                }
            }

            return RedirectToAction("Details", "Students", new { id= Request.Form["studentId"] });
        }

        // GET: CVs/Edit/5
        [Route("CV/Izmeni/{id?}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CV cV = db.CVs.Find(id);
            if (cV == null)
            {
                return HttpNotFound();
            }
            return View(cV);
        }

        // POST: CVs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("CV/Izmeni")]
        public ActionResult Edit([Bind(Include = "Id,FileName")] CV cV)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cV).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cV);
        }

        // GET: CVs/Delete/5
        [Route("CV/Obrisi/{id?}")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CV cV = db.CVs.Find(id);
            if (cV == null)
            {
                return HttpNotFound();
            }
            return View(cV);
        }

        // POST: CVs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("CV/Obrisi/{id?}")]
        public ActionResult DeleteConfirmed(int id)
        {
            CV cV = db.CVs.Find(id);
            db.CVs.Remove(cV);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
