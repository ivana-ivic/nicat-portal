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
using System.Data.Entity.Infrastructure;

namespace NiCATPortal.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Route("Kursevi")]
        public ActionResult Index(string sortBy = "Name", string sortOrder = "ASC", int page = 1, int pageSize = 2, string search = "")
        {
            var courses = db.Courses
                            .Where(x => x.Name.Contains(search))
                            .OrderBy(string.Format("{0} {1}", sortBy, sortOrder));
            //then we get total count
            var count = courses.Count();
            //then we page the records
            courses = courses.Skip((page - 1) * pageSize).Take(pageSize);

            var model = new CourseGridViewModel()
            {
                SortOrder = sortOrder,
                SortBy = sortBy,
                Page = page,
                PageSize = pageSize,
                Search = search,
                Courses = courses.ToList(),
                Count = count
            };

            return View(model);
        }
        [Route("Kursevi/Spisak/{id}")]
        public ActionResult UserCourses(string userId)
        {
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Course> courses = new List<Course>();
            var user = db.Users.FirstOrDefault(t => t.Id.Equals(userId));
            if(user is Teacher)
            {
                courses = (user as Teacher).Courses.ToList();
            }
            else
            {
                courses = (user as Student).Courses.ToList();
            }
            ViewBag.userId = userId;
            return View(courses);
        }

        [Route("Kursevi/Lista/{id?}")]
        public ActionResult CourseUsersList(int? id, string usersType)
        {
            if (id == null || usersType == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            ViewBag.CourseName = course.Name;
            if (usersType.Equals("Students"))
            {
                List<Student> res = course.Students.ToList();
                ViewBag.UsersType = "Studenti";
                return View(res);
            }
            else
            {
                List<Teacher> res = course.Teachers.ToList();
                ViewBag.UsersType = "Predavači";
                return View(res);
            }
        }

        //public ActionResult TeachersList(string userId)
        //{
        //    if (userId == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var courses = (db.Users.FirstOrDefault(t => t.Id.Equals(userId)) as Teacher).Courses.ToList();
        //    return View(courses);
        //}
        [Route("Kursevi/DodajPolaznika/{id?}")]
        public ActionResult AddUserToCourse(int? id, string userId)
        {
            if (userId == null || id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var course = db.Courses.Find(id);
            var user = db.Users.FirstOrDefault(t => t.Id.Equals(userId));
            if(user is Student)
            {
                (user as Student).Courses.Add(course);
            }
            else
            {
                (user as Teacher).Courses.Add(course);
            }
            db.SaveChanges();
            return RedirectToAction("Details", new { id=id, userId=userId});
        }

        [Route("Kursevi/UkloniPolaznika/{id?}")]
        public ActionResult RemoveUserFromCourse(int? id, string userId)
        {
            if (userId == null || id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var course = db.Courses.Find(id);
            var user = db.Users.FirstOrDefault(t => t.Id.Equals(userId));
            if (user is Student)
            {
                (user as Student).Courses.Remove(course);
            }
            else
            {
                (user as Teacher).Courses.Remove(course);
            }
            db.SaveChanges();
            return RedirectToAction("Details", new { id = id, userId = userId });
        }

        // GET: Courses/Details/5
        [Route("Kursevi/Detaljnije/{id?}")]
        public ActionResult Details(int? id, string userId)
        {
            if (id == null || userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            ApplicationUser user = db.Users.FirstOrDefault(s => s.Id.Equals(userId));
            ViewBag.UserContainsCourse = false;
            if (user is Teacher)
            {
                var query1 = from c in db.Courses
                             where c.Teachers.Any(s => s.Id.Equals(userId))
                             select c;
                if(query1.ToList().ToList().Contains(course))
                    ViewBag.UserContainsCourse = true;
            }
            else if(user is Student)
            {
                var query2 = from c in db.Courses
                             where c.Students.Any(s => s.Id.Equals(userId))
                             select c;
                if (query2.ToList().ToList().Contains(course))
                    ViewBag.UserContainsCourse = true;
            }
             
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        [Route("Kursevi/DodajDomaci/{id?}")]
        public ActionResult UploadHomework(int? courseId)
        {
            if (courseId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(courseId);
            return View(course);
        }

        [HttpPost]
        [Route("Kursevi/DodajDomaci")]
        public ActionResult UploadHomework(HttpPostedFileBase fileParameter)
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                int cId = Int32.Parse(Request.Form["courseId"]);
                string uId = Request.Form["userId"];
                Course course = db.Courses.Find(cId);
                //Course updatedCourse = new Course { Id = course.Id, Description = course.Description, Name = course.Name, Year = course.Year };
                //if (course.Literatures == null)
                //{
                //    updatedCourse.Literatures = new List<Literature>();
                //}
                Student usr = db.Students.Find(uId);
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var newHomework = new Homework { FileName = fileName, Course = course, Student = usr };
                    db.Homework.Add(newHomework);
                    //course.Homework.Add(newHomework);
                    db.SaveChanges();
                    var path = Path.Combine(Server.MapPath("~/Files/Homework/"), fileName);
                    file.SaveAs(path);
                }
            }

            return RedirectToAction("Index","Homework",new { courseId = Int32.Parse(Request.Form["courseId"]) });
        }

        [Route("Kursevi/DodajLiteraturu/{id?}")]
        public ActionResult UploadLiterature(int? courseId)
        {
            if (courseId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(courseId);
            return View(course);
        }

        [HttpPost]
        [Route("Kursevi/DodajLiteraturu")]
        public ActionResult UploadLiterature(HttpPostedFileBase fileParameter)
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                int cId = Int32.Parse(Request.Form["courseId"]);
                string uId = Request.Form["userId"];
                Course course = db.Courses.Find(cId);
                Course updatedCourse = new Course { Id = course.Id, Description = course.Description, Name = course.Name, Year = course.Year };
                if (course.Literatures == null)
                {
                    updatedCourse.Literatures = new List<Literature>();
                }
                Teacher usr = db.Teachers.Find(uId);
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var newLiterature = new Literature { FileName=fileName, Course=course, Teacher=usr };
                    course.Literatures.Add(newLiterature);
                    db.Literature.Add(newLiterature);
                    db.SaveChanges();
                    var path = Path.Combine(Server.MapPath("~/Files/Literature/"), fileName);
                    file.SaveAs(path);
                }
            }

            return RedirectToAction("Index", "Literature", new { courseId = Int32.Parse(Request.Form["courseId"]), userId = Request.Form["userId"] });
        }

        // GET: Courses/Create
        [Authorize(Order = 1,Roles = Role.ADMIN)]
        [Route("Kursevi/DodajKurs")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Kursevi/DodajKurs")]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Year")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        // GET: Courses/Edit/5
        [Route("Kursevi/Izmeni/{id?}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Kursevi/Izmeni")]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Year")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        [Route("Kursevi/Obrisi/{id?}")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Kursevi/Obrisi/{id}")]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            var homework = db.Homework.Where(h => h.Course.Id == course.Id).FirstOrDefault();
            while (homework != null)
            {
                db.Homework.Remove(homework);
                db.SaveChanges();
                homework = db.Homework.Where(h => h.Course.Id == course.Id).FirstOrDefault();
            }
            var literature = db.Literature.Where(l => l.Course.Id == course.Id).FirstOrDefault();
            while (literature != null)
            {
                db.Literature.Remove(literature);
                db.SaveChanges();
                literature = db.Literature.Where(l => l.Course.Id == course.Id).FirstOrDefault();
            }
            var evaluation = db.Evaluations.Where(e => e.Course.Id == course.Id).FirstOrDefault();
            while (evaluation != null)
            {
                db.Evaluations.Remove(evaluation);
                db.SaveChanges();
                evaluation = db.Evaluations.Where(e => e.Course.Id == course.Id).FirstOrDefault();
            }
            db.Courses.Remove(course);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                var m = ex.Message;
                var inner = ex.InnerException;
                //// Retrieve the error messages as a list of strings.
                //var errorMessages = ex.EntityValidationErrors
                //        .SelectMany(x => x.ValidationErrors)
                //        .Select(x => x.ErrorMessage);

                //// Join the list to a single string.
                //var fullErrorMessage = string.Join("; ", errorMessages);

                //// Combine the original exception message with the new one.
                //var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                //// Throw a new DbEntityValidationException with the improved exception message.
                //throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
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
