using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ScheduleAutomation;
using ScheduleAutomation.Models;

namespace ScheduleAutomation.Controllers
{

    public class AdminController : Controller
    {
        private static string constr = ConfigurationManager.ConnectionStrings["Connection_ScheduleAutomationDB"].ConnectionString;

        private ScheduleAutomationEntities db = new ScheduleAutomationEntities();

        // GET: Admin
        public ActionResult Index()
        {
            var tblSessions = db.tblSessions.Include(t => t.tblCours);
            return View(tblSessions.ToList());
        }

        // GET: Admin/Details/5
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

        // GET: Admin/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.tblCourses, "CourseID", "CourseCode");
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SessionID,sessionCode,sessionName,StartTime,EndTime,CourseID,SessionCreationDate,sessionStatus")] tblSession tblSession)
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

        // GET: Admin/Edit/5
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

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SessionID,sessionCode,sessionName,StartTime,EndTime,CourseID,SessionCreationDate,sessionStatus")] tblSession tblSession)
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

        // GET: Admin/Delete/5
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

        // POST: Admin/Delete/5
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


        #region Get Pending sessions for a Admin Home screen
        public JsonResult GetPendingSessionList()
        {

            List<AdminSessionListModel> pendingSessionList = new List<AdminSessionListModel>();

            //string query = @"select * 
            //                 from tblSession
            //                 where fk_statusID in ('1', '2') ";

            string query2 = @"SELECT tblSession.*, tblSessionStatus.statusLabel
                              FROM tblSession
                              INNER JOIN tblSessionStatus ON tblSession.fk_statusID=tblSessionStatus.statusID
                              Where tblSession.fk_statusID in (1,2);";

            //string constr = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            //const string constr = @"Data source=L-PC176MXH\SQLEXPRESS;initial catalog=ScheduleAutomation;Integrated Security=SSPI;Pooling=False";

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query2))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            var sessions = new AdminSessionListModel
                            {
                                sessionID = Guid.Parse(sdr["SessionID"].ToString()),
                                sessionCode = sdr["sessionCode"].ToString(),
                                sessionName = sdr["sessionName"].ToString(),
                                startTime = Convert.ToDateTime(sdr["StartTime"]),
                                endTime = Convert.ToDateTime(sdr["EndTime"]),
                                sessionStatus = sdr["statusLabel"].ToString(),
                                sessionCreationDate = Convert.ToDateTime(sdr["SessionCreationDate"]),
                            };

                            pendingSessionList.Add(sessions);

                        }
                        con.Close();
                        var json = JsonConvert.SerializeObject(pendingSessionList);
                        return Json(json, JsonRequestBehavior.AllowGet);
                    }
                }
            }

        }
        #endregion

        #region Get Completed sessions for a Admin Home screen
        public JsonResult GetCompletedSessionList()
        {

            List<AdminSessionListModel> completedSessionList = new List<AdminSessionListModel>();


            string query = @"SELECT tblSession.*, tblSessionStatus.statusLabel
                              FROM tblSession
                              INNER JOIN tblSessionStatus ON tblSession.fk_statusID=tblSessionStatus.statusID
                              Where tblSession.fk_statusID in (3);";

            //string constr = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            //const string constr = @"Data source=L-PC176MXH\SQLEXPRESS;initial catalog=ScheduleAutomation;Integrated Security=SSPI;Pooling=False";

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            var sessions = new AdminSessionListModel
                            {
                                sessionID = Guid.Parse(sdr["SessionID"].ToString()),
                                sessionCode = sdr["sessionCode"].ToString(),
                                sessionName = sdr["sessionName"].ToString(),
                                startTime = Convert.ToDateTime(sdr["StartTime"]),
                                endTime = Convert.ToDateTime(sdr["EndTime"]),
                                sessionStatus = sdr["statusLabel"].ToString(),
                                sessionCreationDate = Convert.ToDateTime(sdr["SessionCreationDate"]),
                            };

                            completedSessionList.Add(sessions);

                        }
                        con.Close();
                        var json = JsonConvert.SerializeObject(completedSessionList);
                        return Json(json, JsonRequestBehavior.AllowGet);
                    }
                }
            }

        }
        #endregion

        #region update session in database


        [HttpPost]
        public JsonResult updateSession(CellSessionListModel model)
        {
            updateSessionModel updatedSession = new updateSessionModel();
            updatedSession.sessionCode = model.sessionCode;
            updatedSession.sessionName = model.sessionName;
            updatedSession.StartTime = model.startTime;
            updatedSession.EndTime = model.endTime;
            updatedSession.SessionId = model.sessionID;
            int updatedsessionStatus = 3;

            string query = @"UPDATE tblSession 
                             SET fk_statusID = '" + updatedsessionStatus +
                             "' WHERE SessionID = '" + updatedSession.SessionId + "'; ";

            try
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        db.SaveChanges();
                        con.Close();
                    }
                }

                using (SqlConnection connection = new SqlConnection(constr))
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;

                    connection.Open();

                    cmd.ExecuteNonQuery();

                    db.SaveChanges();

                    connection.Close();
                }

                //SendMailToAdminsOnUpdate(updatedSession.SessionId, updatedSession.sessionName, updatedSession.sessionCode, (DateTime)updatedSession.StartTime, (DateTime)updatedSession.EndTime);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(model);
        }


        #endregion




    }
}
