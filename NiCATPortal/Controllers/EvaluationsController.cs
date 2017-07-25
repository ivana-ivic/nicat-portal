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
    public class EvaluationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Evaluations
        [Route("Ocene/{id?}")]
        public ActionResult Index(int? courseId)
        {
            if(courseId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var evaluationsResult = db.Evaluations.Where(e => e.Course.Id == courseId);
            var studentResult = db.Courses.Find(courseId).Students.ToList();
            Dictionary<Student, int?> result = new Dictionary<Student, int?>();
            foreach(Student s in studentResult)
            {
                var res = evaluationsResult.Where(e => e.Student.Id.Equals(s.Id)).FirstOrDefault();
                if (res != null)
                {
                    result.Add(s, res.Mark);
                }
                else
                {
                    result.Add(s, null);
                }
            }
            ViewBag.CourseName = db.Courses.Find(courseId).Name;
            ViewBag.CourseId = courseId;
            return View(result);
        }

        // GET: Evaluations/Details/5
        [Route("Ocene/Detaljnije/{id?}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evaluation evaluation = db.Evaluations.Find(id);
            if (evaluation == null)
            {
                return HttpNotFound();
            }
            return View(evaluation);
        }

        // GET: Evaluations/Create
        [Route("Ocene/Oceni/{id?}")]
        public ActionResult Create(string studentId, int? courseId)
        {
            if(studentId==null || courseId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.studentId = studentId;
            ViewBag.courseId = courseId;
            Student s = db.Students.Find(studentId);
            ViewBag.studentName = s.Name + " " + s.LastName;
            ViewBag.courseName = db.Courses.Find(courseId).Name;
            return View();
        }

        // POST: Evaluations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Ocene/Oceni")]
        public ActionResult Create(string studentId, int? courseId, [Bind(Include = "Id,Mark")] Evaluation evaluation)
        {
            if (studentId == null || courseId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                Evaluation evaluationExists = db.Evaluations.Where(e => e.Student.Id.Equals(studentId) && e.Course.Id == courseId).FirstOrDefault();
                if (evaluationExists != null)
                {
                    db.Evaluations.Remove(evaluationExists);
                    db.SaveChanges();
                }
                Evaluation newEvaluation = new Evaluation { Id = evaluation.Id, Mark = evaluation.Mark, Student = db.Students.Find(studentId), Course = db.Courses.Find(courseId) };
                db.Evaluations.Add(newEvaluation);
                db.SaveChanges();
                return RedirectToAction("Index", new { courseId = courseId });
            }
            ViewBag.studentId = studentId;
            ViewBag.courseId = courseId;
            Student s = db.Students.Find(studentId);
            ViewBag.studentName = s.Name + " " + s.LastName;
            ViewBag.courseName = db.Courses.Find(courseId).Name;
            return View(evaluation);
        }

        // GET: Evaluations/Edit/5
        [Route("Ocene/Izmeni/{id?}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evaluation evaluation = db.Evaluations.Find(id);
            if (evaluation == null)
            {
                return HttpNotFound();
            }
            return View(evaluation);
        }

        // POST: Evaluations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Ocene/Izmeni")]
        public ActionResult Edit([Bind(Include = "Id,Mark")] Evaluation evaluation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(evaluation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(evaluation);
        }

        // GET: Evaluations/Delete/5
        [Route("Ocene/Obrisi/{id?}")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evaluation evaluation = db.Evaluations.Find(id);
            if (evaluation == null)
            {
                return HttpNotFound();
            }
            return View(evaluation);
        }

        // POST: Evaluations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Ocene/Obrisi/{id}")]
        public ActionResult DeleteConfirmed(int id)
        {
            Evaluation evaluation = db.Evaluations.Find(id);
            db.Evaluations.Remove(evaluation);
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
