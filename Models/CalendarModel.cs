using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ScheduleAutomation.Models
{
    public class CalendarModel
    {

        public tblEmployee tblEmployee { get; set; }
        public tblRole tblRole { get; set; }
        public tblCourses tblCourses { get; set; }
        public tblSession tblSession { get; set; }
        public tblSessionEmpLink tblSessionEmpLink { get; set; }
                                 
    }

    [Table("tblEmployees")]
    public class tblEmployeeModel
    {

        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }

    }
}