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
    public class CourseController : Controller
    {
        private ScheduleAutomationEntities db = new ScheduleAutomationEntities();

        // GET: Course
        public ActionResult Index()
        {
            return View(db.tblCourses.ToList());
        }

        // GET: Course/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCourses tblCourses = db.tblCourses.Find(id);
            if (tblCourses == null)
            {
                return HttpNotFound();
            }
            return View(tblCourses);
        }


        //public ActionResult Session(Guid? id)
        //{
        //    List<tblSession> sessions = db.tblSessions.ToList();
        //    List<tblCourses> courses = db.tblCourses.ToList();
        //    List<string> container = new List<string>();

            
        //    foreach(var item in sessions)
        //    {

        //        if(item.CourseID == id)
        //        {
        //            container.Add(Convert.ToString(item.tblCours.CourseName));
        //            container.Add(Convert.ToString(item.SessionID));
        //            container.Add(Convert.ToString(item.sessionName));
        //            container.Add(Convert.ToString(item.StartTime));
        //            container.Add(Convert.ToString(item.EndTime));
        //        }

        //    }




        //    //if (id == null)
        //    //{
        //    //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    //}
        //    //tblSession tblSession = db.tblSessions.Find(id);
        //    //if (tblSession == null)
        //    //{
        //    //    return HttpNotFound();
        //    //}tblSession
        //    return View(container);
        //}





        // GET: Course/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,CourseCode,CourseName,CourseType,CourseLang")] tblCourses tblCourses)
        {
            if (ModelState.IsValid)
            {
                tblCourses.CourseID = Guid.NewGuid();
                db.tblCourses.Add(tblCourses);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblCourses);
        }

        // GET: Course/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCourses tblCourses = db.tblCourses.Find(id);
            if (tblCourses == null)
            {
                return HttpNotFound();
            }
            return View(tblCourses);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseID,CourseCode,CourseName,CourseType,CourseLang")] tblCourses tblCourses)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblCourses).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblCourses);
        }

        // GET: Course/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCourses tblCourses = db.tblCourses.Find(id);
            if (tblCourses == null)
            {
                return HttpNotFound();
            }
            return View(tblCourses);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            tblCourses tblCourses = db.tblCourses.Find(id);
            db.tblCourses.Remove(tblCourses);
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
