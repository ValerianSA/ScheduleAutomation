using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScheduleAutomation.Models
{
    public class updateSessionModel
    {
        public Guid SessionId { get; set; }
        public string sessionCode { get; set; }
        public string sessionName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime sessionCreationDate { get; set; }
        public string sessionStatus { get; set; }
        public Guid empID { get; set; }
        public string Instructor { get; set; }
        public string TypeOfParticipation { get; set; }
        public string CourseName { get; set; }
        public Guid CourseID { get; set; }

    }
}