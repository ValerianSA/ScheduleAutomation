using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ScheduleAutomation;

namespace ScheduleAutomation.Controllers
{
    public class RolesController : Controller
    {
        private ScheduleAutomationEntities db = new ScheduleAutomationEntities();

        // GET: Roles
        public ActionResult Index()
        {
            var tblRoles = db.tblRoles;
            return View(tblRoles.ToList());
        }

        //was used to get role details which is now not needed
        // GET: Roles/Details/5

        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    tblRole tblRole = db.tblRoles.Find(id);
        //    if (tblRole == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tblRole);
        //}

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoleID,Role")] tblRole tblRole)
        {
            if (ModelState.IsValid)
            {
                db.tblRoles.Add(tblRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblRole);
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblRole tblRole = db.tblRoles.Find(id);
            if (tblRole == null)
            {
                return HttpNotFound();
            }
            return View(tblRole);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoleID,Role")] tblRole tblRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblRole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblRole);
        }




        // GET: Roles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblRole tblRole = db.tblRoles.Find(id);
            if (tblRole == null)
            {
                return HttpNotFound();
            }
            return View(tblRole);
        }




        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblRole tblRole = db.tblRoles.Find(id);
            db.tblRoles.Remove(tblRole);
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
