using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NiCATPortal.Models;
using System.ComponentModel.DataAnnotations;

namespace NiCATPortal.Controllers
{
    [Authorize]
    public class LiteratureController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Literature
        [Route("Literatura")]
        public ActionResult Index(int? courseId, string userId)
        {
            if (courseId == null || userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var result = db.Courses.Where(c => c.Id == courseId).SelectMany(c => c.Literatures).OrderBy(p => p.Id).ToList();
            ApplicationUser user = db.Users.Find(userId);
            Course course = db.Courses.Find(courseId);
            if(user is Teacher)
            {
                if ((user as Teacher).Courses.Contains(course))
                    ViewBag.userContainsCourse = true;
                else
                    ViewBag.userContainsCourse = false;
            }
            else if(user is Student)
            {
                if ((user as Student).Courses.Contains(course))
                    ViewBag.userContainsCourse = true;
                else
                    ViewBag.userContainsCourse = false;
            }
            else
            {
                ViewBag.userContainsCourse = true;
            }

            ViewBag.courseId = courseId;
            return View(result);
        }

        // GET: Literature/Details/5
        [Route("Literatura/Detaljnije/{id?}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Literature literature = db.Literature.Find(id);
            if (literature == null)
            {
                return HttpNotFound();
            }
            return View(literature);
        }

        // GET: Literature/Create
        [Route("Literatura/DodajLiteraturu/{id?}")]
        public ActionResult Create(int? courseId)
        {
            ViewBag.courseId = courseId;
            return View();
        }

        [Route("Literatura/Download/{id?}")]
        public ActionResult DownloadLiterature(int id)
        {
            Literature l = db.Literature.Find(id);
            string filename = l.FileName;
            return File(Server.MapPath("~/Files/Literature/") + filename, MimeMapping.GetMimeMapping(filename), filename);
        }

        // POST: Literature/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Literatura/DodajLiteraturu")]
        public ActionResult Create([Bind(Include = "Id,FileName")] Literature literature)
        {
            if (ModelState.IsValid)
            {
                string uId = Request.Form["User Id"].ToString();
                int cId = Int32.Parse(Request.Form["Course Id"].ToString());
                var course = db.Courses.Find(cId);
                var user = db.Teachers.Find(uId);
                var newLiterature = new Literature { Id = literature.Id, FileName = literature.FileName, Course = course, Teacher = user };
                db.Literature.Add(newLiterature);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(literature);
        }

        // GET: Literature/Edit/5
        [Authorize(Roles = Role.ADMIN)]
        [Authorize(Roles = Role.TEACHER)]
        [Route("Literatura/Izmeni/{id?}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Literature literature = db.Literature.Find(id);
            if (literature == null)
            {
                return HttpNotFound();
            }
            return View(literature);
        }

        // POST: Literature/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Role.ADMIN)]
        [Authorize(Roles = Role.TEACHER)]
        [Route("Literatura/Izmeni")]
        public ActionResult Edit([Bind(Include = "Id,FileName")] Literature literature)
        {
            if (ModelState.IsValid)
            {
                db.Entry(literature).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(literature);
        }

        // GET: Literature/Delete/5
        
        [Route("Literatura/Obrisi/{id?}")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Literature literature = db.Literature.Find(id);
            if (literature == null)
            {
                return HttpNotFound();
            }
            return View(literature);
        }

        // POST: Literature/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Literatura/Obrisi")]
        public ActionResult DeleteConfirmed(int literatureId)
        {
            Literature literature = db.Literature.Find(literatureId);
            int courseId = literature.Course.Id;
            string userId = literature.Teacher.Id;
            var path = Server.MapPath("~\\Files\\Literature\\" + literature.FileName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            db.Literature.Remove(literature);
            db.SaveChanges();
            return RedirectToAction("Index",new { courseId = courseId, userId = userId });
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
