using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ScheduleAutomation;
using ScheduleAutomation.Models;

namespace ScheduleAutomation.Controllers
{
    public class SessionController : Controller
    {
        private ScheduleAutomationEntities db = new ScheduleAutomationEntities();

        // GET: Session
        public ActionResult Index()
        {
            var tblSessions = db.tblSessions.Include(t => t.tblCours);
            return View(tblSessions.ToList());
        }

        // GET: Session/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSession tblSession = db.tblSessions.Find(id);
            if (tblSession == null)
            {
                return HttpNotFound();
            }
            return View(tblSession);
        }





        // GET: Session/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.tblCourses, "CourseID", "CourseCode");
            return View();
        }

        // POST: Session/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SessionID,sessionCode,sessionName,StartTime,EndTime,CourseID")] tblSession tblSession)
        {
            if (ModelState.IsValid)
            {
                tblSession.SessionID = Guid.NewGuid();
                db.tblSessions.Add(tblSession);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.tblCourses, "CourseID", "CourseCode", tblSession.CourseID);
            return View(tblSession);
        }

        // GET: Session/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSession tblSession = db.tblSessions.Find(id);
            if (tblSession == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.tblCourses, "CourseID", "CourseCode", tblSession.CourseID);
            return View(tblSession);
        }

        // POST: Session/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SessionID,sessionCode,sessionName,StartTime,EndTime,CourseID")] tblSession tblSession)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblSession).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.tblCourses, "CourseID", "CourseCode", tblSession.CourseID);
            return View(tblSession);
        }

        // GET: Session/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSession tblSession = db.tblSessions.Find(id);
            if (tblSession == null)
            {
                return HttpNotFound();
            }
            return View(tblSession);
        }

        // POST: Session/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            tblSession tblSession = db.tblSessions.Find(id);
            db.tblSessions.Remove(tblSession);
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
