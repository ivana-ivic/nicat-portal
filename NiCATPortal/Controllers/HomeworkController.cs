using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NiCATPortal.Models;

namespace NiCATPortal.Controllers
{
    public class HomeworkController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Homework
        [Route("Domaci")]
        public ActionResult Index(int? courseId)
        {
            if (courseId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.CourseName = db.Courses.Find(courseId);
            ViewBag.courseId = courseId;
            var courseHomework = db.Homework.Where(h => h.Course.Id == courseId);
            return View(courseHomework.ToList());
        }

        [Route("Domaci/Download/{id?}")]
        public ActionResult DownloadHomework(int id)
        {
            Homework l = db.Homework.Find(id);
            string filename = l.FileName;
            return File(Server.MapPath("~/Files/Homework/") + filename, MimeMapping.GetMimeMapping(filename), filename);
        }

        // GET: Homework/Details/5
        [Route("Domaci/Detaljnije/{id?}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Homework homework = db.Homework.Find(id);
            if (homework == null)
            {
                return HttpNotFound();
            }
            return View(homework);
        }

        // GET: Homework/Create
        [Route("Domaci/Dodaj")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Homework/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Domaci/Dodaj")]
        public ActionResult Create([Bind(Include = "Id,FileName")] Homework homework)
        {
            if (ModelState.IsValid)
            {
                db.Homework.Add(homework);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(homework);
        }

        // GET: Homework/Edit/5
        [Route("Domaci/Izmeni/{id?}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Homework homework = db.Homework.Find(id);
            if (homework == null)
            {
                return HttpNotFound();
            }
            return View(homework);
        }

        // POST: Homework/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Domaci/Izmeni")]
        public ActionResult Edit([Bind(Include = "Id,FileName")] Homework homework)
        {
            if (ModelState.IsValid)
            {
                db.Entry(homework).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(homework);
        }

        // GET: Homework/Delete/5
        [Route("Domaci/Obrisi/{id?}")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Homework homework = db.Homework.Find(id);
            if (homework == null)
            {
                return HttpNotFound();
            }
            return View(homework);
        }

        // POST: Homework/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Domaci/Obrisi")]
        public ActionResult DeleteConfirmed(int homeworkId)
        {
            Homework homework = db.Homework.Find(homeworkId);
            int courseId = homework.Course.Id;
            var path = Server.MapPath("~\\Files\\Homework\\" + homework.FileName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            db.Homework.Remove(homework);
            db.SaveChanges();
            return RedirectToAction("Index", new { courseId = courseId });
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
