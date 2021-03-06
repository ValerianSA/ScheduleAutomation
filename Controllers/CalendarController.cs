﻿

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

namespace ScheduleAutomation.Controllers
{
    public class CalendarController : Controller
    {
        ScheduleAutomationEntities db = new ScheduleAutomationEntities();

        //// GET: Calendar
        //public ActionResult Index()
        //{
        //    dynamic model = new ExpandoObject();
        //    model.Employees = GetEmployees();
        //    return (model);
        //}



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
            const string constr = @"Data Source=L-PC176QQD\SQLEXPRESS;initial catalog=ScheduleAutomation;Integrated Security=SSPI;Pooling=False";

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




        // GET: Calendar
        public ActionResult Index()
        {
            //List<tblEmployee> employees = db.tblEmployees.ToList();
            //List<tblCourses> courses = db.tblCourses.ToList();
            //List<tblRole> roles = db.tblRoles.ToList();
            //List<tblSession> sessions = db.tblSessions.ToList();
            //List<tblSessionEmpLink> sessionEmpParticipation = db.tblSessionEmpLinks.ToList();


            //var ScheduleAutomationTables = from r in roles
            //                               join e in employees on r.RoleID equals e.RoleID into table1
            //                               from e in table1.DefaultIfEmpty()
            //                               join SEmpLink in sessionEmpParticipation on e.id equals SEmpLink.empID into table2
            //                               from SEmpLink in table2.DefaultIfEmpty()
            //                                   //join session in sessions on SEmpLink.sessionID equals session.SessionID into table3
            //                                   //from session in table3.DefaultIfEmpty()
            //                                   //join course in courses on session.CourseID equals course.CourseID into table4
            //                                   //from course in table4.DefaultIfEmpty()
            //                               select new CalendarModel { tblRole = r, tblEmployee = e, tblSessionEmpLink = SEmpLink };
            ////, tblSession = session, tblCourses = course




            //var tblEmployees = db.tblEmployees.Include(t => t.tblRole);
            //return View(tblEmployees.ToList());
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

            empData.ForEach(e => {
                if (!calendarDataPerDay.ContainsKey(e.EmployeeId)){
                        calendarDataPerDay.Add(e.EmployeeId, new List<EmployeeWithSessionViewModel>());
                }

                calendarDataPerDay[e.EmployeeId].Add(e);
            });
            return calendarDataPerDay;
        }



        // GET: Calendar/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Calendar/Create
        public ActionResult Create()
        {
            return View();
        }

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

        // GET: Calendar/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

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

        // GET: Calendar/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

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
    }
}
