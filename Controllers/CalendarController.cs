

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
using System.Dynamic;
using System.Configuration;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Text;


namespace ScheduleAutomation.Controllers
{
    public class CalendarController : Controller
    {
        private static string constr = ConfigurationManager.ConnectionStrings["Connection_ScheduleAutomationDB"].ConnectionString;
        ScheduleAutomationEntities db = new ScheduleAutomationEntities();


        #region Get all information from the database
        public static List<EmployeeWithSessionViewModel> GetEmployees()
        {

            List<EmployeeWithSessionViewModel> employees = new List<EmployeeWithSessionViewModel>();
            string query = @"  select * from tblEmployees Employees
                                inner join tblRoles Role on Role.RoleID = Employees.RoleID
                                left join tblSessionEmpLink SessionEmpLink on SessionEmpLink.empID = Employees.id
                                left join tblSession Sessions on Sessions.SessionID = SessionEmpLink.sessionID
                                left join tblCourses Courses on Courses.CourseID = Sessions.CourseID
                                where Role.Role = 'Instructor' ";

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
                            var emp = new EmployeeWithSessionViewModel
                            {

                                Username = sdr["Username"].ToString(),
                                FirstName = sdr["FirstName"].ToString(),
                                LastName = sdr["LastName"].ToString(),
                                EmailAddress = sdr["EmailAddress"].ToString(),
                                fk_statusID = sdr["fk_statusID"].ToString(),
                                EmployeeId = Guid.Parse(sdr["Id"].ToString())
                            };

                            if (DBNull.Value != sdr["TypeOfParticipation"])
                            {
                                emp.TypeOfParticipation = sdr["TypeOfParticipation"].ToString();
                            }

                            if (DBNull.Value != sdr["SessionId"])
                            {
                                emp.SessionId = Guid.Parse(sdr["SessionId"].ToString());
                            }

                            if (DBNull.Value != sdr["sessionName"])
                            {
                                emp.sessionName = sdr["sessionName"].ToString();
                            }
                            if (DBNull.Value != sdr["StartTime"])
                            {
                                emp.StartTime = DateTime.Parse(sdr["StartTime"].ToString());
                                emp.EndTime = DateTime.Parse(sdr["EndTime"].ToString());
                            }


                            employees.Add(emp);

                        }
                        con.Close();
                        return employees;
                    }
                }
            }
        }
        #endregion

        #region Get all information to load onto calendar display
        // GET: Calendar
        //[Authorize(Roles = "superadmin")]

        public ActionResult Index()
        {

            return View(GetCalendarCellData());
        }
        public SortedDictionary<Guid, List<EmployeeWithSessionViewModel>> GetCalendarCellData()
        {

            var empData = GetEmployees();

            SortedDictionary<Guid, List<EmployeeWithSessionViewModel>> calendarDataPerDay = new SortedDictionary<Guid, List<EmployeeWithSessionViewModel>>();

            var getDate = DateTime.Now;
            var todayNumb = Convert.ToInt16(getDate.ToString("dd"));

            var firstDayOfTheMonth = new DateTime(getDate.Year, getDate.Month, 1);
            var firstWeekNow = Convert.ToInt16(getDate.ToString("dd")) + 7;
            var lastDayOfTheMonth = firstDayOfTheMonth.AddMonths(1);
            var firstWeek = Convert.ToInt16(firstDayOfTheMonth.ToString("dd")) + 7;

            var numberOfDays = Convert.ToInt16(lastDayOfTheMonth.ToString("dd"));

            var currentMonth = DateTime.Now.ToString("MMMM");


            var currentDate = firstDayOfTheMonth;

            //while (currentDate != lastDayOfTheMonth)
            //{
            //    var nextDay = currentDate.AddDays(1);
            //    if (!calendarDataPerDay.ContainsKey(currentDate))
            //    {
            //        calendarDataPerDay.Add(currentDate, new List<EmployeeWithSessionViewModel>());
            //    }
            //    var empDataForCurrentDate = empData.Where(z => z.StartTime >= currentDate && z.EndTime <= nextDay).ToList();

            //    calendarDataPerDay[currentDate] = empDataForCurrentDate;

            //    currentDate = currentDate.AddDays(1);
            //}

            empData.ForEach(e =>
            {
                if (!calendarDataPerDay.ContainsKey(e.EmployeeId))
                {
                    calendarDataPerDay.Add(e.EmployeeId, new List<EmployeeWithSessionViewModel>());
                }

                calendarDataPerDay[e.EmployeeId].Add(e);
            });
            return calendarDataPerDay;
        }
        #endregion

        #region Calendar details 2
        // GET: Calendar/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        #endregion

        #region Calendar create 1
        // GET: Calendar/Create
        public ActionResult Create()
        {
            return View();
        }
        #endregion

        #region Calendar create 2
        // POST: Calendar/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region Calendar edit 1
        // GET: Calendar/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        #endregion

        #region Calendar edit 2
        // POST: Calendar/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region Calendar delete 1
        // GET: Calendar/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        #endregion

        #region Calendar delete 2
        // POST: Calendar/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region Get List of courses from database
        public JsonResult GetSession()
        {

            List<tblCoursesModel> courses = new List<tblCoursesModel>();
            string query = @"  select * from tblCourses ";

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
                            var course = new tblCoursesModel
                            {
                                CourseID = Guid.Parse(sdr["CourseID"].ToString()),
                                CourseCode = sdr["CourseCode"].ToString(),
                                CourseName = sdr["CourseName"].ToString(),
                                CourseType = sdr["CourseType"].ToString(),
                                CourseLang = sdr["CourseLang"].ToString(),
                            };

                            courses.Add(course);

                        }
                        con.Close();
                        var json = JsonConvert.SerializeObject(courses);
                        return Json(json, JsonRequestBehavior.AllowGet);
                    }
                }
            }

        }
        #endregion

        #region Add new session to database
        [HttpPost]
        public JsonResult addSession(addSessionModel model)
        {
            try
            {
                ScheduleAutomationEntities db = new ScheduleAutomationEntities();
                List<tblCourses> courseList = db.tblCourses.ToList();
                int newSessionStatus = 1;
                //ViewBag.CourseList = new SelectList(courseList,"CourseName");

                tblSession session = new tblSession();
                session.SessionID = Guid.NewGuid();
                session.sessionName = model.CourseName;
                session.StartTime = model.StartTime;
                session.EndTime = model.EndTime;
                session.CourseID = model.CourseID;
                session.sessionCode = model.sessionCode;
                session.SessionCreationDate = DateTime.Now;
                session.fk_statusID = newSessionStatus;
                //Add data to tblSession in database
                db.tblSessions.Add(session);
                db.SaveChanges();

                tblSessionEmpLink sessionEmpLink = new tblSessionEmpLink();
                sessionEmpLink.empID = model.empID;
                sessionEmpLink.sessionID = session.SessionID;
                sessionEmpLink.typeOfParticipation = model.TypeOfParticipation;
                //Add data to tblSessionEmpLinks in database
                db.tblSessionEmpLinks.Add(sessionEmpLink);
                db.SaveChanges();

                //notify admins with email when new session created
                //SendMailToAdmins(DateTime.Now, model.CourseName.ToString(), model.sessionCode.ToString(), model.StartTime, model.EndTime, model.Instructor.ToString(), model.TypeOfParticipation.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(model);
        }
        #endregion

        #region Get sessions for a specific cell
        [HttpPost]
        //[Authorize(Roles = "superadmin")]
        public JsonResult GetCellSessionList(CellSessionListModel model)
        {

            List<CellSessionListModel> cellSessionList = new List<CellSessionListModel>();

            DateTime startOfDay = model.startTime;
            DateTime endOfDay = (model.startTime.AddDays(1).AddMinutes(-1));

            string query = @"select * 
                             from tblSession
                             where SessionID IN (select SessionID from tblSessionEmpLink where empID = '" + model.empID + "' AND StartTime Between'" + startOfDay + "' and '" + endOfDay + "') ";
            
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
                            var sessions = new CellSessionListModel
                            {
                                sessionID = Guid.Parse(sdr["SessionID"].ToString()),
                                sessionCode = sdr["sessionCode"].ToString(),
                                sessionName = sdr["sessionName"].ToString(),
                                startTime = Convert.ToDateTime(sdr["StartTime"]),
                                endTime = Convert.ToDateTime(sdr["EndTime"]),
                                sessionCreationDate = Convert.ToDateTime(sdr["SessionCreationDate"]),

                            };

                            cellSessionList.Add(sessions);

                        }
                        con.Close();
                        var json = JsonConvert.SerializeObject(cellSessionList);
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
            int updatedsessionStatus = 2;

            string query = @"UPDATE tblSession 
                             SET sessionCode = '" + updatedSession.sessionCode +
                             "', sessionName= '" + updatedSession.sessionName + 
                             "', StartTime='" + updatedSession.StartTime + 
                             "', EndTime='" + updatedSession.EndTime + 
                             "', fk_statusID='" + updatedsessionStatus + 
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

                SendMailToAdminsOnUpdate(updatedSession.SessionId, updatedSession.sessionName, updatedSession.sessionCode, (DateTime)updatedSession.StartTime, (DateTime)updatedSession.EndTime);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(model);
        }


        #endregion

        #region Send Email notification to admins on session add
        public static void SendMailToAdmins(DateTime creationDate, string courseName, string sessionCode, DateTime startTime, DateTime endTime, string instructorName, string participationType)
        {
            try
            {
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress(ConfigurationManager.AppSettings["FromEmail"]);
                mail.To.Add("hussain.nundloll@ceridian.com");
                mail.Subject = "New session added notification - Schedule Automation";

                string mailHeader = "***********************************************************************************************************************<br>" + Environment.NewLine +
                                    "THIS IS AN AUTOMATED RESPONSE.<br>" + Environment.NewLine +
                                    "Please do not respond to this email.<br>" + Environment.NewLine +
                                    "The sending address for this email is an automated account. This message is for notification purposes only and should not be replied to.<br>" + Environment.NewLine +
                                    "***********************************************************************************************************************<br><br>";


                string mailBody = "<table>" +
                                  "<tr style='background-color:#10327c;color:#f1f2f2;'><th>Creation Date</th><th>Course Name</th><th>Session Code</th><th>Session name</th><th>Start time</th><th>End time</th><th>Instructor</th><th>Participation Type</th></tr>" +
                                  "<tr>" +
                                  "<td>" + creationDate + "</td>" +
                                  "<td>" + courseName + "</td>" +
                                  "<td>" + sessionCode + "</td>" +
                                  "<td>" + courseName + "</td>" +
                                  "<td>" + startTime + "</td>" +
                                  "<td>" + endTime + "</td>" +
                                  "<td>" + instructorName + "</td>" +
                                  "<td>" + participationType + "</td>" +
                                  "</tr>";
                mail.Body = mailHeader + mailBody;
                mail.IsBodyHtml = true;

                SmtpClient smtpClient = new SmtpClient(ConfigurationManager.AppSettings["eSmtpClient"]);
                smtpClient.Send(mail);
            }

            catch (Exception ex)
            {
                //Instance.Write(MessageType.info, ErroType.Application, ex.Message);
                throw ex;
            }

        }
        #endregion

        #region Send Email notification to admins on session update
        public static void SendMailToAdminsOnUpdate(Guid sessionID, string sessionName, string sessionCode, DateTime startTime, DateTime endTime)
        {
            try
            {
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress(ConfigurationManager.AppSettings["FromEmail"]);
                mail.To.Add("hussain.nundloll@ceridian.com");
                mail.Subject = "Session Details Updated - Schedule Automation";

                string mailHeader = "***********************************************************************************************************************<br>" + Environment.NewLine +
                                    "THIS IS AN AUTOMATED RESPONSE.<br>" + Environment.NewLine +
                                    "Please do not respond to this email.<br>" + Environment.NewLine +
                                    "The sending address for this email is an automated account. This message is for notification purposes only and should not be replied to.<br>" + Environment.NewLine +
                                    "***********************************************************************************************************************<br><br>";
                string borderStyle = "border: 1px solid black;border-style:solid;";
                string mailBody = "<table style='" + borderStyle + "'>" +
                                  "<tr style='background-color:#10327c;color:#f1f2f2;border: 1px solid black;border-style:solid;'><th>Session ID</th><th>Session Code</th><th>Session Name</th><th>Start time</th><th>End time</th><th>Current status</th></tr>" +
                                  "<tr style='"+ borderStyle + "'>" +
                                  "<td style='" + borderStyle + "'>" + sessionID + "</td>" +
                                  "<td style='" + borderStyle + "'>" + sessionCode + "</td>" +
                                  "<td style='" + borderStyle + "'>" + sessionName + "</td>" +
                                  "<td style='" + borderStyle + "'>" + startTime + "</td>" +
                                  "<td style='" + borderStyle + "'>" + endTime + "</td>" +
                                  "<td style='" + borderStyle + "'>Pending</td>" +
                                  "</tr>";
                mail.Body = mailHeader + mailBody;
                mail.IsBodyHtml = true;

                SmtpClient smtpClient = new SmtpClient(ConfigurationManager.AppSettings["eSmtpClient"]);
                smtpClient.Send(mail);
            }

            catch (Exception ex)
            {
                //Instance.Write(MessageType.info, ErroType.Application, ex.Message);
                throw ex;
            }

        }
        #endregion

        #region old email code
        /*
        #region Send Mail to Admins
        //This region demonstrates sending a mail to the admins notifying a session has been added

        public JsonResult SendMailToAdmins()
        {
            bool result = false;
            result = SendEmail("hussain.nundloll@ceridian.com", "Testing Emailsending function", "<p> A new session has been added to the database</br>Please checkout it out by logging in to the calendar site</p>");
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public bool SendEmail(string toEmail, string subject, string emailBody)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("System has entered the mail function successfully");


                string senderEmail = System.Configuration.ConfigurationManager.AppSettings["SenderEmail"].ToString();
                string senderPassword = System.Configuration.ConfigurationManager.AppSettings["SenderPassword"].ToString();

                SmtpClient client = new SmtpClient("smtp.office365.com", 587);
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                MailMessage mailMessage = new MailMessage(senderEmail, toEmail, subject, emailBody);
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = UTF8Encoding.UTF8;
                client.Send(mailMessage);

                System.Diagnostics.Debug.WriteLine("Email should be sent from here");

                return true;

            }

            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
        */
        #endregion

    }
}
