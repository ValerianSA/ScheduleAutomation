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
    public class EmployeeController : Controller
    {
        private ScheduleAutomationEntities db = new ScheduleAutomationEntities();

        // GET: Employee
        public ActionResult Index()
        {
            var tblEmployees = db.tblEmployees.Include(t => t.tblRole);
            return View(tblEmployees.ToList());
        }

        // GET: Employee/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblEmployee tblEmployee = db.tblEmployees.Find(id);
            if (tblEmployee == null)
            {
                return HttpNotFound();
            }
            return View(tblEmployee);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            ViewBag.RoleID = new SelectList(db.tblRoles, "RoleID", "Role");
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Username,FirstName,LastName,EmailAddress,RoleID")] tblEmployee tblEmployee)
        {
            if (ModelState.IsValid)
            {
                tblEmployee.id = Guid.NewGuid();
                db.tblEmployees.Add(tblEmployee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoleID = new SelectList(db.tblRoles, "RoleID", "Role", tblEmployee.RoleID);
            return View(tblEmployee);
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblEmployee tblEmployee = db.tblEmployees.Find(id);
            if (tblEmployee == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleID = new SelectList(db.tblRoles, "RoleID", "Role", tblEmployee.RoleID);
            return View(tblEmployee);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Username,FirstName,LastName,EmailAddress,RoleID")] tblEmployee tblEmployee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblEmployee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoleID = new SelectList(db.tblRoles, "RoleID", "Role", tblEmployee.RoleID);
            return View(tblEmployee);
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblEmployee tblEmployee = db.tblEmployees.Find(id);
            if (tblEmployee == null)
            {
                return HttpNotFound();
            }
            return View(tblEmployee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            tblEmployee tblEmployee = db.tblEmployees.Find(id);
            db.tblEmployees.Remove(tblEmployee);
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

        //Function to search for Employee data in Employee table using jquery/ajax
        public JsonResult GetSearchingData(string SearchBy, string SearchValue)
        {
            List<tblEmployee> EmpList = new List<tblEmployee>();
            if(SearchBy == "Username")
            {
                try
                {
                    string Username = Convert.ToString(SearchValue);
                    EmpList = db.tblEmployees.Where(x => x.Username == Username || SearchValue == null).ToList();

                }
                catch (FormatException)
                {
                    Console.WriteLine("{0} is not a Username", SearchValue); 
                }
                return Json(EmpList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                EmpList = db.tblEmployees.Where(x => x.FirstName.Contains(SearchValue) || SearchValue == null).ToList();
                return Json(EmpList, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
