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
using System.Data.Entity.Validation;

namespace NiCATPortal.Controllers
{
    public class StudentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Route("Polaznici")]
        public ActionResult Index(string sortBy = "Name", string sortOrder = "ASC", int page = 1, int pageSize = 2, string search = "")
    {

        var students = db.Students
                        .Where(x => x.Name.Contains(search))
                        .OrderBy(string.Format("{0} {1}", sortBy, sortOrder));
        //then we get total count
        var count = students.Count();
        //then we page the records
        students = students.Skip((page - 1) * pageSize).Take(pageSize);

        var model = new StudentGridViewModel()
        {
            SortOrder = sortOrder,
            SortBy = sortBy,
            Page = page,
            PageSize = pageSize,
            Search = search,
            Students = students.ToList(),
            Count = count
        };

        return View(model);
    }

        // GET: Students/Details/5
        [Route("Polaznici/Detaljnije/{id}")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.studentHasCV = student.CV != null ? true : false;
            return View(student);
        }

        // GET: Students/Create
        [Route("Polaznici/DodajPolaznika")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Polaznici/DodajPolaznika")]
        public ActionResult Create([Bind(Include = "Name,LastName,Email,PasswordHash,PhoneNumber")] Student student)
        {
            if (ModelState.IsValid)
            {
                var password = new ApplicationUserManager(new UserStore<ApplicationUser>(db)).PasswordHasher.HashPassword(student.PasswordHash);
                Student newStudent = new Student { Name = student.Name, LastName = student.LastName, Email = student.Email, EmailConfirmed = true, PasswordHash = password, PhoneNumber = student.PhoneNumber, UserName = student.Email, SecurityStamp = Guid.NewGuid().ToString() };
                db.Students.Add(newStudent);
                try
                {
                    db.SaveChanges();
                }
                catch(DbEntityValidationException ex)
                {
                    // Retrieve the error messages as a list of strings.
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Combine the original exception message with the new one.
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    // Throw a new DbEntityValidationException with the improved exception message.
                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }
                var studentManager = new UserManager<Student>(new UserStore<Student>(db));
                studentManager.AddToRole(newStudent.Id, Role.STUDENT);
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Students/Edit/5
        [Route("Polaznici/Izmeni/{id}")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       [Route("Polaznici/Izmeni/{id}")]
        public ActionResult Edit([Bind(Include = "Id,Name,LastName,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Students", new { id = student.Id });
            }
            return View(student);
        }

        // GET: Students/Delete/5
        [Route("Polaznici/Obrisi/{id}")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Polaznici/Obrisi/{id}")]
        public ActionResult DeleteConfirmed(string id)
        {
            Student student = db.Students.Find(id);
            if (student.CV != null)
            {
                db.CVs.Remove(student.CV);
                db.SaveChanges();
            }
            var homework = db.Homework.Where(h => h.Student.Id.Equals(student.Id)).FirstOrDefault();
            while (homework != null)
            {
                db.Homework.Remove(homework);
                db.SaveChanges();
                homework = db.Homework.Where(h => h.Student.Id.Equals(student.Id)).FirstOrDefault();
            }
            var evaluation = db.Evaluations.Where(e => e.Student.Id.Equals(student.Id)).FirstOrDefault();
            while (evaluation != null)
            {
                db.Evaluations.Remove(evaluation);
                db.SaveChanges();
                evaluation = db.Evaluations.Where(e => e.Student.Id.Equals(student.Id)).FirstOrDefault();
            }
            db.Students.Remove(student);
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
