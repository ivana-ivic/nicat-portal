using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NiCATPortal.Models;
using System.IO;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace NiCATPortal.Controllers
{
    public class TeachersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Route("Predavaci")]
        public ActionResult Index(string sortBy = "Name", string sortOrder = "ASC", int page = 1, int pageSize = 2, string search = "")
        {

            var teachers = db.Teachers
                            .Where(x => x.Name.Contains(search))
                            .OrderBy(string.Format("{0} {1}", sortBy, sortOrder));
            //then we get total count
            var count = teachers.Count();
            //then we page the records
            teachers = teachers.Skip((page - 1) * pageSize).Take(pageSize);

            var model = new TeacherGridViewModel()
            {
                SortOrder = sortOrder,
                SortBy = sortBy,
                Page = page,
                PageSize = pageSize,
                Search = search,
                Teachers = teachers.ToList(),
                Count = count
            };

            return View(model);
        }

        [Route("Predavaci/Kurs/{id}")]
        public ActionResult Courses(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // GET: Teacher/Details/5
        [Route("Predavaci/Detaljnije/{id}")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // GET: Teachers/Create
        [Route("Predavaci/DodajPredavaca")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Predavaci/DodajPredavaca")]
        public ActionResult Create([Bind(Include = "Name,LastName,Email,PasswordHash,PhoneNumber")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                var password = new ApplicationUserManager(new UserStore<ApplicationUser>(db)).PasswordHasher.HashPassword(teacher.PasswordHash);
                Teacher newTeacher = new Teacher { Name = teacher.Name, LastName = teacher.LastName, Email = teacher.Email, EmailConfirmed = true, PasswordHash = password, PhoneNumber = teacher.PhoneNumber, UserName = teacher.Email, SecurityStamp = Guid.NewGuid().ToString() };
                db.Teachers.Add(newTeacher);
                db.SaveChanges();
                var teacherManager = new UserManager<Teacher>(new UserStore<Teacher>(db));
                teacherManager.AddToRole(newTeacher.Id, Role.TEACHER);
                return RedirectToAction("Index");
            }

            return View(teacher);
        }

        // GET: Teachers/Edit/5
        [Route("Predavaci/Izmeni/{id}")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Predavaci/Izmeni/{id}")]
        public ActionResult Edit([Bind(Include = "Id,Name,LastName,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = teacher.Id });
            }
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        [Route("Predavaci/Obrisi/{id}")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Predavaci/Obrisi/{id}")]
        public ActionResult DeleteConfirmed(string id)
        {
            Teacher teacher = db.Teachers.Find(id);
            var literature = db.Literature.Where(l => l.Teacher.Id.Equals(teacher.Id)).FirstOrDefault();
            while (literature != null)
            {
                db.Literature.Remove(literature);
                db.SaveChanges();
                literature = db.Literature.Where(l => l.Teacher.Id.Equals(teacher.Id)).FirstOrDefault();
            }
            db.Teachers.Remove(teacher);
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
