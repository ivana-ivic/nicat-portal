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
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace NiCATPortal.Controllers
{
    public class AdministratorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //public ActionResult Index(string sortBy = "Name", string sortOrder = "ASC", int page = 1, int pageSize = 2, string search = "")
        //{

        //    var administrators = db.Administrators
        //                    .Where(x => x.Name.Contains(search))
        //                    .OrderBy(string.Format("{0} {1}", sortBy, sortOrder));
        //    //then we get total count
        //    var count = administrators.Count();
        //    //then we page the records
        //    administrators = administrators.Skip((page - 1) * pageSize).Take(pageSize);

        //     var model = new AdministratorGridViewModel()
        //    {
        //        SortOrder = sortOrder,
        //        SortBy = sortBy,
        //        Page = page,
        //        PageSize = pageSize,
        //        Search = search,
        //        Administrators = administrators.ToList(),
        //        Count = count
        //    };

        //    return View(model);
        //}

        // GET: Administrators/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var administrator = db.Administrators.Find(id);
            if (administrator == null)
            {
                return HttpNotFound();
            }
            return View(administrator);
        }

        public ActionResult UserDetails(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Find(id);
            
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Administrators/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administrators/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,LastName,Email,PhoneNumber")] ApplicationUser appUser)
        {
            string userType = Request.Form["Tip korisnika"].ToString();
            var usr = new ApplicationUser();
            var password = new ApplicationUserManager(new UserStore<ApplicationUser>(db)).PasswordHasher.HashPassword("lozinka");
            if (userType.Equals("Profesor"))
            {
                usr = new Teacher { UserName = appUser.Email, Email = appUser.Email, Name = appUser.Name, LastName = appUser.LastName, PhoneNumber = appUser.PhoneNumber, PasswordHash= password };
                var result = db.Teachers.Add(usr as Teacher);
                db.SaveChanges();
                if (result!=null)
                {
                    var teacherManager = new UserManager<Teacher>(new UserStore<Teacher>(db));
                    teacherManager.AddToRole(usr.Id, Role.TEACHER);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                usr = new Student { UserName = appUser.Email, Email = appUser.Email, Name = appUser.Name, LastName = appUser.LastName, PhoneNumber = appUser.PhoneNumber, PasswordHash = password };
                var result = db.Students.Add(usr as Student);
                db.SaveChanges();
                if (result != null)
                {
                    var studentManager = new UserManager<Student>(new UserStore<Student>(db));
                    studentManager.AddToRole(usr.Id, Role.STUDENT);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index", "Adminitrators");
            //if (ModelState.IsValid)
            //{
            //    db.Users.Add(administrator);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //return View(administrator);
        }

        // GET: Administrators/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrator administrator = db.Users.Find(id) as Administrator;
            if (administrator == null)
            {
                return HttpNotFound();
            }
            return View(administrator);
        }

        // POST: Administrators/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,LastName,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] Administrator administrator)
        {
            if (ModelState.IsValid)
            {
                db.Entry(administrator).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = administrator.Id });
            }
            return View(administrator);
        }

        // GET: Administrators/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrator administrator = db.Users.Find(id) as Administrator;
            if (administrator == null)
            {
                return HttpNotFound();
            }
            return View(administrator);
        }

        // POST: Administrators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Administrator administrator = db.Users.Find(id) as Administrator;
            db.Users.Remove(administrator);
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
